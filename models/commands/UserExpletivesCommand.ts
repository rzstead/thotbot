import { Message } from 'discord.js';
import { Command } from './Command';
import { CommandService } from '../../services/CommandService';
import { Expletive } from '../schemas/ExpletiveSchema';

export class UserExpletivesCommand extends Command {
	async run(msg: Message) {
		let expletives: Expletive[] = await CommandService.getExpletivesByUser(msg.guild.id, msg.author.id);

		msg.channel.send(
			`You want to see the leaderboard with the properties: ${msg.content}`
		);
	}
}