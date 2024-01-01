using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using NLog.Filters;
using PKHeX.Core;
using PKHeX.Drawing;
using PKHeX.Drawing.Misc.Properties;
using PKHeX.Drawing.PokeSprite;
using RaidCrawler.Core.Connection;
using RaidCrawler.Core.Discord;
using RaidCrawler.Core.Structures;
using RaidCrawler.WinForms.SubForms;
using SysBot.Base;
using System;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;
using System.Xml.Linq;
using static RaidCrawler.Core.Structures.Offsets;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ScrollBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace RaidCrawler.WinForms;

public partial class MainWindow : Form
{
    private static CancellationTokenSource Source = new();
    private static CancellationTokenSource DateAdvanceSource = new();

    private static readonly object _connectLock = new();
    private static readonly object _readLock = new();

    private readonly ClientConfig Config;
    private ConnectionWrapperAsync ConnectionWrapper = default!;

    private readonly SwitchConnectionConfig ConnectionConfig;

    private readonly RaidContainer RaidContainer;
    private readonly NotificationHandler Webhook;
    private readonly FomoNotificationHandler FomoWebhook;

    private List<RaidFilter> RaidFilters = [];
    private static readonly Image MapBase = Image.FromStream(
        new MemoryStream(Utils.GetBinaryResource("paldea.png"))
    );
    private static readonly Image MapKitakami = Image.FromStream(
        new MemoryStream(Utils.GetBinaryResource("kitakami.png"))
    );
    private static readonly Image MapBlueberry = Image.FromStream(
        new MemoryStream(Utils.GetBinaryResource("blueberry.png"))
    );
    private static Dictionary<string, float[]>? DenLocationsBase;
    private static Dictionary<string, float[]>? DenLocationsKitakami;
    private static Dictionary<string, float[]>? DenLocationsBlueberry;

    // statistics
    public int StatDaySkipTries = 0;
    public int StatDaySkipSuccess = 0;
    public int StatDaySkipStreak = 0;
    public int StatShinyCount = 0;
    public string formTitle;
    public List<string> Fomo = new();
    public int FomoCount = 0;

    private ulong RaidBlockOffsetBase;
    private ulong RaidBlockOffsetKitakami;
    private ulong RaidBlockOffsetBlueberry;
    private bool IsReading;
    private bool HideSeed;
    private bool ShowExtraMoves;

    private Color DefaultColor;
    private FormWindowState _WindowState;
    private readonly Stopwatch stopwatch = new();
    private TeraRaidView? teraRaidView;

    private bool StopAdvances =>
        !Config.EnableFilters || RaidFilters.Count == 0 || RaidFilters.All(x => !x.Enabled);

    public MainWindow()
    {
        Config = new ClientConfig();
#if DEBUG
        var date = File.GetLastWriteTime(AppContext.BaseDirectory);
        var build = $" (dev-{date:yyyyMMdd})";
#else
        var build = "";
#endif
        var v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version!;
        // check for all files of pattern filter*.json
        // load them all into a list of RaidFilters

        DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

        var searchPattern = "filter*.json";
        var filterFiles =
            di.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
        for (int i = 0; i < filterFiles.Length; i++)
        {
            var filterPath = filterFiles[i];
            var filter = JsonSerializer.Deserialize<List<RaidFilter>>(File.ReadAllText(filterPath)) ?? [];
            RaidFilters.AddRange(filter);
        }

        // var filterPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "filters.json");
        // if (File.Exists(filterPath))
        //     RaidFilters = JsonSerializer.Deserialize<List<RaidFilter>>(File.ReadAllText(filterPath)) ?? [];
        DenLocationsBase = JsonSerializer.Deserialize<Dictionary<string, float[]>>(Utils.GetStringResource("den_locations_base.json") ?? "{}");
        DenLocationsKitakami = JsonSerializer.Deserialize<Dictionary<string, float[]>>(Utils.GetStringResource("den_locations_kitakami.json") ?? "{}");
        DenLocationsBlueberry = JsonSerializer.Deserialize<Dictionary<string, float[]>>(Utils.GetStringResource("den_locations_blueberry.json") ?? "{}");

        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        if (File.Exists(configPath))
        {
            var text = File.ReadAllText(configPath);
            Config = JsonSerializer.Deserialize<ClientConfig>(text)!;
        }
        else
        {
            Config = new();
        }

        ConnectionConfig = new SwitchConnectionConfig
        {
            Protocol = SwitchProtocol.WiFi,
            IP = "192.168.0.0",
            Port = 6000,
        };
        formTitle = $"RaidCrawler v{v.Major}.{v.Minor}.{v.Build}{build} {Config.InstanceName}";
        Text = formTitle;

        // load raids
        RaidContainer = new(Config.Game);

        SpriteBuilder.ShowTeraThicknessStripe = 0x4;
        SpriteBuilder.ShowTeraOpacityStripe = 0xAF;
        SpriteBuilder.ShowTeraOpacityBackground = 0xFF;
        SpriteUtil.ChangeMode(SpriteBuilderMode.SpritesArtwork5668);

        var protocol = Config.Protocol;
        ConnectionConfig = new()
        {
            IP = Config.IP,
            Port = protocol is SwitchProtocol.WiFi ? 6000 : Config.UsbPort,
            Protocol = Config.Protocol,
        };

        Webhook = new(Config);
        FomoWebhook = new(Config);
        InitializeComponent();

        btnOpenMap.Enabled = false;
        Rewards.Enabled = false;
        CopyAnnounce.Enabled = false;
        SendScreenshot.Enabled = false;
        ButtonScreenState.Enabled = false;
        CheckEnableFilters.Checked = Config.EnableFilters;

        if (Config.Protocol is SwitchProtocol.USB)
        {
            InputSwitchIP.Visible = false;
            LabelSwitchIP.Visible = false;
            USB_Port_TB.Visible = true;
            USB_Port_label.Visible = true;
        }
        else
        {
            InputSwitchIP.Visible = true;
            LabelSwitchIP.Visible = true;
            USB_Port_TB.Visible = false;
            USB_Port_label.Visible = false;
        }
    }

    private void UpdateStatus(string status)
    {
        ToolStripStatusLabel.Text = status;
    }

    private void ButtonEnable(bool enable, params object[] obj)
    {
        lock (_readLock)
        {
            foreach (object o in obj)
            {
                if (o is not Button btn)
                    continue;

                if (InvokeRequired)
                    Invoke(() => btn.Enabled = enable);
                else
                    btn.Enabled = enable;
            }

            IsReading = !enable;
        }
    }

    private void ShowDialog(object obj)
    {
        var window = (Form?)obj;
        if (window is null)
            return;

        window.StartPosition = FormStartPosition.CenterParent;
        if (InvokeRequired)
            Invoke(() => window.ShowDialog());
        else
            window.ShowDialog();
    }

    private int GetRaidBoost()
    {
        if (InvokeRequired)
            return Invoke(() => RaidBoost.SelectedIndex);
        return RaidBoost.SelectedIndex;
    }

    public int GetStatDaySkipTries() => StatDaySkipTries;

    public int GetStatDaySkipSuccess() => StatDaySkipSuccess;
    public int GetStatDaySkipStreak() => StatDaySkipStreak;
    public int GetStatShinyCount() => StatShinyCount;
    public List<string> GetFomo() => Fomo;

    private void MainWindow_Load(object sender, EventArgs e)
    {
        Location = Config.Location;
        if (Location.X == 0 && Location.Y == 0)
            CenterToScreen();
        InputSwitchIP.Text = Config.IP;
        Protocol_dropdown.SelectedIndex = (int)Config.Protocol;
        USB_Port_TB.Text = Config.UsbPort.ToString();
        DefaultColor = IVs.BackColor;
        RaidBoost.SelectedIndex = 0;
        ToggleStreamerView();
    }

    private void InputSwitchIP_Changed(object sender, EventArgs e)
    {
        TextBox textBox = (TextBox)sender;
        Config.IP = textBox.Text;
        ConnectionConfig.IP = textBox.Text;
    }

    private void USB_Port_Changed(object sender, EventArgs e)
    {
        if (Config.Protocol is SwitchProtocol.WiFi)
            return;

        TextBox textBox = (TextBox)sender;
        if (int.TryParse(textBox.Text, out int port) && port >= 0)
        {
            Config.UsbPort = port;
            ConnectionConfig.Port = port;
            return;
        }

        Task.Run(async () => await this.DisplayMessageBox(Webhook, "Please enter a valid numerical USB port.", Source.Token).ConfigureAwait(false), Source.Token);
    }

    private void ButtonConnect_Click(object sender, EventArgs e)
    {
        lock (_connectLock)
        {
            if (ConnectionWrapper is { Connected: true })
                return;

            ConnectionWrapper = new(ConnectionConfig, UpdateStatus);
            Connect(Source.Token);
        }
    }

