import { Command } from '../commands/Command';
import { Message, Client } from 'discord.js';
import { CommandService } from '../../services/CommandService';

export class RemoveExpletive extends Command {

    constructor(client: Client, commandService: CommandService) {
        super(client, commandService);
    }

    run(msg: Message) {
        msg.channel.send(`You want to remove an expletive with the properties: ${msg.content}`);
    }
}