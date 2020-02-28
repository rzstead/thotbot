import { Message, Client } from "discord.js";
import { ICommand } from "../models/commands/ICommand";
import { CommandService } from "./CommandService";
import { Expletive } from '../models/schemas/ExpletiveSchema';
import { commandAliases } from "../models/commands";
var cuss = require('cuss');

export class MessageHandler {

    public static handleMessage(msg: Message) {
        if (msg.content.startsWith('!thot')) {
            msg.content = msg.content.slice(5);
            let command = msg.content.split(' ')[0];
            msg.content = msg.content.slice(command.length + 1);
            this.handleCommand(command, msg);
        }else{
            this.parseExpletives(msg);
        }
    }

    private static async parseExpletives(msg: Message){
        let guildId = msg.guild.id;
        let userId = msg.author.id;

        //split message by spaces, to granularlize expletive checking
        let words = msg.content.toLowerCase().split(' ');
        words = words.map(word => word.replace(/[\W_]+/g,""));

        //parse the message for the existing expletives to match against
        words.forEach(word => {
            //check common expletive dictionary for a higher than unlikely rating for expletive
            // if (cuss[word] > 0) {
            //     if (expletiveList.includes(word)) {
            //         currentExpletivesMap[word]++;
            //         if (!currentUserExpletiveCount.has(word)) {
            //             currentUserExpletiveCount.set(word, 1);
            //         } else {
            //             currentUserExpletiveCount[word]++;
            //         }
            //     }
            // }
        });

        //get existing expletive map for guild - expletive:occurence
        let currentExpletives : Expletive[] = await CommandService.getExpletives(guildId);

        //create temp map for found expletives and their count
        let currentUserExpletiveCount : Map<string, number> = new Map<string, number>();

        //after finding matches set by each guild, update the leaderboard to reflect expletive count per user
        CommandService.updateUserExpletiveCount(guildId, userId, currentUserExpletiveCount);
        CommandService.updateGuildWideExpletiveTotals(guildId, currentExpletives);
    }

    private static handleCommand(command: string, msg: Message) {
        let parsedCommand: ICommand = commandAliases[command];

        if (parsedCommand) {
            parsedCommand.run(msg);
        }
        else {
            this.unrecognizedCommand(msg, command);
        }
    }

    private static unrecognizedCommand(msg: Message, command: string) {
        msg.channel.send(`'${command}' is not a recognized command.`)
    }
}