    private void Connect(CancellationToken token)
    {
        Task.Run(
            async () =>
            {
                ButtonEnable(false, ButtonConnect, SendScreenshot, btnOpenMap, Rewards, CopyAnnounce, ButtonScreenState);
                try
                {
                    (bool success, string err) = await ConnectionWrapper
                        .Connect(token)
                        .ConfigureAwait(false);
                    if (!success)
                    {
                        ButtonEnable(true, ButtonConnect);
                        await this
                            .DisplayMessageBox(Webhook, err, token)
                            .ConfigureAwait(false);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ButtonEnable(true, ButtonConnect);
                    await this
                        .DisplayMessageBox(Webhook, ex.Message, token)
                        .ConfigureAwait(false);
                    return;
                }

                UpdateStatus("Detecting game version");
                string id = await ConnectionWrapper.Connection
                    .GetTitleID(token)
                    .ConfigureAwait(false);
                var game = id switch
                {
                    ScarletID => "Scarlet",
                    VioletID => "Violet",
                    _ => "",
                };

                if (game is "")
                {
                    try
                    {
                        (bool success, string err) = await ConnectionWrapper
                            .DisconnectAsync(token)
                            .ConfigureAwait(false);
                        if (!success)
                        {
                            ButtonEnable(true, ButtonConnect);
                            await this
                                .DisplayMessageBox(Webhook, err, token)
                                .ConfigureAwait(false);
                            return;
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                    finally
                    {
                        ButtonEnable(true, ButtonConnect);
                        await this
                            .DisplayMessageBox(Webhook,
                                "Unable to detect Pokémon Scarlet or Pokémon Violet running on your Switch",
                                token
                            )
                            .ConfigureAwait(false);
                    }
                    return;
                }

                Config.Game = game;
                RaidContainer.SetGame(Config.Game);

                UpdateStatus("Reading story progress");
                Config.Progress = await ConnectionWrapper
                    .GetStoryProgress(token)
                    .ConfigureAwait(false);
                Config.EventProgress = Math.Min(Config.Progress, 3);

                UpdateStatus("Reading event raid status");
                try
                {
                    await ReadEventRaids(token).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    ButtonEnable(true, ButtonConnect);
                    await this.DisplayMessageBox(Webhook, $"Error occurred while reading event raids: {ex.Message}", token)
                        .ConfigureAwait(false);
                    return;
                }

                UpdateStatus("Reading raids");
                try
                {
                    await ReadRaids(token).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    ButtonEnable(true, ButtonConnect);
                    await this.DisplayMessageBox(Webhook, $"Error occurred while reading raids: {ex.Message}", token)
                        .ConfigureAwait(false);
                    return;
                }

                ButtonEnable(true, ButtonAdvanceDate, ButtonReadRaids, ButtonDisconnect, ButtonViewRAM, ButtonDownloadEvents, SendScreenshot, btnOpenMap, Rewards, CopyAnnounce, ButtonScreenState, ButtonResetTime);
                if (InvokeRequired)
                {
                    Invoke(() =>
                    {
                        ComboIndex.Enabled = true;
                        ComboIndex.SelectedIndex = 0;
                    });
                }
                else
                {
                    ComboIndex.SelectedIndex = 0;
                }

                UpdateStatus("Completed!");
            },
            token
        );
    }

    private void Disconnect_Click(object sender, EventArgs e)
    {
        lock (_connectLock)
        {
            if (ConnectionWrapper is not { Connected: true })
                return;

            Disconnect(Source.Token);
        }
    }

    private void Disconnect(CancellationToken token)
    {
        Task.Run(
            async () =>
            {
                ButtonEnable(false, ButtonAdvanceDate, ButtonReadRaids, ButtonDisconnect, ButtonViewRAM, ButtonDownloadEvents, SendScreenshot, ButtonResetTime);
                try
                {
                    (bool success, string err) = await ConnectionWrapper
                        .DisconnectAsync(token)
                        .ConfigureAwait(false);
                    if (!success)
                        await this.DisplayMessageBox(Webhook, err, token).ConfigureAwait(false); }
                catch (Exception ex)
                {
                    await this
                        .DisplayMessageBox(Webhook, ex.Message, token)
                        .ConfigureAwait(false);
                }

                await Source.CancelAsync();
                await DateAdvanceSource.CancelAsync();
                Source = new();
                DateAdvanceSource = new();
                RaidBlockOffsetBase = 0;
                ButtonEnable(true, ButtonConnect);
            },
            token
        );
    }

    private void ButtonPrevious_Click(object sender, EventArgs e)
    {
        var count = RaidContainer.GetRaidCount();
        if (count > 0)
        {
            var index = (ComboIndex.SelectedIndex + count - 1) % count; // Wrap around
            if (ModifierKeys == Keys.Shift)
            {
                for (int i = 0; i < count; i++)
                {
                    var chk = (index + count - i) % count;
                    if (
                        StopAdvances
                        || RaidFilters.Any(
                            z =>
                                z.FilterSatisfied(
                                    RaidContainer,
                                    RaidContainer.Encounters[chk],
                                    RaidContainer.Raids[chk],
                                    RaidBoost.SelectedIndex
                                )
                        )
                    )
                    {
                        index = chk;
                        break;
                    }
                }
            }
            ComboIndex.SelectedIndex = index;
        }
    }

    private void ButtonNext_Click(object sender, EventArgs e)
    {
        var count = RaidContainer.GetRaidCount();
        if (count > 0)
        {
            var index = (ComboIndex.SelectedIndex + count + 1) % count; // Wrap around
            if (ModifierKeys == Keys.Shift)
            {
                for (int i = 0; i < count; i++)
                {
                    var chk = (index + count + i) % count;
                    if (StopAdvances || RaidFilters.Any(z => z.FilterSatisfied(RaidContainer, RaidContainer.Encounters[chk], RaidContainer.Raids[chk], RaidBoost.SelectedIndex)))
                    {
                        index = chk;
                        break;
                    }
                }
            }
            ComboIndex.SelectedIndex = index;
        }
    }

    private void ButtonAdvanceDate_Click(object sender, EventArgs e)
    {
        if (ConnectionWrapper is not { Connected: true })
            return;

        ButtonAdvanceDate.Visible = false;
        StopAdvance_Button.Visible = true;
        Task.Run(async () => await AdvanceDateClick(DateAdvanceSource.Token).ConfigureAwait(false), Source.Token);
    }

    private async Task AdvanceDateClick(CancellationToken token)
    {
        try
        {
            ButtonEnable(false, ButtonViewRAM, ButtonAdvanceDate, ButtonDisconnect, ButtonDownloadEvents, SendScreenshot, ButtonReadRaids);
            Invoke(() => Label_DayAdvance.Visible = true);
            SearchTimer.Start();
            stopwatch.Reset();
            stopwatch.Start();
            StatDaySkipTries = 0;
            StatDaySkipSuccess = 0;
            StatDaySkipStreak = 0;
            StatShinyCount = 0;
            _WindowState = WindowState;

            // Clear shinies missed tooltip
            Fomo.Clear();

            var advanceTextInit = $"Skip Rate: {GetStatDaySkipSuccess()}/{GetStatDaySkipTries()}";
            var missInit = $"Total Miss: {GetStatDaySkipTries() - GetStatDaySkipSuccess()}";
            var streakInit = $"Streak: {GetStatDaySkipStreak()}";
            var shinyTextInint = $"Shinies Missed: {GetStatShinyCount()}";
            var fomoInit = GetFomo();
            Invoke(() => FomoTip.SetToolTip(LabelShinyCount, string.Join(Environment.NewLine, fomoInit)));
            Invoke(() => DaySkipSuccessRate.Text = advanceTextInit);
            Invoke(() => TotalMiss.Text = missInit);
            Invoke(() => Streak.Text = streakInit);
            Invoke(() => LabelShinyCount.Text = shinyTextInint);
            if (teraRaidView is not null)
                Invoke(() => teraRaidView.DaySkips.Text = advanceTextInit);

            var stop = false;
            var raids = RaidContainer.Raids;
            int skips = 0;

            while (!stop)
            {
                bool streamer = Config.StreamerView && teraRaidView is not null;
                Action<int>? action = streamer ? teraRaidView!.UpdateProgressBar : null;

                if (skips >= Config.SystemReset)
                {
                    // When raids are generated, the game determines raids for both the current and next day.
                    // In order to avoid rescanning the same raids on a reset, save the game before reset.
                    // The only exception to this is when FoMO saves are turned on. In which case, don't
                    // save in order to preserve the last found FoMO raid.
                    if (!Config.SaveOnFomo)
                    {
                        await ConnectionWrapper.SaveGame(Config, token).ConfigureAwait(false);
                    }

                    await ConnectionWrapper.CloseGame(token).ConfigureAwait(false);
                    await ConnectionWrapper.StartGame(Config, token).ConfigureAwait(false);

                    RaidBlockOffsetBase = 0;
                    RaidBlockOffsetKitakami = 0;
                    RaidBlockOffsetBlueberry = 0;
                    skips = 0;

                    // Read the initial raids upon reopening the game to correctly detect if the next advance fails
                    await ReadRaids(token).ConfigureAwait(false);
                    raids = RaidContainer.Raids;
                }

                var previousSeeds = raids.Select(z => z.Seed).ToList();
                UpdateStatus("Changing date");
                await ConnectionWrapper
                    .AdvanceDate(Config, skips, token, action)
                    .ConfigureAwait(false);
                await ReadRaids(token).ConfigureAwait(false);
                raids = RaidContainer.Raids;

                Invoke(DisplayRaid);
                if (streamer)
                    Invoke(DisplayPrettyRaid);

                stop = StopAdvanceDate(previousSeeds);

                skips++;
                var advanceText = $"Skip Rate: {GetStatDaySkipSuccess()}/{GetStatDaySkipTries()}";
                var miss = $"Total Miss: {GetStatDaySkipTries() - GetStatDaySkipSuccess()}";
                var streak = $"Streak: {GetStatDaySkipStreak()}";
                var shinyText = $"Shinies Missed: {GetStatShinyCount()}";
                var fomo = GetFomo();
                Invoke(() => FomoTip.SetToolTip(LabelShinyCount, string.Join(Environment.NewLine, fomo)));
                Invoke(() => DaySkipSuccessRate.Text = advanceText);
                Invoke(() => TotalMiss.Text = miss);
                Invoke(() => Streak.Text = streak);
                Invoke(() => LabelShinyCount.Text = shinyText);
                if (teraRaidView is not null)
                    Invoke(() => teraRaidView.DaySkips.Text = advanceText);
            }

            stopwatch.Stop();
            SearchTimer.Stop();
            var timeSpan = stopwatch.Elapsed;
            var timeEmpty = new TimeSpan(0, 0, 0, 0);
            string time = string.Empty;
            if (((int)timeSpan.TotalDays) != timeEmpty.TotalDays) { time = timeSpan.ToString(@"d\d\ %h\h\ mm\m\ ss\s"); }
            else if (((int)timeSpan.TotalHours) != timeEmpty.TotalHours) { time = timeSpan.ToString(@"%h\h\ mm\m\ ss\s"); }
            else if (((int)timeSpan.TotalMinutes) != timeEmpty.TotalMinutes) { time = timeSpan.ToString(@"%m\m\ ss\s"); }
            else { time = timeSpan.ToString(@"%s\s"); }

            if (Config.PlaySound)
                System.Media.SystemSounds.Asterisk.Play();

            if (Config.FocusWindow)
            {
                Invoke(() =>
                {
                    WindowState = _WindowState;
                    Activate();
                });
            }

            if (Config.EnableFilters)
            {
                var encounters = RaidContainer.Encounters;
                var rewards = RaidContainer.Rewards;
                var boost = Invoke(() => RaidBoost.SelectedIndex);
                var satisfiedFilters = new List<(RaidFilter, ITeraRaid, Raid, IReadOnlyList<(int, int, int)>)>();
                for (int i = 0; i < raids.Count; i++)
                {
                    foreach (var filter in RaidFilters)
                    {
                        if (filter is null)
                            continue;

                        if (filter.FilterSatisfied(RaidContainer, encounters[i], raids[i], boost))
                        {
                            satisfiedFilters.Add((filter, encounters[i], raids[i], rewards[i]));
                            if (InvokeRequired)
                                Invoke(() => ComboIndex.SelectedIndex = i);
                            else
                                ComboIndex.SelectedIndex = i;
                        }
                    }
                }

                if (Config.EnableNotification)
                {
                    foreach (var satisfied in satisfiedFilters)
                    {
                        var teraType = satisfied.Item3.GetTeraType(satisfied.Item2);
                        var color = TypeColor.GetTypeSpriteColor((byte)teraType);
                        var hexColor = $"{color.R:X2}{color.G:X2}{color.B:X2}";
                        var blank = new PK9
                        {
                            Species = satisfied.Item2.Species,
                            Form = satisfied.Item2.Form,
                        };

                        var spriteName = GetSpriteNameForUrl(
                            blank,
                            satisfied.Item3.CheckIsShiny(satisfied.Item2)
                        );
                        await Webhook
                            .SendNotification(satisfied.Item2, satisfied.Item3, satisfied.Item1, time, satisfied.Item4, hexColor, spriteName, Source.Token)
                            .ConfigureAwait(false);
                    }
                }

                // Save game on match.
                if (Config.SaveOnMatch && satisfiedFilters.Count > 0)
                    await ConnectionWrapper.SaveGame(Config, token).ConfigureAwait(false);

                if (Config.EnableAlertWindow)
                    await this.DisplayMessageBox(Webhook, $"{Config.AlertWindowMessage}\n\nTime Spent: {time}", token, "Result found!").ConfigureAwait(false);

                if (Config.SearchTimeInTitle)
                    Invoke(() => Text = $"{formTitle} [Match Found in {time}]");
            }
        }
        catch (Exception ex)
        {
            UpdateStatus("Date advance stopped.");
            SearchTimer.Stop();
            if (ex is not TaskCanceledException)
                await ErrorHandler
                    .DisplayMessageBox(this, Webhook, ex.Message, token, "Date Advance Error")
                    .ConfigureAwait(false);
        }

        if (InvokeRequired)
        {
            Invoke(() =>
            {
                ButtonAdvanceDate.Visible = true;
                StopAdvance_Button.Visible = false;
            });
        }
        else
        {
            ButtonAdvanceDate.Visible = true;
            StopAdvance_Button.Visible = false;
        }

        var buttons = new[]
        {
            ButtonViewRAM,
            ButtonAdvanceDate,
            ButtonDisconnect,
            ButtonDownloadEvents,
            SendScreenshot,
            ButtonReadRaids,
        };
        ButtonEnable(true, buttons);
        DateAdvanceSource = new();
    }

    private void StopAdvanceButton_Click(object sender, EventArgs e)
    {
        StopAdvance_Button.Visible = false;
        ButtonAdvanceDate.Visible = true;
        DateAdvanceSource.Cancel();
        DateAdvanceSource = new();
        teraRaidView?.ResetProgressBar();

        stopwatch.Stop();
        SearchTimer.Stop();
    }

    private void ButtonReadRaids_Click(object sender, EventArgs e)
    {
        Task.Run(async () => await ReadRaidsAsync(Source.Token).ConfigureAwait(false), Source.Token);
    }

    private async Task ReadRaidsAsync(CancellationToken token)
    {
        if (IsReading)
        {
            await this
                .DisplayMessageBox(Webhook,
                    "Please wait for the current RAM read to finish.",
                    token
                )
                .ConfigureAwait(false);
            return;
        }

        ButtonEnable(false, ButtonViewRAM, ButtonAdvanceDate, ButtonDisconnect, ButtonDownloadEvents, SendScreenshot, ButtonReadRaids);
        try
        {
            await ReadRaids(token).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await this
                .DisplayMessageBox(Webhook,
                    $"Error occurred while reading raids: {ex.Message}",
                    token
                )
                .ConfigureAwait(false);
        }

        ButtonEnable(true, ButtonViewRAM, ButtonAdvanceDate, ButtonDisconnect, ButtonDownloadEvents, SendScreenshot, ButtonReadRaids);
    }

    private void ViewRAM_Click(object sender, EventArgs e)
    {
        if (IsReading)
        {
            Task.Run(async () => await this.DisplayMessageBox(Webhook, "Please wait for the current RAM read to finish.", Source.Token).ConfigureAwait(false), Source.Token);
            return;
        }

        ButtonEnable(false, ButtonViewRAM);
        RaidBlockViewer window = default!;

        if (ConnectionWrapper is { Connected: true } && ModifierKeys == Keys.Shift)
        {
            try
            {
                var data = ConnectionWrapper.Connection
                    .ReadBytesAbsoluteAsync(
                        RaidBlockOffsetBase,
                        (int)RaidBlock.SIZE_BASE,
                        Source.Token
                    )
                    .Result;
                window = new(data, RaidBlockOffsetBase);
            }
            catch (Exception ex)
            {
                ButtonEnable(true, ButtonViewRAM);
                Task.Run(
                    async () =>
                        await this
                            .DisplayMessageBox(Webhook, ex.Message, Source.Token)
                            .ConfigureAwait(false),
                    Source.Token
                );
                return;
            }
        }
        else if (RaidContainer.Raids.Count > ComboIndex.SelectedIndex)
        {
            var data = RaidContainer.Raids[ComboIndex.SelectedIndex].GetData();
            window = new(data, RaidBlockOffsetBase);
        }

        ShowDialog(window);
        ButtonEnable(true, ButtonViewRAM);
    }

    private void StopFilter_Click(object sender, EventArgs e)
    {
        var form = new FilterSettings(ref RaidFilters);
        ShowDialog(form);
    }

    private void DownloadEvents_Click(object sender, EventArgs e)
    {
        if (ConnectionWrapper is not { Connected: true })
            return;

        if (IsReading)
        {
            Task.Run(async () => await this.DisplayMessageBox(Webhook, "Please wait for the current RAM read to finish.", Source.Token).ConfigureAwait(false), Source.Token);
            return;
        }

        Task.Run(async () => await DownloadEventsAsync(Source.Token).ConfigureAwait(false), Source.Token);
    }

    private async Task DownloadEventsAsync(CancellationToken token)
    {
        ButtonEnable(false, ButtonViewRAM, ButtonAdvanceDate, ButtonDisconnect, ButtonDownloadEvents, SendScreenshot, ButtonReadRaids);
        UpdateStatus("Reading event raid status");

        try
        {
            await ReadEventRaids(token, true).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await this
                .DisplayMessageBox(Webhook,
                    $"Error occurred while reading event raids: {ex.Message}",
                    token
                )
                .ConfigureAwait(false);
        }

        ButtonEnable(true, ButtonViewRAM, ButtonAdvanceDate, ButtonDisconnect, ButtonDownloadEvents, SendScreenshot, ButtonReadRaids);
        UpdateStatus("Completed!");
    }

    private void Seed_Click(object sender, EventArgs e)
    {
        if (ModifierKeys == Keys.Shift && RaidContainer.Raids.Count > ComboIndex.SelectedIndex)
        {
            var raid = RaidContainer.Raids[ComboIndex.SelectedIndex];
            Seed.Text = HideSeed ? $"{raid.Seed:X8}" : "Hidden";
            EC.Text = HideSeed ? $"{raid.EC:X8}" : "Hidden";
            PID.Text =
                (HideSeed ? $"{raid.PID:X8}" : "Hidden")
                + $"{(raid.IsShiny ? " (☆)" : string.Empty)}";
            HideSeed = !HideSeed;
            ActiveControl = null;
        }
    }

    private void ConfigSettings_Click(object sender, EventArgs e)
    {
        var form = new ConfigWindow(Config);
        ShowDialog(form);
    }

    private void EnableFilters_Click(object sender, EventArgs e)
    {
        Config.EnableFilters = CheckEnableFilters.Checked;
    }

    private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
        Config.Location = Location;
        var configpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        JsonSerializerOptions options = new() { WriteIndented = true };
        string output = JsonSerializer.Serialize(Config, options);
        using StreamWriter sw = new(configpath);
        sw.Write(output);

        if (ConnectionWrapper is { Connected: true })
        {
            try
            {
                _ = ConnectionWrapper.DisconnectAsync(Source.Token).Result;
            }
            catch
            {
                // ignored
            }
        }

        Source.Cancel();
        DateAdvanceSource.Cancel();
        Source = new();
        DateAdvanceSource = new();
    }

    private async Task ReadEventRaids(CancellationToken token, bool force = false)
    {
        var priorityFile = Path.Combine(
            Directory.GetCurrentDirectory(),
            "cache",
            "raid_priority_array"
        );
        if (!force && File.Exists(priorityFile))
        {
            var (_, version) = FlatbufferDumper.DumpDeliveryPriorities(
                await File.ReadAllBytesAsync(priorityFile, token)
            );
            var block = await ConnectionWrapper
                .ReadBlockDefault(
                    BCATRaidPriorityLocation,
                    "raid_priority_array.tmp",
                    true,
                    token
                )
                .ConfigureAwait(false);
            var (_, v2) = FlatbufferDumper.DumpDeliveryPriorities(block);
            if (version != v2)
                force = true;

            var tempFile = Path.Combine(
                Directory.GetCurrentDirectory(),
                "cache",
                "raid_priority_array.tmp"
            );
            if (File.Exists(tempFile))
                File.Delete(tempFile);

            if (v2 == 0) // raid reset
                return;
        }

        var deliveryRaidPriorityFlatbuffer = await ConnectionWrapper
            .ReadBlockDefault(BCATRaidPriorityLocation, "raid_priority_array", force, token)
            .ConfigureAwait(false);
        var (groupID, priority) = FlatbufferDumper.DumpDeliveryPriorities(deliveryRaidPriorityFlatbuffer);
        if (priority == 0)
            return;

        var deliveryRaidFlatbuffer = await ConnectionWrapper
            .ReadBlockDefault(BCATRaidBinaryLocation, "raid_enemy_array", force, token)
            .ConfigureAwait(false);
        var deliveryFixedRewardFlatbuffer = await ConnectionWrapper
            .ReadBlockDefault(
                BCATRaidFixedRewardLocation,
                "fixed_reward_item_array",
                force,
                token
            )
            .ConfigureAwait(false);
        var deliveryLotteryRewardFlatbuffer = await ConnectionWrapper
            .ReadBlockDefault(
                BCATRaidLotteryRewardLocation,
                "lottery_reward_item_array",
                force,
                token
            )
            .ConfigureAwait(false);

        RaidContainer.DistTeraRaids = TeraDistribution.GetAllEncounters(deliveryRaidFlatbuffer);
        RaidContainer.MightTeraRaids = TeraMight.GetAllEncounters(deliveryRaidFlatbuffer);
        RaidContainer.DeliveryRaidPriority = groupID;
        RaidContainer.DeliveryRaidFixedRewards = FlatbufferDumper.DumpFixedRewards(
            deliveryFixedRewardFlatbuffer
        );
        RaidContainer.DeliveryRaidLotteryRewards = FlatbufferDumper.DumpLotteryRewards(
            deliveryLotteryRewardFlatbuffer
        );
    }

    private void DisplayRaid()
    {
        int index = ComboIndex.SelectedIndex;
        var raids = RaidContainer.Raids;
        if (raids.Count > index)
        {
            Raid raid = raids[index];
            var encounter = RaidContainer.Encounters[index];

            GameVersionImg.BackgroundImage = (Config.Game == "Violet") ? Properties.Resources.violet : Properties.Resources.scarlet;
            Seed.Text = !HideSeed ? $"{raid.Seed:X8}" : "Hidden";
            EC.Text = !HideSeed ? $"{raid.EC:X8}" : "Hidden";
            PID.Text = GetPIDString(raid, encounter);
            Area.Text = $"{Areas.GetArea((int)(raid.Area - 1), raid.MapParent)} - Den {raid.Den}";
            labelEvent.Visible = raid.IsEvent;

            var teraType = raid.GetTeraType(encounter);
            TeraType.Text = RaidContainer.Strings.types[teraType];

            int starCount = encounter switch
            {
                TeraDistribution => encounter.Stars,
                TeraMight => encounter.Stars,
                _ => raid.GetStarCount(raid.Difficulty, Config.Progress, raid.IsBlack),
            };
            Difficulty.Text = string.Concat(Enumerable.Repeat("☆", starCount));

            ///vio thing
            var map = GenerateMap(raid, teraType);
            statsPanel.BackgroundImage = (Config.MapBackground ? map : null);
            ///vio thing

            var strings = GameInfo.GetStrings(1);
            var param = encounter.GetParam();
            var blank = new PK9 { Species = encounter.Species, Form = encounter.Form };

            Encounter9RNG.GenerateData(blank, param, EncounterCriteria.Unrestricted, raid.Seed);
            var img = blank.Sprite();
            img = ApplyTeraColor((byte)teraType, img, SpriteBackgroundType.BottomStripe);

            var form = Utils.GetFormString(blank.Species, blank.Form, strings);

            Species.Text = $"{RaidContainer.Strings.Species[encounter.Species]}{form}";
            Sprite.Image = img;
            GemIcon.Image = PKHeX.Drawing.Misc.TypeSpriteUtil.GetTypeSpriteGem((byte)teraType);
            Gender.Text = $"{(Gender)blank.Gender}";

            var nature = blank.Nature;
            Nature.Text = $"{RaidContainer.Strings.Natures[nature]}";
            Ability.Text = $"{RaidContainer.Strings.Ability[blank.Ability]}";
            Scale.Text = $"{PokeSizeDetailedUtil.GetSizeRating(blank.Scale)} ({blank.Scale})";

            var extraMoves = new ushort[] { 0, 0, 0, 0 };
            for (int i = 0; i < encounter.ExtraMoves.Length; i++)
            {
                if (i < extraMoves.Length)
                    extraMoves[i] = encounter.ExtraMoves[i];
            }

            Move1.Text = ShowExtraMoves
                ? RaidContainer.Strings.Move[extraMoves[0]]
                : RaidContainer.Strings.Move[encounter.Move1];
            Move2.Text = ShowExtraMoves
                ? RaidContainer.Strings.Move[extraMoves[1]]
                : RaidContainer.Strings.Move[encounter.Move2];
            Move3.Text = ShowExtraMoves
                ? RaidContainer.Strings.Move[extraMoves[2]]
                : RaidContainer.Strings.Move[encounter.Move3];
            Move4.Text = ShowExtraMoves
                ? RaidContainer.Strings.Move[extraMoves[3]]
                : RaidContainer.Strings.Move[encounter.Move4];

            IVs.Text = IVsString(Utils.ToSpeedLast(blank.IVs));
            toolTip.SetToolTip(IVs, IVsString(Utils.ToSpeedLast(blank.IVs), true));

            shinyBox.Image = raid.CheckIsShiny(encounter) ? (ShinyExtensions.IsSquareShinyExist(blank) ? Properties.Resources.square : Properties.Resources.shiny) : null;
            shinyBox.SizeMode = PictureBoxSizeMode.Zoom;

            PID.BackColor = raid.CheckIsShiny(encounter) ? (ShinyExtensions.IsSquareShinyExist(blank) ? Color.FromArgb(125, 255, 135, 0) : Color.FromArgb(125, 255, 215, 0)) : Color.FromArgb(100, 240, 240, 240); //Square - Orange, Shiny - Yellow
            IVs.BackColor = IVs.Text is "31/31/31/31/31/31" ? Color.FromArgb(125, 154, 205, 50) : Color.FromArgb(100, 240, 240, 240); //Green-yellow
            EC.BackColor = (raid.EC % 100 == 0 && (encounter!.Species == 924 || encounter.Species == 206) ? Color.FromArgb(125, 0, 215, 255) : Color.FromArgb(100, 240, 240, 240)); //Cyan
            return;
        }

        var msg = $"Unable to display raid at index {index}. Ensure there are no cheats running or anything else that might shift RAM (Edizon, overlays, etc.), then reboot your console and try again.";
        Task.Run(async () => await this.DisplayMessageBox(Webhook, msg, Source.Token).ConfigureAwait(false), Source.Token);
    }

    private static Image? GetDisplayGemImage(int teratype, Raid raid)
    {
        var shouldDisplayBlack = raid.IsBlack || raid.Flags == 3;
        var baseImg = shouldDisplayBlack
            ? (Image?)Properties.Resources.ResourceManager.GetObject($"black_{teratype:D2}")
            : (Image?)Properties.Resources.ResourceManager.GetObject($"gem_{teratype:D2}");
        if (baseImg is null)
            return null;

        var backlayer = new Bitmap(
            baseImg.Width + 10,
            baseImg.Height + 10,
            baseImg.PixelFormat
        );
        baseImg = ImageUtil.LayerImage(backlayer, baseImg, 5, 5);
        var pixels = ImageUtil.GetPixelData((Bitmap)baseImg);
        for (int i = 0; i < pixels.Length; i += 4)
        {
            if (pixels[i + 3] == 0)
            {
                pixels[i] = 0;
                pixels[i + 1] = 0;
                pixels[i + 2] = 0;
            }
        }

        baseImg = ImageUtil.GetBitmap(
            pixels,
            baseImg.Width,
            baseImg.Height,
            baseImg.PixelFormat
        );
        if (shouldDisplayBlack)
        {
            var color = Color.Indigo;
            SpriteUtil.GetSpriteGlow(baseImg, color.B, color.G, color.R, out var glow);
            baseImg = ImageUtil.LayerImage(
                ImageUtil.GetBitmap(glow, baseImg.Width, baseImg.Height, baseImg.PixelFormat),
                baseImg,
                0,
                0
            );
        }
        else if (raid.IsEvent)
        {
            var color = Color.DarkTurquoise;
            SpriteUtil.GetSpriteGlow(baseImg, color.B, color.G, color.R, out var glow);
            baseImg = ImageUtil.LayerImage(
                ImageUtil.GetBitmap(glow, baseImg.Width, baseImg.Height, baseImg.PixelFormat),
                baseImg,
                0,
                0
            );
        }
        return baseImg;
    }

    private void DisplayPrettyRaid()
    {
        if (teraRaidView is null)
        {
            Task.Run(
                async () =>
                    await this
                        .DisplayMessageBox(Webhook,
                            "Something went terribly wrong: teraRaidView is not initialized.",
                            Source.Token
                        )
                        .ConfigureAwait(false),
                Source.Token
            );
            return;
        }

        int index = ComboIndex.SelectedIndex;
        var raids = RaidContainer.Raids;
        if (raids.Count > index)
        {
            Raid raid = raids[index];
            var encounter = RaidContainer.Encounters[index];

            teraRaidView.Area.Text =
                $"{Areas.GetArea((int)(raid.Area - 1), raid.MapParent)} - Den {raid.Den}";

            var teraType = raid.GetTeraType(encounter);
            teraRaidView.TeraType.Image = (Bitmap)
                Properties.Resources.ResourceManager.GetObject($"gem_text_{teraType}")!;

            int StarCount = encounter switch
            {
                TeraDistribution => encounter.Stars,
                TeraMight => encounter.Stars,
                _ => raid.GetStarCount(raid.Difficulty, Config.Progress, raid.IsBlack),
            };
            teraRaidView.Difficulty.Text = string.Concat(Enumerable.Repeat("⭐", StarCount));

            var param = encounter.GetParam();
            var blank = new PK9 { Species = encounter.Species, Form = encounter.Form };

            Encounter9RNG.GenerateData(blank, param, EncounterCriteria.Unrestricted, raid.Seed);
            var img = blank.Sprite();

            teraRaidView.picBoxPokemon.Image = img;
            var form = Utils.GetFormString(blank.Species, blank.Form, RaidContainer.Strings);

            teraRaidView.Species.Text =
                $"{RaidContainer.Strings.Species[encounter.Species]}{form}";
            teraRaidView.Gender.Text = $"{(Gender)blank.Gender}";

            var nature = blank.Nature;
            teraRaidView.Nature.Text = $"{RaidContainer.Strings.Natures[nature]}";
            teraRaidView.Ability.Text = $"{RaidContainer.Strings.Ability[blank.Ability]}";

            teraRaidView.Move1.Text =
                encounter.Move1 > 0 ? RaidContainer.Strings.Move[encounter.Move1] : "---";
            teraRaidView.Move2.Text =
                encounter.Move2 > 0 ? RaidContainer.Strings.Move[encounter.Move2] : "---";
            teraRaidView.Move3.Text =
                encounter.Move3 > 0 ? RaidContainer.Strings.Move[encounter.Move3] : "---";
            teraRaidView.Move4.Text =
                encounter.Move4 > 0 ? RaidContainer.Strings.Move[encounter.Move4] : "---";

            var length = encounter.ExtraMoves.Length < 4 ? 4 : encounter.ExtraMoves.Length;
            var extraMoves = new ushort[length];
            for (int i = 0; i < encounter.ExtraMoves.Length; i++)
                extraMoves[i] = encounter.ExtraMoves[i];

            teraRaidView.Move5.Text =
                extraMoves[0] > 0 ? RaidContainer.Strings.Move[extraMoves[0]] : "---";
            teraRaidView.Move6.Text =
                extraMoves[1] > 0 ? RaidContainer.Strings.Move[extraMoves[1]] : "---";
            teraRaidView.Move7.Text =
                extraMoves[2] > 0 ? RaidContainer.Strings.Move[extraMoves[2]] : "---";
            teraRaidView.Move8.Text =
                extraMoves[3] > 0 ? RaidContainer.Strings.Move[extraMoves[3]] : "---";

            var ivs = Utils.ToSpeedLast(blank.IVs);

            // HP
            teraRaidView.HP.Text = $"{ivs[0]:D2}";
            teraRaidView.HP.BackColor = Color.FromArgb(0, 5, 25);
            if (teraRaidView.HP.Text is "31")
                teraRaidView.HP.BackColor = Color.ForestGreen;
            else if (teraRaidView.HP.Text is "00")
                teraRaidView.HP.BackColor = Color.DarkRed;

            // ATK
            teraRaidView.ATK.Text = $"{ivs[1]:D2}";
            teraRaidView.ATK.BackColor = Color.FromArgb(0, 5, 25);
            if (teraRaidView.ATK.Text is "31")
                teraRaidView.ATK.BackColor = Color.ForestGreen;
            else if (teraRaidView.ATK.Text is "00")
                teraRaidView.ATK.BackColor = Color.DarkRed;

            // DEF
            teraRaidView.DEF.Text = $"{ivs[2]:D2}";
            teraRaidView.DEF.BackColor = Color.FromArgb(0, 5, 25);
            if (teraRaidView.DEF.Text is "31")
                teraRaidView.DEF.BackColor = Color.ForestGreen;
            else if (teraRaidView.DEF.Text is "00")
                teraRaidView.DEF.BackColor = Color.DarkRed;

            // SPA
            teraRaidView.SPA.Text = $"{ivs[3]:D2}";
            teraRaidView.SPA.BackColor = Color.FromArgb(0, 5, 25);
            if (teraRaidView.SPA.Text is "31")
                teraRaidView.SPA.BackColor = Color.ForestGreen;
            else if (teraRaidView.SPA.Text is "00")
                teraRaidView.SPA.BackColor = Color.DarkRed;

            // SPD
            teraRaidView.SPD.Text = $"{ivs[4]:D2}";
            teraRaidView.SPD.BackColor = Color.FromArgb(0, 5, 25);
            if (teraRaidView.SPD.Text is "31")
                teraRaidView.SPD.BackColor = Color.ForestGreen;
            else if (teraRaidView.SPD.Text is "00")
                teraRaidView.SPD.BackColor = Color.DarkRed;

            // SPEED
            teraRaidView.SPEED.Text = $"{ivs[5]:D2}";
            teraRaidView.SPEED.BackColor = Color.FromArgb(0, 5, 25);
            if (teraRaidView.SPEED.Text is "31")
                teraRaidView.SPEED.BackColor = Color.ForestGreen;
            else if (teraRaidView.SPEED.Text is "00")
                teraRaidView.SPEED.BackColor = Color.DarkRed;

            var map = GenerateMap(raid, teraType);
            if (map is null)
                Task.Run(async () => await this.DisplayMessageBox(Webhook, "Error generating map.", Source.Token).ConfigureAwait(false), Source.Token);
            teraRaidView.Map.Image = map;

            // Rewards
            var rewards = RaidContainer.Rewards[index];

            teraRaidView.textAbilityPatch.Text = "0";
            teraRaidView.textAbilityPatch.ForeColor = Color.DimGray;
            teraRaidView.labelAbilityPatch.ForeColor = Color.DimGray;

            teraRaidView.textAbilityCapsule.Text = "0";
            teraRaidView.textAbilityCapsule.ForeColor = Color.DimGray;
            teraRaidView.labelAbilityCapsule.ForeColor = Color.DimGray;

            teraRaidView.textBottleCap.Text = "0";
            teraRaidView.textBottleCap.ForeColor = Color.DimGray;
            teraRaidView.labelBottleCap.ForeColor = Color.DimGray;

            teraRaidView.textSweetHerba.Text = "0";
            teraRaidView.textSweetHerba.ForeColor = Color.DimGray;
            teraRaidView.labelSweetHerba.ForeColor = Color.DimGray;

            teraRaidView.textSaltyHerba.Text = "0";
            teraRaidView.textSaltyHerba.ForeColor = Color.DimGray;
            teraRaidView.labelSaltyHerba.ForeColor = Color.DimGray;

            teraRaidView.textBitterHerba.Text = "0";
            teraRaidView.textBitterHerba.ForeColor = Color.DimGray;
            teraRaidView.labelBitterHerba.ForeColor = Color.DimGray;

            teraRaidView.textSourHerba.Text = "0";
            teraRaidView.textSourHerba.ForeColor = Color.DimGray;
            teraRaidView.labelSourHerba.ForeColor = Color.DimGray;

            teraRaidView.textSpicyHerba.Text = "0";
            teraRaidView.textSpicyHerba.ForeColor = Color.DimGray;
            teraRaidView.labelSpicyHerba.ForeColor = Color.DimGray;

            for (int i = 0; i < rewards.Count; i++)
            {
                if (rewards[i].Item1 == 645)
                {
                    teraRaidView.textAbilityCapsule.Text = (
                        int.Parse(teraRaidView.textAbilityCapsule.Text) + 1
                    ).ToString();
                    teraRaidView.textAbilityCapsule.ForeColor = Color.White;
                    teraRaidView.labelAbilityCapsule.ForeColor = Color.WhiteSmoke;
                }
                if (rewards[i].Item1 == 795)
                {
                    teraRaidView.textBottleCap.Text = (
                        int.Parse(teraRaidView.textBottleCap.Text) + 1
                    ).ToString();
                    teraRaidView.textBottleCap.ForeColor = Color.White;
                    teraRaidView.labelBottleCap.ForeColor = Color.WhiteSmoke;
                }
                if (rewards[i].Item1 == 1606)
                {
                    teraRaidView.textAbilityPatch.Text = (
                        int.Parse(teraRaidView.textAbilityPatch.Text) + 1
                    ).ToString();
                    teraRaidView.textAbilityPatch.ForeColor = Color.White;
                    teraRaidView.labelAbilityPatch.ForeColor = Color.WhiteSmoke;
                }
                if (rewards[i].Item1 == 1904)
                {
                    teraRaidView.textSweetHerba.Text = (
                        int.Parse(teraRaidView.textSweetHerba.Text) + 1
                    ).ToString();
                    teraRaidView.textSweetHerba.ForeColor = Color.White;
                    teraRaidView.labelSweetHerba.ForeColor = Color.WhiteSmoke;
                }
                if (rewards[i].Item1 == 1905)
                {
                    teraRaidView.textSaltyHerba.Text = (
                        int.Parse(teraRaidView.textSaltyHerba.Text) + 1
                    ).ToString();
                    teraRaidView.textSaltyHerba.ForeColor = Color.White;
                    teraRaidView.labelSaltyHerba.ForeColor = Color.WhiteSmoke;
                }
                if (rewards[i].Item1 == 1906)
                {
                    teraRaidView.textSourHerba.Text = (
                        int.Parse(teraRaidView.textSourHerba.Text) + 1
                    ).ToString();
                    teraRaidView.textSourHerba.ForeColor = Color.White;
                    teraRaidView.labelSourHerba.ForeColor = Color.WhiteSmoke;
                }
                if (rewards[i].Item1 == 1907)
                {
                    teraRaidView.textBitterHerba.Text = (
                        int.Parse(teraRaidView.textBitterHerba.Text) + 1
                    ).ToString();
                    teraRaidView.textBitterHerba.ForeColor = Color.White;
                    teraRaidView.labelBitterHerba.ForeColor = Color.WhiteSmoke;
                }
                if (rewards[i].Item1 == 1908)
                {
                    teraRaidView.textSpicyHerba.Text = (
                        int.Parse(teraRaidView.textSpicyHerba.Text) + 1
                    ).ToString();
                    teraRaidView.textSpicyHerba.ForeColor = Color.White;
                    teraRaidView.labelSpicyHerba.ForeColor = Color.WhiteSmoke;
                }
            }

            var shiny = raid.CheckIsShiny(encounter);
            teraRaidView.Shiny.Visible = shiny;
            teraRaidView.picShinyAlert.Enabled = shiny;
            return;
        }

        var msg = $"Unable to display raid at index {index}. Ensure there are no cheats running or anything else that might shift RAM (Edizon, overlays, etc.), then reboot your console and try again.";
        Task.Run(
            async () =>
                await this
                    .DisplayMessageBox(Webhook, msg, Source.Token)
                    .ConfigureAwait(false),
            Source.Token
        );
    }

    private string GetPIDString(Raid raid, ITeraRaid? enc)
    {
        if (HideSeed)
            return "Hidden";

        const string shiny_mark = " (☆)";
        var pid = $"{raid.PID:X8}";
        return raid.CheckIsShiny(enc) ? pid + shiny_mark : pid;
    }

    private static string IVsString(ReadOnlySpan<int> ivs, bool verbose = false)
    {
        string s = string.Empty;
        var stats = new[] { "HP", "Atk", "Def", "SpA", "SpD", "Spe" };
        for (int i = 0; i < ivs.Length; i++)
        {
            s += $"{ivs[i]:D2}{(verbose ? " " + stats[i] : string.Empty)}";
            if (i < 5)
                s += "/";
        }
        return s;
    }

    private static Image ApplyTeraColor(byte elementalType, Image img, SpriteBackgroundType type)
    {
        var color = TypeColor.GetTypeSpriteColor(elementalType);
        var thk = SpriteBuilder.ShowTeraThicknessStripe;
        var op = SpriteBuilder.ShowTeraOpacityStripe;
        var bg = SpriteBuilder.ShowTeraOpacityBackground;
        return ApplyColor(img, type, color, thk, op, bg);
    }

    private static Image ApplyColor(Image img, SpriteBackgroundType type, Color color, int thick, byte opacStripe, byte opacBack)
    {
        if (type == SpriteBackgroundType.BottomStripe)
        {
            int stripeHeight = thick; // from bottom
            if ((uint)stripeHeight > img.Height) // clamp negative & too-high values back to height.
                stripeHeight = img.Height;

            return ImageUtil.BlendTransparentTo(img, color, opacStripe, img.Width * 4 * (img.Height - stripeHeight));
        }
        if (type == SpriteBackgroundType.TopStripe)
        {
            int stripeHeight = thick; // from top
            if ((uint)stripeHeight > img.Height) // clamp negative & too-high values back to height.
                stripeHeight = img.Height;

            return ImageUtil.BlendTransparentTo(img, color, opacStripe, 0, (img.Width * 4 * stripeHeight) - 4);
        }
        if (type == SpriteBackgroundType.FullBackground) // full background
            return ImageUtil.BlendTransparentTo(img, color, opacBack);
        return img;
    }

    private static Image? GenerateMap(Raid raid, int teratype)
    {
        var original = PKHeX.Drawing.Misc.TypeSpriteUtil.GetTypeSpriteGem((byte)teratype);
        if (original == null)
            return null;
        var gem = (Image)new Bitmap(original, new Size(30, 30));
        var gem2 = (Image)new Bitmap(original, new Size(30, 30));
        SpriteUtil.GetSpriteGlow(gem, 0xFF, 0xFF, 0xFF, out var glow, true);
        SpriteUtil.GetSpriteGlow(gem, 0xCC, 0xCC, 0xCC, out var glow2, true);
        gem = ImageUtil.LayerImage(gem, ImageUtil.GetBitmap(glow, gem.Width, gem.Height, gem.PixelFormat), 0, 0);
        gem2 = ImageUtil.LayerImage(gem, ImageUtil.GetBitmap(glow2, gem.Width, gem.Height, gem.PixelFormat), 0, 0);

        if (
            DenLocationsBase is null
            || DenLocationsBase.Count == 0
            || DenLocationsKitakami is null
            || DenLocationsKitakami.Count == 0
        )
            return null;

        var locData = raid.MapParent switch
        {
            TeraRaidMapParent.Paldea => DenLocationsBase,
            TeraRaidMapParent.Kitakami => DenLocationsKitakami,
            _ => DenLocationsBlueberry,
        };
        var map = raid.MapParent switch
        {
            TeraRaidMapParent.Paldea => MapBase,
            TeraRaidMapParent.Kitakami => MapKitakami,
            _ => MapBlueberry,
        };
        try
        {
            (double x, double y) = GetCoordinate(raid, locData);
            return ImageUtil.LayerImage(map, gem, (int)x, (int)y);
        }
        catch
        {
            return null;
        }
    }

    private static (double x, double y) GetCoordinate(Raid raid, IReadOnlyDictionary<string, float[]> locData)
    {
        (double a, double b, double c, double d, short e, short f) = raid.MapParent switch
        {
            TeraRaidMapParent.Paldea => (MapMagic.X_MULT_BASE, MapMagic.X_ADD_BASE, MapMagic.Y_MULT_BASE, MapMagic.Y_ADD_BASE, MapMagic.MULT_CONST_BASE, MapMagic.DIV_CONST_BASE),
            TeraRaidMapParent.Kitakami => (MapMagic.X_MULT_KITAKAMI, MapMagic.X_ADD_KITAKAMI, MapMagic.Y_MULT_KITAKAMI, MapMagic.Y_ADD_KITAKMI, MapMagic.MULT_CONST_KITAKAMI, MapMagic.DIV_CONST_KITAKAMI),
            _ => (MapMagic.X_MULT_BLUEBERRY, MapMagic.X_ADD_BLUEBERRY, MapMagic.Y_MULT_BLUEBERRY, MapMagic.Y_ADD_BLUEBERRY, MapMagic.MULT_CONST_BLUEBERRY, MapMagic.DIV_CONST_BLUEBERRY)
        };
        double x = ((a * locData[$"{raid.Area}-{raid.LotteryGroup}-{raid.Den}"][0]) + b) * e / f;
        double y = ((c * locData[$"{raid.Area}-{raid.LotteryGroup}-{raid.Den}"][2]) + d) * e / f;
        return (x, y);
    }

    private bool StopAdvanceDate(IEnumerable<uint> previousSeeds)
    {
        var raids = RaidContainer.Raids;
        var rewards = RaidContainer.Rewards;
        var strings = GameInfo.GetStrings(1);
        var curSeeds = raids.Select(x => x.Seed).ToArray();
        var sameRaids = curSeeds.Except(previousSeeds).ToArray().Length == 0;

        StatDaySkipTries++;
        if (sameRaids)
        {
            StatDaySkipStreak = 0;
            return false;
        }

        StatDaySkipSuccess++;
        StatDaySkipStreak++;
        if (!Config.EnableFilters)
            return true;

        var encounters = RaidContainer.Encounters;

        foreach (RaidFilter rf in RaidFilters)
        {
            var index = Invoke(() => RaidBoost.SelectedIndex);

            if (rf.FilterSatisfied(RaidContainer, encounters, raids, index))
                return true;
        }

        bool alreadySaved = false;

        // Vio thing: FoMO match detection
        for (int i = 0; i < raids.Count; i++)
        {
            var raid = raids[i];
            var encounter = encounters[i];
            var reward = rewards[i];
            var param = encounter.GetParam();

            var timeSpan = stopwatch.Elapsed;
            var timeEmpty = new TimeSpan(0, 0, 0, 0);
            string time = string.Empty;
            if (((int)timeSpan.TotalDays) != timeEmpty.TotalDays) { time = timeSpan.ToString(@"d\d\ %h\h\ mm\m\ ss\s"); }
            else if (((int)timeSpan.TotalHours) != timeEmpty.TotalHours) { time = timeSpan.ToString(@"%h\h\ mm\m\ ss\s"); }
            else if (((int)timeSpan.TotalMinutes) != timeEmpty.TotalMinutes) { time = timeSpan.ToString(@"%m\m\ ss\s"); }
            else { time = timeSpan.ToString(@"%s\s"); }
            var teraType = raids[i].GetTeraType(encounters[i]);
            var color = TypeColor.GetTypeSpriteColor((byte)teraType);
            var hexColor = $"{color.R:X2}{color.G:X2}{color.B:X2}";
            var filter = new RaidFilter { Name = "FoMO" };

            var blank = new PK9
            {
                Species = encounter.Species,
                Form = encounter.Form,
                Gender = encounters[i].Gender,
            };
            blank.SetSuggestedFormArgument();
            Encounter9RNG.GenerateData(blank, param, EncounterCriteria.Unrestricted, raid.Seed);
            var spriteName = GetSpriteNameForUrl(blank, raids[i].CheckIsShiny(encounters[i]));
            var form = Utils.GetFormString(blank.Species, blank.Form, strings);
            var species = $"{strings.Species[encounter.Species]}";
            var shiny = $"{(raid.CheckIsShiny(encounter) ? (ShinyExtensions.IsSquareShinyExist(blank) ? "⛋" : "☆") : "")}";
            if (raids[i].CheckIsShiny(encounters[i]))
            {
                StatShinyCount++;
                Fomo.Add($"{shiny} {species}{form}");
                if (Config.EnableFomoNotification)
                    Task.Run(async () => await FomoWebhook.SendNotification(encounter, raid, filter, time, reward, hexColor, spriteName, Source.Token));
                if (Config.SaveOnFomo && !alreadySaved)
                {
                    Task.WaitAll(Task.Run(async () => await ConnectionWrapper.SaveGame(Config, Source.Token).ConfigureAwait(false)));
                    alreadySaved = true;
                }
            }
        }

        return StopAdvances;
    }

    private async Task ReadRaids(CancellationToken token)
    {
        if (Config is { PaldeaScan: false, KitakamiScan: false, BlueberryScan: false })
        {
            await this.DisplayMessageBox(Webhook, "Please select a location to scan in your General Settings.", token, "No locations selected").ConfigureAwait(false);
            return;
        }

        if (RaidBlockOffsetBase == 0)
        {
            UpdateStatus("Caching the raid block pointers...");
            RaidBlockOffsetBase = await ConnectionWrapper.Connection
                .PointerAll(RaidBlockPointerBase.ToArray(), token)
                .ConfigureAwait(false);
            RaidBlockOffsetKitakami = await ConnectionWrapper.Connection
                .PointerAll(RaidBlockPointerKitakami.ToArray(), token)
                .ConfigureAwait(false);
            RaidBlockOffsetBlueberry = await ConnectionWrapper.Connection
                .PointerAll(RaidBlockPointerBlueberry.ToArray(), token)
                .ConfigureAwait(false);
        }

        RaidContainer.ClearRaids();
        RaidContainer.ClearEncounters();
        RaidContainer.ClearRewards();

        // Base
        var msg = string.Empty;
        int delivery,
            enc;

        if (Config.PaldeaScan)
        {
            UpdateStatus("Reading Paldea raid block...");
            var data = await ConnectionWrapper.Connection
                .ReadBytesAbsoluteAsync(RaidBlockOffsetBase + RaidBlock.HEADER_SIZE, (int)RaidBlock.SIZE_BASE, token)
                .ConfigureAwait(false);

            (delivery, enc) = RaidContainer.ReadAllRaids(data, Config.Progress, Config.EventProgress, GetRaidBoost(), TeraRaidMapParent.Paldea);
            if (enc > 0)
                msg += $"Failed to find encounters for {enc} raid(s).\n";

            if (delivery > 0)
                msg += $"Invalid delivery group ID for {delivery} raid(s). Try deleting the \"cache\" folder.\n";

            if (msg != string.Empty)
            {
                msg += $"\nMore info can be found in the \"raid_dbg_{TeraRaidMapParent.Paldea}.txt\" file.";
                await this.DisplayMessageBox(Webhook, msg, token, "Raid Read Error").ConfigureAwait(false);
            }
        }

        var raids = RaidContainer.Raids;
        var encounters = RaidContainer.Encounters;
        var rewards = RaidContainer.Rewards;
        RaidContainer.ClearRaids();
        RaidContainer.ClearEncounters();
        RaidContainer.ClearRewards();

        // Kitakami
        if (Config.KitakamiScan)
        {
            UpdateStatus("Reading Kitakami raid block...");
            var data = await ConnectionWrapper.Connection
                .ReadBytesAbsoluteAsync(RaidBlockOffsetKitakami, (int)RaidBlock.SIZE_KITAKAMI, token)
                .ConfigureAwait(false);

            msg = string.Empty;
            (delivery, enc) = RaidContainer.ReadAllRaids(data, Config.Progress, Config.EventProgress, GetRaidBoost(), TeraRaidMapParent.Kitakami);
            if (enc > 0)
                msg += $"Failed to find encounters for {enc} raid(s).\n";

            if (delivery > 0)
                msg += $"Invalid delivery group ID for {delivery} raid(s). Try deleting the \"cache\" folder.\n";

            if (msg != string.Empty)
            {
                msg += $"\nMore info can be found in the \"raid_dbg_{TeraRaidMapParent.Kitakami}.txt\" file.";
                await this.DisplayMessageBox(Webhook, msg, token, "Raid Read Error")
                    .ConfigureAwait(false);
            }
        }

        var allRaids = raids.Concat(RaidContainer.Raids).ToList().AsReadOnly();
        var allEncounters = encounters.Concat(RaidContainer.Encounters).ToList().AsReadOnly();
        var allRewards = rewards.Concat(RaidContainer.Rewards).ToList().AsReadOnly();
        RaidContainer.ClearRaids();
        RaidContainer.ClearEncounters();
        RaidContainer.ClearRewards();

        // Blueberry
        if (Config.BlueberryScan)
        {
            UpdateStatus("Reading Blueberry raid block...");
            var data = await ConnectionWrapper.Connection
                .ReadBytesAbsoluteAsync(RaidBlockOffsetBlueberry, (int)RaidBlock.SIZE_BLUEBERRY, token)
                .ConfigureAwait(false);

            msg = string.Empty;
            (delivery, enc) = RaidContainer.ReadAllRaids(data, Config.Progress, Config.EventProgress, GetRaidBoost(), TeraRaidMapParent.Blueberry);
            if (enc > 0)
                msg += $"Failed to find encounters for {enc} raid(s).\n";

            if (delivery > 0)
                msg += $"Invalid delivery group ID for {delivery} raid(s). Try deleting the \"cache\" folder.\n";

            if (msg != string.Empty)
            {
                msg += $"\nMore info can be found in the \"raid_dbg_{TeraRaidMapParent.Blueberry}.txt\" file.";
                await this.DisplayMessageBox(Webhook, msg, token, "Raid Read Error")
                    .ConfigureAwait(false);
            }
        }

        allRaids = allRaids.Concat(RaidContainer.Raids).ToList().AsReadOnly();
        allEncounters = allEncounters.Concat(RaidContainer.Encounters).ToList().AsReadOnly();
        allRewards = allRewards.Concat(RaidContainer.Rewards).ToList().AsReadOnly();

        RaidContainer.SetRaids(allRaids);
        RaidContainer.SetEncounters(allEncounters);
        RaidContainer.SetRewards(allRewards);

        UpdateStatus("Completed!");

        var filterMatchCount = Enumerable.Range(0, allRaids.Count)
            .Count(c => RaidFilters.Any(z => z.FilterSatisfied(RaidContainer, allEncounters[c], allRaids[c], GetRaidBoost())));
        var shinyCount = Enumerable.Range(0, allRaids.Count).Where(i => allRaids[i].CheckIsShiny(allEncounters[i])).Count();

        if (InvokeRequired)
            Invoke(() => { LabelLoadedRaids.Text = $"Met Filters/Shiny: {filterMatchCount}/{shinyCount}"; });
        else
            LabelLoadedRaids.Text = $"Met Filters/Shiny: {filterMatchCount}/{shinyCount}";

        if (allRaids.Count > 0)
        {
            ButtonEnable(true, ButtonPrevious, ButtonNext);
            var dataSource = GetComboList();
            if (InvokeRequired)
                Invoke(() => ComboIndex.DataSource = dataSource);
            else
                ComboIndex.DataSource = dataSource;

            if (InvokeRequired)
                Invoke(() => ComboIndex.SelectedIndex = ComboIndex.SelectedIndex < allRaids.Count ? ComboIndex.SelectedIndex : 0);
            else
                ComboIndex.SelectedIndex = ComboIndex.SelectedIndex < allRaids.Count ? ComboIndex.SelectedIndex : 0;
        }
        else
        {
            ButtonEnable(false, ButtonPrevious, ButtonNext);
            if (allRaids.Count > RaidBlock.MAX_COUNT_BASE + RaidBlock.MAX_COUNT_KITAKAMI || allRaids.Count == 0)
            {
                msg = "Bad read, ensure there are no cheats running or anything else that might shift RAM (Edizon, overlays, etc.), then reboot your console and try again.";
                await this.DisplayMessageBox(Webhook, msg, token, "Raid Read Error").ConfigureAwait(false);
            }
        }
    }

    private List<string> GetComboList()
    {
        var nameList = new List<String>();
        var raids = RaidContainer.Raids;
        var encounters = RaidContainer.Encounters;
        var strings = GameInfo.GetStrings(1);

        for (var i = 0; raids.Count > i; i++)
        {
            var raid = raids[i];
            var encounter = encounters[i];
            var param = encounter.GetParam();
            var blank = new PK9
            {
                Species = encounter.Species,
                Form = encounter.Form
            };
            Encounter9RNG.GenerateData(blank, param, EncounterCriteria.Unrestricted, raid.Seed);
            var form = Utils.GetFormString(blank.Species, blank.Form, strings);
            var species = $"{strings.Species[encounter.Species]}";
            var shiny = $"{(raid.CheckIsShiny(encounter) ? (ShinyExtensions.IsSquareShinyExist(blank) ? "⛋" : "☆") : "")}";

            nameList.Add($"{shiny}{i + 1:D2} {species}{form}{shiny}");
        }

        return nameList;
    }

    public void Game_SelectedIndexChanged(string name)
    {
        Config.Game = name;
        RaidContainer.SetGame(name);
        if (RaidContainer.Raids.Count > 0)
            DisplayRaid();
    }

    public void Protocol_SelectedIndexChanged(SwitchProtocol protocol)
    {
        Config.Protocol = protocol;
        ConnectionConfig.Protocol = protocol;
        if (protocol is SwitchProtocol.USB)
        {
            InputSwitchIP.Visible = false;
            LabelSwitchIP.Visible = false;
            USB_Port_label.Visible = true;
            USB_Port_TB.Visible = true;
            ConnectionConfig.Port = Config.UsbPort;
        }
        else
        {
            InputSwitchIP.Visible = true;
            LabelSwitchIP.Visible = true;
            USB_Port_label.Visible = false;
            USB_Port_TB.Visible = false;
            ConnectionConfig.Port = 6000;
        }
    }

    private void DisplayMap(object sender, EventArgs e)
    {
        var raids = RaidContainer.Raids;
        if (raids.Count == 0)
        {
            Task.Run(async () => await this.DisplayMessageBox(Webhook, "Raids not loaded.", Source.Token).ConfigureAwait(false), Source.Token);
            return;
        }

        var raid = raids[ComboIndex.SelectedIndex];
        var encounter = RaidContainer.Encounters[ComboIndex.SelectedIndex];
        var teraType = raid.GetTeraType(encounter);
        var map = GenerateMap(raid, teraType);
        if (map is null)
        {
            Task.Run(async () => await this.DisplayMessageBox(Webhook, "Error generating map.", Source.Token).ConfigureAwait(false), Source.Token);
            return;
        }

        var form = new MapView(map);
        ShowDialog(form);
    }

    private void Rewards_Click(object sender, EventArgs e)
    {
        if (RaidContainer.Raids.Count == 0)
        {
            Task.Run(async () => await this.DisplayMessageBox(Webhook, "Raids not loaded.", Source.Token).ConfigureAwait(false), Source.Token);
            return;
        }

        var rewards = RaidContainer.Rewards[ComboIndex.SelectedIndex];
        if (rewards is null)
        {
            Task.Run(async () => await this.DisplayMessageBox(Webhook, "Error while displaying rewards.", Source.Token).ConfigureAwait(false), Source.Token);
            return;
        }

        var form = new RewardsView(RaidContainer.Strings.Item, RaidContainer.Strings.Move, rewards);
        ShowDialog(form);
    }

    private void RaidBoost_SelectedIndexChanged(object sender, EventArgs e)
    {
        RaidContainer.ClearRewards();
        var raids = RaidContainer.Raids;
        var encounters = RaidContainer.Encounters;

        List<List<(int, int, int)>> newRewards = [];
        for (int i = 0; i < raids.Count; i++)
        {
            var raid = raids[i];
            var encounter = encounters[i];
            newRewards.Add(encounter.GetRewards(RaidContainer, raid, RaidBoost.SelectedIndex));
        }
        RaidContainer.SetRewards(newRewards);
    }

    private void Move_Clicked(object sender, EventArgs e)
    {
        if (RaidContainer.Raids.Count == 0)
        {
            Task.Run(async () => await this.DisplayMessageBox(Webhook, "Raids not loaded.", Source.Token).ConfigureAwait(false), Source.Token);
            return;
        }

        var encounter = RaidContainer.Encounters[ComboIndex.SelectedIndex];
        if (encounter is null)
            return;

        ShowExtraMoves ^= true;
        LabelMoves.Text = ShowExtraMoves ? "Extra:" : "Moves:";
        LabelMoves.Location = LabelMoves.Location with { X = LabelMoves.Location.X + (ShowExtraMoves ? 9 : -9) };

        var length = encounter.ExtraMoves.Length < 4 ? 4 : encounter.ExtraMoves.Length;
        var extraMoves = new ushort[length];
        for (int i = 0; i < encounter.ExtraMoves.Length; i++)
            extraMoves[i] = encounter.ExtraMoves[i];

        Move1.Text = ShowExtraMoves
            ? RaidContainer.Strings.Move[extraMoves[0]]
            : RaidContainer.Strings.Move[encounter.Move1];
        Move2.Text = ShowExtraMoves
            ? RaidContainer.Strings.Move[extraMoves[1]]
            : RaidContainer.Strings.Move[encounter.Move2];
        Move3.Text = ShowExtraMoves
            ? RaidContainer.Strings.Move[extraMoves[2]]
            : RaidContainer.Strings.Move[encounter.Move3];
        Move4.Text = ShowExtraMoves
            ? RaidContainer.Strings.Move[extraMoves[3]]
            : RaidContainer.Strings.Move[encounter.Move4];
    }

    private void ComboIndex_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RaidContainer.Raids.Count == 0)
            return;

        DisplayRaid();
        if (Config.StreamerView)
            DisplayPrettyRaid();
    }

