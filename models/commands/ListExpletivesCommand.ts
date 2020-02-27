import { Command } from './Command';
import { Message, Client, TextChannel, TextBasedChannel } from 'discord.js';
import { CommandService } from '../../services/CommandService';
import { Expletive } from '../schemas';

export class ListExpletivesCommand extends Command {
	public constructor(client: Client, commandService: CommandService) {
		super(client, commandService);
	}

	public async run(msg: Message) {
		let result: Expletive[] = await this.commandService.getExpletives(msg.guild.id);
		let expletiveNames: Array<string> = new Array<string>();

		result.forEach(expletive => {
			expletiveNames.push(`\n'${expletive.expletive}'`);
		});

		msg.channel.send(`Here's the expletives you're tracking: ${expletiveNames}`);
	}
}
