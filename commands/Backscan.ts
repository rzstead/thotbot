import { Command } from '../commands/Command';
import { Message, Client } from 'discord.js';

export class Backscan extends Command {

    constructor(client: Client) {
        super(client);
    }

    run(msg: Message) {
        msg.channel.send(`You want to backscan with the properties: ${msg.content}`);
    }
}