    private void SendScreenshot_Click(object sender, EventArgs e)
    {
        Task.Run(async () =>
            {
                try
                {
                    await Webhook.SendScreenshot(ConnectionWrapper.Connection, Source.Token).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await this.DisplayMessageBox(Webhook, $"Could not send the screenshot: {ex.Message}", Source.Token).ConfigureAwait(false);
                }
            },
            Source.Token
        );
    }

    private void SearchTimer_Elapsed(object sender, EventArgs e)
    {
        if (!stopwatch.IsRunning)
            return;

        var timeSpan = stopwatch.Elapsed;
        var timeEmpty = new TimeSpan(0, 0, 0, 0);
        string time = string.Empty;
        if (((int)timeSpan.TotalDays) != timeEmpty.TotalDays) { time = timeSpan.ToString(@"d\d\ %h\h\ mm\m\ ss\s"); }
        else if (((int)timeSpan.TotalHours) != timeEmpty.TotalHours) { time = timeSpan.ToString(@"%h\h\ mm\m\ ss\s"); }
        else if (((int)timeSpan.TotalMinutes) != timeEmpty.TotalMinutes) { time = timeSpan.ToString(@"%m\m\ ss\s"); }
        else { time = timeSpan.ToString(@"%s\s"); }

        if (Config.SearchTimeInTitle)
            Invoke(() => Text = formTitle + " [Searching for " + time + "]");
        SearchTime.Text = "Searching for: " + time;
        if (Config.StreamerView && teraRaidView is not null)
            Invoke(() => teraRaidView.textSearchTime.Text = time);
    }

