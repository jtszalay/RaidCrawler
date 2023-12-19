using PKHeX.Core;
using RaidCrawler.Core.Interfaces;
using RaidCrawler.Core.Structures;
using SysBot.Base;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace RaidCrawler.Core.Discord;

public class FomoNotificationHandler(IWebhookConfig config) : NotificationHandler(config)
{
    private readonly string[]? FomoDiscordWebhooks = config.EnableNotification ? config.DiscordWebhook.Split(',') : null;

    public override async Task SendNotification(ITeraRaid encounter, Raid raid, RaidFilter filter, string time, IReadOnlyList<(int, int, int)> RewardsList,
        string hexColor, string spriteName, CancellationToken token
    )
    {
        if (FomoDiscordWebhooks is null || !config.EnableNotification)
            return;

        var webhook = GenerateWebhook(encounter, raid, filter, time, RewardsList, hexColor, spriteName);
        var content = new StringContent(JsonSerializer.Serialize(webhook), Encoding.UTF8, "application/json");
        foreach (var url in FomoDiscordWebhooks)
            await _client.PostAsync(url.Trim(), content, token).ConfigureAwait(false);
    }

    public override async Task SendErrorNotification(string error, string caption, CancellationToken token)
    {
        await SendErrorNotificationHelper(error, caption, token, string.Empty);
    }
}
