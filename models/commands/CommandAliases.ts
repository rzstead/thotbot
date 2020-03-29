import { ICommand } from './ICommand';
import * as Commands from './index';
interface CommandAlias {
    [key: string]: ICommand
}

export const commandAliases: CommandAlias = {
    "a": new Commands.AddExpletiveCommand(),
    "l": new Commands.ListExpletivesCommand(),
    "m": new Commands.UserExpletivesCommand(),
    "r": new Commands.RemoveExpletiveCommand(),
    "t": new Commands.LeaderboardCommand(),
    //"backscan": new Commands.BackscanCommand(),
}