    public void TestWebhook() => Task.Run(async () => await TestWebhookAsync(Source.Token).ConfigureAwait(false), Source.Token);

    private async Task TestWebhookAsync(CancellationToken token)
    {
        var filter = new RaidFilter { Name = "Test Webhook" };

        int i = -1;
        if (InvokeRequired)
            i = Invoke(() =>
            {
                return ComboIndex.SelectedIndex;
            });
        else
            i = ComboIndex.SelectedIndex;

        var raids = RaidContainer.Raids;
        var encounters = RaidContainer.Encounters;
        var rewards = RaidContainer.Rewards;
        if (i > -1 && encounters[i] is not null && raids[i] is not null)
        {
            var timeSpan = stopwatch.Elapsed;
            var timeEmpty = new TimeSpan(0, 0, 0, 0);
            string time = string.Empty;
            if (((int)timeSpan.TotalDays) != timeEmpty.TotalDays) { time = timeSpan.ToString(@"d\d\ %h\h\ mm\m\ ss\s"); }
            else if (((int)timeSpan.TotalHours) != timeEmpty.TotalHours) { time = timeSpan.ToString(@"%h\h\ mm\m\ ss\s"); }
            else if (((int)timeSpan.TotalMinutes) != timeEmpty.TotalMinutes) { time = timeSpan.ToString(@"%m\m\ ss\s"); }
            else { time = timeSpan.ToString(@"%s\s"); }
            var teraType = raids[i].GetTeraType(encounters[i]);
            var color = TypeColor.GetTypeSpriteColor((byte)teraType);
            var hexColor = $"{color.R:X2}{color.G:X2}{color.B:X2}";

            var blank = new PK9
            {
                Species = encounters[i].Species,
                Form = encounters[i].Form,
                Gender = encounters[i].Gender,
            };
            blank.SetSuggestedFormArgument();

            var spriteName = GetSpriteNameForUrl(blank, raids[i].CheckIsShiny(encounters[i]));
            await Webhook
                .SendNotification(
                    encounters[i],
                    raids[i],
                    filter,
                    time,
                    rewards[i],
                    hexColor,
                    spriteName,
                    token
                )
                .ConfigureAwait(false);
            return;
        }

        await ErrorHandler
            .DisplayMessageBox(
                this,
                Webhook,
                "Please connect to your device and ensure a raid has been found.",
                token
            )
            .ConfigureAwait(false);
    }

