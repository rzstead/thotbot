using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Thot.Listener.Repository;

namespace Thot.Listener.Commands
{
    [Group("words")]
    public class WordsModule : ModuleBase<SocketCommandContext>
    {
        private WordService _wordService = new WordService();
        private const int UPPER_WORD_OPERATION_LIMIT = 10;

        [Command("add")]
        [Summary("Add some words for me to track")]
        public async Task AddAsync([Summary("The word(s) to start tracking")] params string[] words)
        {
            if (words.Any())
            {
                if (words.Count() <= UPPER_WORD_OPERATION_LIMIT)
                {
                    var response = "";
                    try
                    {
                        response = await _wordService.Add(Context.Guild.Id, words.Select(x => x.ToLower()).ToList());
                    }
                    catch (Exception e)
                    {
                        System.Console.Error.Write(e);
                        await ErrorMessage();
                    }

                    if (string.IsNullOrEmpty(response))
                    {
                        await ReplyAsync("Added.");
                    }
                    else
                    {
                        await ReplyAsync(response);
                    }
                }
                else
                {
                    await ReplyAsync("Please keep your requests to sets of 10!");
                }
            }
            else
            {
                await ReplyAsync("Nothing to add.");
            }
        }

        [Command("remove")]
        [Summary("Remove some words from my track list")]
        public async Task RemoveAsync([Summary("The word(s) to remove from my list")] params string[] words)
        {
            if (words.Any())
            {
                string response = "";
                try
                {
                    response = await _wordService.Remove(Context.Guild.Id, words.Select(x => x.ToLower()).ToList());
                }
                catch (Exception e)
                {
                    System.Console.Error.Write(e);
                    await ErrorMessage();
                }

                if (string.IsNullOrEmpty(response))
                {
                    await ReplyAsync("Removed.");
                }
                else
                {
                    await ReplyAsync(response);
                }
            }
            else
            {
                await ReplyAsync("Nothing to remove.");
            }
        }

        [Command("list")]
        [Summary("List the words I'm tracking")]
        public async Task ListAsync()
        {
            var words = new List<WordCount>();
            try
            {
                words = await _wordService.List(Context.Guild.Id);
            }
            catch (Exception e)
            {
                System.Console.Error.Write(e);
                await ErrorMessage();
            }

            if (words.Any())
            {
                var builder = new EmbedBuilder();
                builder.WithTitle("Tracked Words");
                builder.WithDescription("And how many times I've seen each");

                words.OrderByDescending(x => x.Count).ToList().ForEach(word =>
                {
                    builder.AddField(word.Word, word.Count, true);
                });

                var embed = builder.Build();

                await ReplyAsync("", false, embed);
            }
            else
            {
                await ReplyAsync($"Add some words for me to track first!");
            }
        }

        [Command("reset")]
        [Summary("Reset my tracking statistics")]
        public async Task ResetAsync(params string[] wordsToReset)
        {
            if (!wordsToReset.Any())
            {
                string response = "";

                try
                {
                    response = await _wordService.Reset(Context.Guild.Id, wordsToReset.Select(x => x.ToLower()).ToList());
                }
                catch (Exception e)
                {
                    System.Console.Error.Write(e);
                    await ErrorMessage();
                }

                if (string.IsNullOrEmpty(response))
                {
                    await ReplyAsync("Reset all tracking statistics.");
                }
                else
                {
                    await ReplyAsync(response);
                }
            }
            else
            {
                await ReplyAsync("Resetting individual word statistics is currently unsupported.");
            }
        }

        private async Task ErrorMessage()
        {
            await ReplyAsync("An error has occured. Please try again.");
        }
    }
}