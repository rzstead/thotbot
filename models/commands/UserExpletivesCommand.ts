import { Message, MessageOptions, MessageEmbed, RichEmbed } from 'discord.js';
import { Command } from './Command';
import { CommandService } from '../../services/CommandService';
import { Expletive } from '../schemas/ExpletiveSchema';

export class UserExpletivesCommand extends Command {
	async run(msg: Message) {
		let authorId = msg.author.id;
		let expletives: Expletive[] = await CommandService.getExpletivesByUser(msg.guild.id, authorId);		

		let embed = new RichEmbed()
		.setColor('#F0435B')
		.setTitle('Saucy!')
		.setDescription(`<@${authorId}>'s filthy mouth is something!`);

		expletives.forEach(expletive => {
			let occurenceVal = expletive.userOccurences.find(occurrence => occurrence.userId === authorId).occurences
			embed.addField(expletive.expletive, occurenceVal, true);
		});

		msg.channel.sendEmbed(embed);
	}
}
