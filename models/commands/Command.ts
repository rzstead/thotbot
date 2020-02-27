import { Client, Message } from 'discord.js';
import { ICommand } from './ICommand';
import { CommandService } from '../../services/CommandService';

export abstract class Command implements ICommand {
	constructor(
		protected client: Client,
		protected commandService: CommandService
	) {}

	abstract run(msg: Message): void;
}
