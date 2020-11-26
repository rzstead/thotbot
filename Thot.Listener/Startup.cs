using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Thot.Listener.Util;
using Thot.Listener.Commands;

namespace Thot.Listener
{
    public class Startup
    {
        private readonly Config _config = new Config();
        private DiscordSocketClient _discordClient;
        private CommandHandler _commandHandler;

        public static async Task Main(string[] args)
            => await new Startup().MainAsync();

        public async Task MainAsync()
        {
            _discordClient = new DiscordSocketClient();

            var services = new Initialize(_discordClient).BuildServiceProvider();
            _commandHandler = (CommandHandler)services.GetService(typeof(CommandHandler));

            _discordClient.Ready += RegisterDiscordEvents;
            await _discordClient.LoginAsync(TokenType.Bot, _config.DiscordToken);
            await _discordClient.StartAsync();

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
