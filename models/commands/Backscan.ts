import { Command } from '../commands/Command';
import { Message, Client } from 'discord.js';
import { CommandService } from '../../services/CommandService';

export class Backscan extends Command {

    constructor(client: Client, commandService: CommandService) {
        super(client, commandService);
    }

    run(msg: Message) {
        msg.channel.send(`You want to backscan with the properties: ${msg.content}`);
    }
}