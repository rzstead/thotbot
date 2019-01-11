import { Command } from '../commands/Command';
import { Message, Client } from 'discord.js';

export class Leaderboard extends Command{
    
    constructor(client: Client){
        super(client);
    }

    run(msg: Message){
        msg.channel.send(`You want to see the leaderboard with the properties: ${msg.content}`);        
    }
}