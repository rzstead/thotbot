import { ICommand } from './ICommand';
import * as Commands from './index';
interface CommandAlias {
    [key: string]: ICommand
}

export const commandAliases: CommandAlias = {
    "add": new Commands.AddExpletiveCommand(),
    "remove": new Commands.RemoveExpletiveCommand(),
    "list": new Commands.ListExpletivesCommand(),
    "backscan": new Commands.BackscanCommand(),
    "top": new Commands.LeaderboardCommand()
}