    public void ToggleStreamerView()
    {
        if (Config.StreamerView)
        {
            teraRaidView = new();
            teraRaidView.Map.Image = MapBase;
            teraRaidView.Show();
        }
    }

    private static string GetSpriteNameForUrl(PK9 pk, bool shiny)
    {
        // Since we're later using this for URL assembly later, we need dashes instead of underscores for forms.
        var spriteName = SpriteName.GetResourceStringSprite(pk.Species, pk.Form, pk.Gender, pk.FormArgument, EntityContext.Gen9, shiny)[1..];
        return spriteName.Replace('_', '-').Insert(0, "_");
    }

    private void CopyAnnounce_Click(object sender, EventArgs e)
    {

        var filter = new RaidFilter { Name = "Test Webhook" };

        int i = -1;
        if (InvokeRequired)
            i = Invoke(() => { return ComboIndex.SelectedIndex; });
        else i = ComboIndex.SelectedIndex;

        var raids = RaidContainer.Raids;
        var encounters = RaidContainer.Encounters;
        var rewards = RaidContainer.Rewards;
        if (i > -1 && encounters[i] is not null && raids[i] is not null)
        {
            var timeSpan = stopwatch.Elapsed;
            var timeEmpty = new TimeSpan(0, 0, 0, 0);
            string time = string.Empty;
            if (((int)timeSpan.TotalDays) != timeEmpty.TotalDays) { time = timeSpan.ToString(@"d\d\ %h\h\ mm\m\ ss\s"); }
            else if (((int)timeSpan.TotalHours) != timeEmpty.TotalHours) { time = timeSpan.ToString(@"%h\h\ mm\m\ ss\s"); }
            else if (((int)timeSpan.TotalMinutes) != timeEmpty.TotalMinutes) { time = timeSpan.ToString(@"%m\m\ ss\s"); }
            else { time = timeSpan.ToString(@"%s\s"); }
            var teraType = raids[i].GetTeraType(encounters[i]);
            var color = TypeColor.GetTypeSpriteColor((byte)teraType);
            var hexColor = $"{color.R:X2}{color.G:X2}{color.B:X2}";

            var blank = new PK9
            {
                Species = encounters[i].Species,
                Form = encounters[i].Form,
                Gender = encounters[i].Gender,
            };
            blank.SetSuggestedFormArgument();

            var spriteName = GetSpriteNameForUrl(blank, raids[i].CheckIsShiny(encounters[i]));

            if (ModifierKeys == Keys.Control)
                Clipboard.SetText(Webhook.GetAnnouncement(encounters[i], raids[i], filter, time, rewards[i], hexColor, spriteName, "technicalcopy"));
            else if (ModifierKeys == (Keys.Shift | Keys.Control))
                Task.Run(async () => await Webhook.SendNotification(encounters[i], raids[i], filter, time, rewards[i], hexColor, spriteName, Source.Token));
            else if (ModifierKeys == (Keys.Shift | Keys.Control | Keys.Alt))
                Task.Run(async () => await FomoWebhook.SendNotification(encounters[i], raids[i], filter, time, rewards[i], hexColor, spriteName, Source.Token));
            else
                Clipboard.SetText(Webhook.GetAnnouncement(encounters[i], raids[i], filter, time, rewards[i], hexColor, spriteName, "copy"));
            return;
        }
    }

