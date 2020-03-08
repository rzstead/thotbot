import { Command } from '../commands/Command';
import { Message } from 'discord.js';
import { CommandService } from '../../services/CommandService';
import { Expletive } from '../schemas/ExpletiveSchema';

export class LeaderboardCommand extends Command {
	async run(msg: Message) {
		let expletives: Expletive[] = await CommandService.getExpletives(msg.guild.id);

		

		msg.channel.send(
			`You want to see the leaderboard with the properties: ${msg.content}`
		);
	}
}
