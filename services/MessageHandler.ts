import { Message, Client } from "discord.js";
import { CommandParser } from "./CommandParser";
import { ICommand } from "../interfaces/ICommand";

export class MessageHandler {

    public static handleMessage(client, msg: Message) {
        if (msg.content.startsWith('!thot')) {
            msg.content = msg.content.slice(5);
            let command = msg.content.split(' ')[0];
            msg.content = msg.content.slice(command.length + 1);
            this.handleCommand(client, command, msg);
        }
    }

    private static handleCommand(client: Client, command: string, msg: Message) {
        let parsedCommand: ICommand = CommandParser.parseCommand(client, command);

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