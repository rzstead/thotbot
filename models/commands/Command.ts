import { Message } from 'discord.js';
import { ICommand } from './ICommand';

export abstract class Command implements ICommand {
	abstract run(msg: Message): void;
}
