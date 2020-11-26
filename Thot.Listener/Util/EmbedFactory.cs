using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using Thot.Listener;

public static class EmbedFactory
{
    public static Embed BuildLeaderboardEmbed(List<UserWordCount> words, int currentPage, SocketGuildUser user = null, IReadOnlyCollection<SocketGuildUser> users = null)
    {
        var builder = new EmbedBuilder();
        builder.WithTitle("Leaderboard");

        if (user is null)
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
            words.OrderByDescending(x => x.Count).Where(x => x.AuthorId == user.Id).ToList().ForEach(word =>
            {
                builder.AddField($"{word.Word}", word.Count, true);
            });
            builder.WithDescription($"And how many times I've seen {user.Username} say each");
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