import { Client, Message } from 'discord.js';
import { Database } from 'sqlite3';
import { IConfig } from 'config';

const config: IConfig = require('config');
const token: string = config.get('discord.token');

let sqlite: Database = new Database('./db/basic.db', err => {
    if (err) {
        console.log('DB connection error!')
    } else {
        console.log('Connected to db.')
    }
});

let client = new Client();

client.login(token);

client
    .on('error', console.error)
    .on('warn', console.warn)
    .on('debug', console.log)
    .on('ready', () => {
        console.log(`Client ready; logged in as ${client.user.username}#${client.user.discriminator} (${client.user.id})`);
        client.user.setActivity("over my christian servers", {
            type: "WATCHING"
        })
    })
    .on('disconnect', () => { console.warn('Disconnected!'); })
    .on('reconnecting', () => { console.warn('Reconnecting...'); })
    .on('message', (msg) => onMessage(msg));

function onMessage(msg: Message) {
    if (msg.content.startsWith('!thot')) {
        msg.content = msg.content.slice(5);
        let command = msg.content.split(' ')[0];
        msg.content = msg.content.slice(command.length + 1);
        handleCommand(command, msg);
    }
}

function handleCommand(command: string, msg: Message) {
    let cCommand = parseCommand(command.toLocaleLowerCase());
    let properties = msg.content.split(' ');
    msg.channel.send(`You called me with the command '${command}' and ${properties.length > 0 && properties[0].length > 0 ?  "'" + msg.content + "'" : "no properties."}`);
    if (cCommand) {
        switch (cCommand.valueOf()) {
            case 0:
                addExpletives(msg, properties);
                break;
            case 1:
                removeExpletives(msg, properties);
                break;
            case 2:
                showLeaderboard(msg);
                break;
            case 3:
                scanHistory(msg);
                break;
            default:
                break;
        }
    }
    else {
        unrecognizedCommand(msg, command);
    }
}

function unrecognizedCommand(msg: Message, command: string) {
    msg.channel.send(`'${command}' is not a recognized command.`)
}



function removeExpletives(msg: Message, properties: string[]) {
    msg.channel.send(`You'd like to remove the expletives: ${properties}`);
}

function showLeaderboard(msg: Message) {
    msg.channel.send("You want to show the leaderboard.");
}

function scanHistory(msg: Message) {
    msg.channel.send("You'd like to scan the channel history.");
}


