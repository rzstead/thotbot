import { Client } from 'discord.js';
import { IConfig } from 'config';
import { MessageHandler } from './services/MessageHandler';

class Thotbot {
    config: IConfig = require('config');

    public start() {
        let client: Client = new Client();
        let token: string = this.config.get('discord.token');

        client.login(token);
        client
            .on('error', console.error)
            .on('warn', console.warn)
            .on('debug', console.log)
            .on('rateLimit', console.warn)
            .on('ready', () => {
                console.log(`Client ready; logged in as ${client.user.username}#${client.user.discriminator} (${client.user.id})`);
                client.user.setActivity("over my christian servers", {
                    type: "WATCHING"
                });
            })
            .on('disconnect', () => { console.warn('Disconnected!'); })
            .on('reconnecting', () => { console.warn('Reconnecting...'); })
            .on('message', (msg) => MessageHandler.handleMessage(msg));      
    }
}

new Thotbot().start();



