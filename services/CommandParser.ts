import { ICommand } from '../models/interfaces/ICommand';
import { AddExpletive, RemoveExpletive, Leaderboard, Backscan } from '../models/commands';
import { Client } from 'discord.js';
import { CommandService } from './CommandService';
export class CommandParser {

    public static parseCommand(client: Client, commandService: CommandService, command: string): ICommand {
        return this.getCommand(client, commandService, command);
    }

    private static getCommand(client: Client, commandService: CommandService, command: string): ICommand {
        let commandVal = Commands[command.toLocaleUpperCase()];
        console.log("Parsed command value: " + commandVal);
        if (commandVal != undefined) {
            return this.initCommand(client, commandService,  commandVal);
        }

        return null;
    }

    private static initCommand(client: Client, commandService: CommandService, commandVal: number): ICommand {
        switch (commandVal) {
            case Commands.ADD.valueOf():
                return new AddExpletive(client, commandService);
            case Commands.REMOVE.valueOf():
                return new RemoveExpletive(client, commandService);
            case Commands.BOARD.valueOf():
                return new Leaderboard(client, commandService);
            case Commands.BACKSCAN.valueOf():
                return new Backscan(client, commandService);
        }
    }
}

enum Commands {
    ADD = 0,
    A = 0,
    REMOVE = 1,
    R = 1,
    BOARD = 2,
    B = 2,
    BACKSCAN = 3,
    BS = 3
}