    private void Protocol_dropdown_SelectedIndexChanged(object sender, EventArgs e)
    {
        Config.Protocol = (SysBot.Base.SwitchProtocol)Protocol_dropdown.SelectedIndex;
        Protocol_SelectedIndexChanged(Config.Protocol);
        WriteConfig();
    }

    private async void ButtonScreenState_Click(object sender, EventArgs e)
    {
        ButtonScreenState.Text = $"{(ConnectionWrapper.CurrentScreenState ? "Screen Off" : "Screen On")}";
        await ConnectionWrapper.ScreenToggle(Source.Token).ConfigureAwait(false);
    }

    public void WriteConfig()
    {
        JsonSerializerOptions options = new() { WriteIndented = true };
        string output = JsonSerializer.Serialize(Config, options);
        using StreamWriter sw = new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json"));
        sw.Write(output);
    }

    private void LabelShinyCount_Click(object sender, EventArgs e)
    {
        FomoCount++;
        //LabelShinyCount.Text = LabelShinyCount.Text + "" + FomoCount.ToString();
        if (FomoCount == 7)
        {
            FomoCount = 0;
            Config.SaveOnFomo = !Config.SaveOnFomo;
            LabelShinyCount.Text = $"FoMO Saves: {(Config.SaveOnFomo ? "On" : "Off")}";

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer()
            {
                Interval = 800,
                Enabled = true
            };

            timer.Tick += (sender, e) =>
            {
                LabelShinyCount.Text = $"Shinies Missed: {GetStatShinyCount()}";
                WriteConfig();
                timer.Dispose();
            };
        }
    }

    private void B_ResetTime_Click(object sender, EventArgs e)
    {
        Task.Run(async () =>
        {
            try
            {
                UpdateStatus("Resetting date");
                await ConnectionWrapper.ResetTime(Source.Token).ConfigureAwait(false);
                UpdateStatus("Date reset");
            }
            catch (Exception ex)
            {
                await this.DisplayMessageBox(Webhook, $"Could not reset the date: {ex.Message}", Source.Token).ConfigureAwait(false);
            }
        });
    }
}
