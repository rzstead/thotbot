import { Client, Message } from "discord.js";
import { ICommand } from "../interfaces/ICommand";

export abstract class Command implements ICommand{

    constructor(protected client: Client){}

    abstract run(msg: Message);
    
}