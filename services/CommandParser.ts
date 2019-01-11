import { ICommand } from '../interfaces/ICommand';
import { AddExpletive, RemoveExpletive, Leaderboard, Backscan } from '../commands/commands';
import { Client } from 'discord.js';
export class CommandParser {

    static parseCommand(client: Client, command: string): ICommand {
        return this.getCommand(client, command);
    }

    private static getCommand(client: Client, command: string): ICommand {
        let commandVal = Commands[command.toLocaleUpperCase()];
        console.log("Parsed command value: " + commandVal);
        if (commandVal != undefined) {
            return this.initCommand(client, commandVal);
        }

        return null;
    }

    private static initCommand(client: Client, commandVal: number): ICommand {
        switch (commandVal) {
            case Commands.ADD.valueOf():
                return new AddExpletive(client);
            case Commands.REMOVE.valueOf():
                return new RemoveExpletive(client);
            case Commands.BOARD.valueOf():
                return new Leaderboard(client);
            case Commands.BACKSCAN.valueOf():
                return new Backscan(client);
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

