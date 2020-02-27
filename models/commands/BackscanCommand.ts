import { Command } from './Command';
import { Message, Client, TextChannel, DMChannel, GroupDMChannel, GuildChannel, Collection } from 'discord.js';
import { CommandService } from '../../services/CommandService';
import { stringify } from 'querystring';

export class BackscanCommand extends Command {

    constructor(client: Client, commandService: CommandService) {
        super(client, commandService);
    }

    public async run(msg: Message) {
        //scan all channel of guild
        //for every channel
        //for every message by user?
        msg.channel.send(`You want to backscan with the properties: ${msg.content}`);
        
        let textChannels= msg.guild.channels.filter((x => x.type === 'text'));
        console.log(`Found text channels: ${textChannels.map(x => x.id + " - " + x.name)}`);
        let messageStack: Collection<string, Message> = new Collection<string, Message>();

        for (var [id, channel] of textChannels) {         
            console.log(`Processing channel: ${channel.id}`);
            let messageId = msg.id;
            let callResponse: Collection<string, Message> = new Collection<string, Message>();
            let channelMessageAmount: number = 0;
            do{  
                try{
                    callResponse = await (channel as TextChannel).fetchMessages({before: messageId});
                    if(callResponse.size > 0){
                        callResponse.sort((x, y) => x.createdAt.getTime() - y.createdAt.getTime());
                        callResponse.map(response => { messageStack.set(response.id, response) });
                        channelMessageAmount += callResponse.size;
                        messageId = callResponse.last().id;
                    }
                }catch(err){
                    console.log(err.message);
                }        
            }while(callResponse.size > 0);
            msg.channel.send(`Processed ${channelMessageAmount} records for channel ${channel.name}`);
        }
    }
}