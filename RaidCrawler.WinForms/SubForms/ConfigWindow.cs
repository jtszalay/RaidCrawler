using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace RaidCrawler.WinForms.SubForms;

public partial class ConfigWindow : Form
{
    private readonly ClientConfig c;

    public ConfigWindow(ClientConfig c)
    {
        this.c = c;
        var assembly = System.Reflection.Assembly.GetEntryAssembly();
        var v = assembly?.GetName().Version!;
        var gitVersionInformationType = assembly?.GetType("GitVersionInformation");
        var shaField = gitVersionInformationType?.GetField("ShortSha");

        InitializeComponent();

        InstanceName.Text = c.InstanceName;
        StoryProgress.SelectedIndex = c.Progress;
        EventProgress.SelectedIndex = c.EventProgress;
        Game.SelectedIndex = Game.FindString(c.Game);
        Protocol_dropdown.SelectedIndex = (int)c.Protocol;

        PlayTone.Checked = c.PlaySound;
        FocusWindow.Checked = c.FocusWindow;
        EnableAlert.Checked = c.EnableAlertWindow;
        AlertMessage.Text = c.AlertWindowMessage;
        AlertMessage.Enabled = EnableAlert.Checked;
        EnableDiscordNotifications.Checked = c.EnableNotification;
        DiscordWebhook.Text = c.DiscordWebhook;
        DiscordWebhook.Enabled = EnableDiscordNotifications.Checked;
        DiscordMessageContent.Text = c.DiscordMessageContent;
        DiscordMessageContent.Enabled = EnableDiscordNotifications.Checked;
        EnableFomoNotifications.Checked = c.EnableFomoNotification;
        FomoWebhook.Text = c.DiscordFomoWebhook;
        FomoWebhook.Enabled = EnableFomoNotifications.Checked;

        ZyroMethod.Checked = c.ZyroMethod;

        UseTouch.Checked = c.UseTouch;
        UseOvershoot.Checked = c.UseOvershoot;
        SaveGame.Checked = c.SaveOnMatch;
        DodgeSystemUpdate.Checked = c.DodgeSystemUpdate;
        UseSetStick.Checked = c.UseSetStick;
        UseMapTrick.Checked = c.UseMapTrick;

        OpenHome.Value = c.OpenHomeDelay;
        NavigateToSettings.Value = c.NavigateToSettingsDelay;
        OpenSettings.Value = c.OpenSettingsDelay;
        Hold.Value = c.HoldDuration;
        SystemDDownPresses.Value = c.SystemDownPresses;
        SystemOvershoot.Value = c.SystemOvershoot;
        Submenu.Value = c.Submenu;
        DateChange.Value = c.DateChange;
        DaysToSkip.Value = c.DaysToSkip;
        ReturnHome.Value = c.ReturnHomeDelay;
        ReturnGame.Value = c.ReturnGameDelay;
        BaseDelay.Value = c.BaseDelay;
        SaveGameDelay.Value = c.SaveGameDelay;
        SystemReset.Value = c.SystemReset;
        RelaunchDelay.Value = c.RelaunchDelay;
        ExtraOverworldWait.Value = c.ExtraOverworldWait;
        PaldeaScanCheck.Checked = c.PaldeaScan;
        KitakamiScanCheck.Checked = c.KitakamiScan;
        BlueberryScanCheck.Checked = c.BlueberryScan;

        SystemDDownPresses.Enabled = !UseOvershoot.Checked;
        SystemOvershoot.Enabled = UseOvershoot.Checked;
        SaveGameDelay.Enabled = SaveGame.Checked;

        IVstyle.SelectedIndex = c.IVsStyle;
        IVspacer.Text = c.IVsSpacer;
        IVverbose.Checked = c.VerboseIVs;

        denToggle.Checked = c.ToggleDen;

        EnableEmoji.Checked = c.EnableEmoji;
        CopyEmoji.Checked = c.CopyEmoji;

        ExperimentalView.Checked = c.StreamerView;
        MapBackground.Checked = c.MapBackground;
        TitleSearchTime.Checked = c.SearchTimeInTitle;

        labelAppVersion.Text =
            "v" + v.Major + "." + v.Minor + "." + v.Build + "-" + shaField?.GetValue(null);
        labelAppVersion.Left = (tabAbout.Width - labelAppVersion.Width) / 2;
        labelAppName.Left = ((tabAbout.Width - labelAppName.Width) / 2) /*+ (picAppIcon.Width / 2) + 2*/;
        //picAppIcon.Left = labelAppName.Left - picAppIcon.Width - 2;
        linkLabel1.Left = (tabAbout.Width - linkLabel1.Width) / 2;
        linkLabel2.Left = (tabAbout.Width - linkLabel2.Width) / 2;
        label1.Left = (tabAbout.Width - label1.Width) / 2;
        label2.Left = (tabAbout.Width - label2.Width) / 2;

        labelWebhooks.Text =
            "Webhooks are " + (DiscordWebhook.Enabled ? "enabled." : "disabled.");
    }

