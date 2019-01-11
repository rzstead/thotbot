import { Command } from '../commands/Command';
import { Message, Client } from 'discord.js';

export class RemoveExpletive extends Command {

    constructor(client: Client) {
        super(client);
    }

    run(msg: Message) {
        msg.channel.send(`You want to remove an expletive with the properties: ${msg.content}`);
    }
}