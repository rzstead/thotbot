using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Thot.Listener.Util;
using Thot.Listener.Commands;
using System.Timers;
using System;

namespace Thot.Listener
{
    public class Startup
    {
        private const int REFRESH_TIMEOUT = 10;
        private readonly Config _config = new Config();
        private DiscordSocketClient _discordClient;
        private CommandHandler _commandHandler;

        public static void Main(string[] args)
            => new Startup().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _discordClient = new DiscordSocketClient();

            var services = new Initialize(_discordClient).BuildServiceProvider();
            _commandHandler = (CommandHandler)services.GetService(typeof(CommandHandler));

            _discordClient.Ready += RegisterDiscordEvents;

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task RegisterDiscordEvents()
        {
            _discordClient.JoinedGuild += async (SocketGuild guild) =>
            {
                await guild.SystemChannel.SendMessageAsync("Hiya, I'm Thotbot. Use !thot.help to see what I can do!");
            };

            await _discordClient.SetActivityAsync(new ThotActivity());
            await _commandHandler.InstallCommandsAsync();
            await _discordClient.DownloadUsersAsync(_discordClient.Guilds);
            System.Console.WriteLine("Ready!");
        }
    }
}