    private void EnableAlert_CheckedChanged(object sender, EventArgs e)
    {
        AlertMessage.Enabled = EnableAlert.Checked;
    }

    private void EnableDiscordNotifications_Click(object sender, EventArgs e)
    {
        DiscordWebhook.Enabled = EnableDiscordNotifications.Checked;
        DiscordMessageContent.Enabled = EnableDiscordNotifications.Checked;
        labelWebhooks.Text =
            "Webhooks are " + (DiscordWebhook.Enabled ? "enabled." : "disabled.");
    }

    private void Config_Closing(object sender, EventArgs e)
    {
        c.InstanceName = InstanceName.Text;

        c.PlaySound = PlayTone.Checked;
        c.FocusWindow = FocusWindow.Checked;
        c.EnableAlertWindow = EnableAlert.Checked;
        c.AlertWindowMessage = AlertMessage.Text;
        c.EnableNotification = EnableDiscordNotifications.Checked;
        c.DiscordWebhook = DiscordWebhook.Text;
        c.DiscordMessageContent = DiscordMessageContent.Text;
        c.EnableFomoNotification = EnableFomoNotifications.Checked;
        c.DiscordFomoWebhook = FomoWebhook.Text;

        c.ZyroMethod = ZyroMethod.Checked;

        c.UseTouch = UseTouch.Checked;
        c.UseOvershoot = UseOvershoot.Checked;
        c.SaveOnMatch = SaveGame.Checked;
        c.DodgeSystemUpdate = DodgeSystemUpdate.Checked;
        c.UseSetStick = UseSetStick.Checked;
        c.UseMapTrick = UseMapTrick.Checked;

        c.OpenHomeDelay = (int)OpenHome.Value;
        c.NavigateToSettingsDelay = (int)NavigateToSettings.Value;
        c.OpenSettingsDelay = (int)OpenSettings.Value;
        c.HoldDuration = (int)Hold.Value;
        c.SystemDownPresses = (int)SystemDDownPresses.Value;
        c.SystemOvershoot = (int)SystemOvershoot.Value;
        c.SystemReset = (int)SystemReset.Value;
        c.Submenu = (int)Submenu.Value;
        c.DateChange = (int)DateChange.Value;
        c.DaysToSkip = (int)DaysToSkip.Value;
        c.ReturnHomeDelay = (int)ReturnHome.Value;
        c.ReturnGameDelay = (int)ReturnGame.Value;
        c.BaseDelay = (int)BaseDelay.Value;
        c.SaveGameDelay = (int)SaveGameDelay.Value;
        c.RelaunchDelay = (int)RelaunchDelay.Value;
        c.ExtraOverworldWait = (int)ExtraOverworldWait.Value;
        c.PaldeaScan = PaldeaScanCheck.Checked;
        c.KitakamiScan = KitakamiScanCheck.Checked;
        c.BlueberryScan = BlueberryScanCheck.Checked;

        c.IVsStyle = IVstyle.SelectedIndex;
        c.IVsSpacer = IVspacer.Text;
        c.VerboseIVs = IVverbose.Checked;

        c.EnableEmoji = EnableEmoji.Checked;
        c.CopyEmoji = CopyEmoji.Checked;

        c.ToggleDen = denToggle.Checked;
        c.StreamerView = ExperimentalView.Checked;
        c.MapBackground = MapBackground.Checked;
        c.SearchTimeInTitle = TitleSearchTime.Checked;

        c.Protocol = (SysBot.Base.SwitchProtocol)Protocol_dropdown.SelectedIndex;

        //Update mainForm title and webhook on save
        if (!TitleSearchTime.Checked || InstanceName.Modified)
        {
            c.SearchTimeInTitle = false;
            var mainForm = Application.OpenForms.OfType<MainWindow>().Single();
            var v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version!;
            var build = string.Empty;
#if DEBUG
            var date = File.GetLastWriteTime(AppContext.BaseDirectory);
            build = $" (dev-{date:yyyyMMdd})";
#endif                
            mainForm.formTitle = "RaidCrawlerV v" + v.Major + "." + v.Minor + "." + v.Build + build + " " + c.InstanceName;
            Invoke(() => mainForm.Text = mainForm.formTitle);
        }
        if (DiscordWebhook.Modified)
        {
            DiscordWebhook.Enabled = EnableDiscordNotifications.Checked;
            labelWebhooks.Text = "Webhooks are " + (DiscordWebhook.Enabled ? "enabled." : "disabled.");
            var mainForm = Application.OpenForms.OfType<MainWindow>().Single();
            mainForm.UpdateWebhook(c);
        }
        if (FomoWebhook.Modified)
        {
            FomoWebhook.Enabled = EnableFomoNotifications.Checked;
            var mainForm = Application.OpenForms.OfType<MainWindow>().Single();
            mainForm.UpdateWebhook(c);
        }
        //end update stuff

        string output = JsonSerializer.Serialize(c, Options);
        using StreamWriter sw =
            new(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json"));
        sw.Write(output);
    }

    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    private void UseOvershoot_CheckedChanged(object sender, EventArgs e)
    {
        SystemDDownPresses.Enabled = !UseOvershoot.Checked;
        SystemOvershoot.Enabled = UseOvershoot.Checked;
    }

    private void UseSetStick_CheckedChanged(object sender, EventArgs e)
    {
        UseSetStick.Enabled = !UseSetStick.Checked;
    }

    private void SaveGame_CheckedChanged(object sender, EventArgs e)
    {
        SaveGameDelay.Enabled = SaveGame.Checked;
    }

    private void BtnTestWebHook_Click(object sender, EventArgs e)
    {
        c.InstanceName = InstanceName.Text;
        c.DiscordMessageContent = DiscordMessageContent.Text;
        c.IVsStyle = IVstyle.SelectedIndex;
        c.VerboseIVs = IVverbose.Checked;
        c.EnableEmoji = EnableEmoji.Checked;
        c.ToggleDen = denToggle.Checked;

        var mainForm = Application.OpenForms.OfType<MainWindow>().Single();
        mainForm.TestWebhook();
    }

    private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        System.Diagnostics.Process.Start(
            new System.Diagnostics.ProcessStartInfo(((LinkLabel)sender).Text)
            {
                UseShellExecute = true,
            }
        );
    }

