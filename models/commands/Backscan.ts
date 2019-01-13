import { Command } from '../commands/Command';
import { Message, Client, TextChannel, DMChannel, GroupDMChannel, GuildChannel } from 'discord.js';
import { CommandService } from '../../services/CommandService';

export class Backscan extends Command {

    constructor(client: Client, commandService: CommandService) {
        super(client, commandService);
    }

    run(msg: Message) {
        //scan all channel of guild
        //for every channel
        //for every message by user?
        msg.channel.send(`You want to backscan with the properties: ${msg.content}`);
        
        let channelKeys = msg.guild.channels.keyArray;

        for (let index = 0; index < channelKeys.length; index++) {
            let channel: GuildChannel = msg.guild.channels[channelKeys[index]];

            if(typeof channel === typeof TextChannel){
                (channel as TextChannel).fetchMessages()
            }
        }
    }
}