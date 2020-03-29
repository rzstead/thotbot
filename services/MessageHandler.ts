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
            this.parseMessage(msg);
        }
    }

    private static async parseMessage(msg: Message): Promise<void> {
        let guildId = msg.guild.id;
        let userId = msg.author.id;

        //split message by spaces, to granularlize expletive checking
        let words = msg.content.toLowerCase().split(' ');
        words = words.map(word => word.replace(/[\W_]+/g, ""));

        let potentialExpletives: Map<string, number> = new Map<string, number>();
        //parse the message for the existing expletives to match against
        this.findPotentialExpletives(words, potentialExpletives);

        if (potentialExpletives.size > 0) {
            let guildExpletives: Expletive[] = await CommandService.getExpletives(guildId);

            this.populateGuildExpletives(guildExpletives, potentialExpletives, userId);

            CommandService.updateExpletives(guildExpletives)
        }
    }

    private static populateGuildExpletives(guildExpletives: Expletive[], potentialExpletives: Map<string, number>, userId: string) {
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
    }

    private static findPotentialExpletives(words: string[], potentialExpletives: Map<string, number>) {
        words.forEach(word => {
            //check common expletive dictionary for a higher than unlikely rating for expletive
            if (potentialExpletives.has(word)) {
                let currVal: number = potentialExpletives.get(word);
                potentialExpletives.set(word, currVal++);
            }
            else {
                potentialExpletives.set(word, 1);
            } 
        });
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