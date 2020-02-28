import { Command } from './Command';
import { Message, Client, TextChannel, TextBasedChannel } from 'discord.js';
import { CommandService } from '../../services/CommandService';

export class AddExpletiveCommand extends Command {
    public async run(msg: Message) {
        let expletiveList = msg.content.toLocaleLowerCase().split(' ');
        
        if(expletiveList.length > 0){
            
            console.log("Attempting to add expletives: " + expletiveList);

            var result = await CommandService.addExpletives(msg.guild.id, expletiveList);
            let expletiveNames: Array<string> = new Array<string>();
    
            result.forEach(expletive =>{
                expletiveNames.push(`'${expletive.expletive}'`)
            });
    
            msg.channel.send('Added ' + expletiveNames);
        }

        console.log(result);
    } 
}