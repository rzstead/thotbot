import { Command } from '../commands/Command';
import { Message, Client, TextChannel, TextBasedChannel } from 'discord.js';
import { CommandService } from '../../services/CommandService';

export class AddExpletive extends Command {

    public constructor(client: Client, commandService: CommandService) {
        super(client, commandService);
    }

    public async run(msg: Message) {
        msg.channel.send(`You want to add an expletive with the properties: ${msg.content}`);

        let result = await this.commandService.addExpletive(msg.guild.id, msg.content);

        console.log(result);

        let expletives: string = "";

        result.forEach(element => {
            expletives += element.expletive + ', ';
        });

        expletives = expletives.slice(0, expletives.length - 2);

        msg.channel.send(`Here's the list of expletives you've added so far: ${expletives}`);
    }
}