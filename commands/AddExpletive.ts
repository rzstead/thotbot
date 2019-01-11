import { Command } from '../commands/Command';
import { Message, Client, TextChannel, TextBasedChannel } from 'discord.js';

export class AddExpletive extends Command{
    
    constructor(client: Client){
        super(client);
    }

    run(msg: Message){
        msg.channel.send(`You want to add an expletive with the properties: ${msg.content}`);
    }
}