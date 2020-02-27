import { ICommand } from '../models/commands/ICommand';
import { AddExpletiveCommand, RemoveExpletiveCommand, LeaderboardCommand, BackscanCommand, ListExpletivesCommand } from '../models/commands';
import { Client } from 'discord.js';
import { CommandService } from './CommandService';
export class CommandParser {

    public static parseCommand(client: Client, commandService: CommandService, command: string): ICommand {
        return this.getCommand(client, commandService, command);
    }

    private static getCommand(client: Client, commandService: CommandService, commandString: string): ICommand {
        let command: Command = commandString as Command;
        if (command !== undefined) {
            return this.initCommand(client, commandService, command);
        }
        console.log("Parsed command value: " + command);
        return null;
    }

    private static initCommand(client: Client, commandService: CommandService, command: Command): ICommand {
        switch (command) {
            case Command.ADD:
                return new AddExpletiveCommand(client, commandService);
            case Command.REMOVE:
                return new RemoveExpletiveCommand(client, commandService);
            case Command.LIST:
                return new ListExpletivesCommand(client, commandService);
            case Command.BOARD:
                return new LeaderboardCommand(client, commandService);
            case Command.BACKSCAN:
                return new BackscanCommand(client, commandService);
        }
    }
}

enum Command {
    ADD = "add",
    A = "a",
    REMOVE = "remove",
    R = "r",
    BOARD = 'board',
    B = "b",
    BACKSCAN = "backscan",
    BS = "bs",
    LIST = "list",
    L = "l",
}

