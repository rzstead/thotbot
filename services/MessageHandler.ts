import { Message, Client } from "discord.js";
import { ICommand } from "../models/commands/ICommand";
import { CommandService } from "./CommandService";
import { Expletive, UserOccurences } from '../models/schemas/ExpletiveSchema';
import { commandAliases } from "../models/commands";

var cuss = require('cuss');

export class MessageHandler {

    public static async handleMessage(msg: Message): Promise<void> {
        if (msg.content.startsWith('!thot')) {
            msg.content = msg.content.slice(5);
            let command = msg.content.split(' ')[0];
            msg.content = msg.content.slice(command.length + 1);
            this.handleCommand(command, msg);
        } else {
            this.parseExpletives(msg);
        }
    }

    private static async parseExpletives(msg: Message): Promise<void> {
        let guildId = msg.guild.id;
        let userId = msg.author.id;

        //split message by spaces, to granularlize expletive checking
        let words = msg.content.toLowerCase().split(' ');
        words = words.map(word => word.replace(/[\W_]+/g, ""));

        let potentialExpletives: Map<string, number> = new Map<string, number>();
        //parse the message for the existing expletives to match against
        words.forEach(word => {
            //check common expletive dictionary for a higher than unlikely rating for expletive
            if (cuss[word] > 0) {
                if (potentialExpletives.has(word)) {
                    let currVal: number = potentialExpletives.get(word);
                    potentialExpletives.set(word, currVal++);
                }
                else {
                    potentialExpletives.set(word, 1);
                }
            }
        });

        if (potentialExpletives.size > 0) {
            //get existing expletive map for guild - expletive:occurence
            let guildExpletives: Expletive[] = await CommandService.getExpletives(guildId);

            guildExpletives.forEach(expletive => {
                if (potentialExpletives.has(expletive.expletive)) {

                    expletive.totalOccurences++;

                    let userOccurences = expletive.userOccurences.find(x => x.userId === userId);
                    if (userOccurences) {
                        userOccurences.occurences++;
                    }
                    else {
                        expletive.userOccurences.push({
                            userId: userId,
                            occurences: 1
                        });
                    }
                }
            });

            CommandService.updateExpletives(guildExpletives)

            //after finding matches set by each guild, update the leaderboard to reflect expletive count per user
            //CommandService.updateUserExpletiveCount(guildId, userId, currentUserExpletiveCount);
            //CommandService.updateGuildWideExpletiveTotals(guildId, currentExpletives);
        }
    }

    private static handleCommand(command: string, msg: Message): void {
        let parsedCommand: ICommand = commandAliases[command];

        if (parsedCommand) {
            parsedCommand.run(msg);
        }
        else {
            this.unrecognizedCommand(msg, command);
        }
    }

    private static unrecognizedCommand(msg: Message, command: string): void {
        msg.channel.send(`'${command}' is not a recognized command.`)
    }
}