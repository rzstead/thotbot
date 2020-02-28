import { Command } from '../commands/Command';
import { Message, Client } from 'discord.js';
import { CommandService } from '../../services/CommandService';

export class LeaderboardCommand extends Command {
	run(msg: Message) {
		msg.channel.send(
			`You want to see the leaderboard with the properties: ${msg.content}`
		);
	}
}
