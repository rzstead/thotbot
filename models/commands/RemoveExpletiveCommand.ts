import { Command } from './Command';
import { Message, Client } from 'discord.js';
import { CommandService } from '../../services/CommandService';

export class RemoveExpletiveCommand extends Command {
    public async run(msg: Message) {
        msg.channel.send(`You want to remove an expletive with the properties: ${msg.content}`);
        let expletiveList = msg.content.split(' ');
        await CommandService.removeExpletives(msg.guild.id, expletiveList);       
    }
}