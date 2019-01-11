import { Client, Message } from 'discord.js';
import { IConfig } from 'config';
import { CommandParser } from './util/CommandParser';
import { ICommand } from './interfaces/ICommand';

class Thotbot{

    config: IConfig = require('config');
    token: string = this.config.get('discord.token');
    client: Client = new Client();
    
    public start(){
        this.client.login(this.token);
        this.client
        .on('error', console.error)
        .on('warn', console.warn)
        .on('debug', console.log)
        .on('ready', () => {
            console.log(`Client ready; logged in as ${this.client.user.username}#${this.client.user.discriminator} (${this.client.user.id})`);
            this.client.user.setActivity("over my christian servers", {
                type: "WATCHING"
            })
        })
        .on('disconnect', () => { console.warn('Disconnected!'); })
        .on('reconnecting', () => { console.warn('Reconnecting...'); })
        .on('message', (msg) => this.onMessage(msg));
    }
    
    private onMessage(msg: Message) {
        if (msg.content.startsWith('!thot')) {
            msg.content = msg.content.slice(5);
            let command = msg.content.split(' ')[0];
            msg.content = msg.content.slice(command.length + 1);
            this.handleCommand(command, msg);
        }
    }
    
    private handleCommand(command: string, msg: Message) {
        let parsedCommand: ICommand = CommandParser.parseCommand(this.client, command);
    
        if (parsedCommand) {
            parsedCommand.run(msg);
        }
        else {
            this.unrecognizedCommand(msg, command);
        }
    }
    
    private unrecognizedCommand(msg: Message, command: string) {
        msg.channel.send(`'${command}' is not a recognized command.`)
    }
}

new Thotbot().start();



