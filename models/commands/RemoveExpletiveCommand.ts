import { Command } from './Command';
import { Message, Client } from 'discord.js';
import { CommandService } from '../../services/CommandService';

export class RemoveExpletiveCommand extends Command {
    public async run(msg: Message) {
        let expletiveList = msg.content.split(' ');
        await CommandService.removeExpletives(msg.guild.id, expletiveList);      
        msg.channel.send(`You removed the expletives: ${msg.content}`);
    }
}