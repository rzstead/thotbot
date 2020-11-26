using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using Thot.Listener;

public static class EmbedFactory
{
    public static Embed BuildLeaderboardEmbed(List<UserWordCount> words, int currentPage, string userMention = null, IReadOnlyCollection<SocketGuildUser> users = null)
    {
        var builder = new EmbedBuilder();
        builder.WithTitle("Leaderboard");

        if (userMention is null)
        {
            builder.WithDescription($"And how many times I've seen you all say each");
            foreach (var word in words)
            {
                var wordUser = users.First(x => x.Id == word.AuthorId).Username;
                builder.AddField(wordUser, $"{word.Word} - {word.Count}", true);
            }
        }
        else
        {
            words.OrderByDescending(x => x.Count).ToList().ForEach(word =>
            {
                builder.AddField($"{word.Word}", word.Count, true);
            });
            builder.WithDescription($"And how many times I've seen {userMention} say each");
        }

        builder.WithFooter($"Page {currentPage + 1}");

        var embed = builder.Build();
        return embed;
    }

    public static Embed BuildWordListEmbed(List<WordCount> words, int currentPage)
    {
        var builder = new EmbedBuilder();
        builder.WithTitle("Tracked Words");
        builder.WithDescription("And how many times I've seen each");
        builder.WithFooter($"Page {currentPage + 1}");

        words.OrderByDescending(x => x.Count).ToList().ForEach(word =>
        {
            builder.AddField(word.Word, word.Count, true);
        });

        return builder.Build();
    }
}