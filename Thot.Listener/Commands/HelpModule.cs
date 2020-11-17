using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using System.Linq;
using Discord;


namespace Thot.Listener.Commands
{
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private CommandService _commandService;
        public HelpModule(CommandService commandService)
        {
            _commandService = commandService;
        }

        [Command("help")]
        [Summary("The command you're running now!")]
        public async Task HelpAsync()
        {
            var commands = _commandService.Commands.ToList();
            var summaryBuilder = new EmbedBuilder();

            commands.ForEach(command =>
            {
                summaryBuilder.AddField($"{command.Module.Group}  {command.Name}", command.Summary ?? "No description provided.");
            });

            await ReplyAsync("Here's the list of things you can ask me to do: ", false, summaryBuilder.Build());
        }
    }
}