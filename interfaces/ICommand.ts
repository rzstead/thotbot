import { Message } from "discord.js";

export interface ICommand {
    run(msg: Message);
}