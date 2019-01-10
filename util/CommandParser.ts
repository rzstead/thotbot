import { ICommand } from '../interfaces/ICommand';
import { AddExpletive } from '../commands/AddExpletive';
import { Client } from 'discord.js';
export class CommandParser{

    private _client;

    CommandParser(client: Client){
        this._client = client;
    }
    
    static parseCommand(): ICommand{
        return null;
    }

}

enum CCommands {
    eadd = new AddExpletive(_client),
    a = 0,
    erem = 1,
    r = 1,
    board = 2,
    b = 2,
    backscan = 3,
    bs = 3
}