    private void Game_SelectedIndexChanged(object sender, EventArgs e)
    {
        var game = (string)Game.SelectedItem!;
        var mainForm = Application.OpenForms.OfType<MainWindow>().Single();
        mainForm.Game_SelectedIndexChanged(game);
    }

    private void EmojiConfig_Click(object sender, EventArgs e)
    {
        EmojiConfig config = new(c);
        if (config.ShowDialog() == DialogResult.OK)
            config.Show();
    }

    private void Protocol_Changed(object sender, EventArgs e)
    {
        c.Protocol = (SysBot.Base.SwitchProtocol)Protocol_dropdown.SelectedIndex;
        var mainForm = Application.OpenForms.OfType<MainWindow>().Single();
        mainForm.Protocol_SelectedIndexChanged(c.Protocol);
    }

    private void StreamerView_Clicked(object sender, EventArgs e)
    {
        c.StreamerView = ExperimentalView.Checked;
        var mainForm = Application.OpenForms.OfType<MainWindow>().Single();
        mainForm.ToggleStreamerView();
    }

    private void Save_Click(object sender, EventArgs e)
    {
        Config_Closing(sender, e);

        Save.Text = "Saved";

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer()
        {
            Interval = 500,
            Enabled = true
        };

        timer.Tick += (sender, e) =>
        {
            Save.Text = "Save";

            timer.Dispose();
        };
    }

    private void EnableDiscordNotifications_CheckedChanged(object sender, EventArgs e)
    {
        DiscordWebhook.Enabled = EnableDiscordNotifications.Checked;
    }

    private void EnableFomoNotifications_CheckedChanged(object sender, EventArgs e)
    {
        FomoWebhook.Enabled = EnableFomoNotifications.Checked;
    }
}
