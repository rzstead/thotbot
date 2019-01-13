import { Command } from '../commands/Command';
import { Message, Client, TextChannel, TextBasedChannel } from 'discord.js';
import { CommandService } from '../../services/CommandService';

export class AddExpletive extends Command {

    public constructor(client: Client, commandService: CommandService) {
        super(client, commandService);
    }

    public async run(msg: Message) {
        let expletiveList = msg.content.split(' ');
        console.log("Attempting to add expletives: " + expletiveList);

        var result = await this.commandService.addExpletives(msg.guild.id, expletiveList);
        let expletiveNames: Array<string> = new Array<string>();

        result.forEach(expletive =>{
            expletiveNames.push(`'${expletive.expletive}'`)
        });

        msg.channel.send('Added ' + expletiveNames);
        
        console.log(result);
    } 
}