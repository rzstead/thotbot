import { ICommand } from './ICommand';
import * as Commands from './index';
interface CommandAlias {
    [key: string]: ICommand
}

export const commandAliases: CommandAlias = {
    "a": new Commands.AddExpletiveCommand(),
    "r": new Commands.RemoveExpletiveCommand(),
    "l": new Commands.ListExpletivesCommand(),
    "m": new Commands.UserExpletivesCommand(),
    //"backscan": new Commands.BackscanCommand(),
    "t": new Commands.LeaderboardCommand(),
}