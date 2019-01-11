import { IConfig } from 'config';
import { Mongoose, Connection, Schema, Model, MongooseDocument, Document } from 'mongoose';
import { ErrorHandler } from '../services/errorHandler';

export class MongoRepo {
    config: IConfig = require('config');
    mongoose: Mongoose = require('mongoose');
    connectionString: string = this.config.get('mongo.connectionString');
    
    leaderboard: Model<Document> = this.mongoose.model('Leaderboard', 
        new Schema({
            expletive: String,
            occurrence: Number,
            guildId: String,
            userId: String
        })
    );

    expletives: Model<Document> = this.mongoose.model('Expletives', 
        new Schema({
            expletive: String,
            guildId: String,
            totalOccurence: Number
        })
    );

    private constructor(){
       
    }

    public async addExpletive() {
        this.openConnection()
        .then(db =>{
            //this.expletives.create(new expl, 
            }, 
            ErrorHandler.handleError)
        .catch(ErrorHandler.handleError);
    }

    public async removeExpletive() {

    }

    public async getExpletives() {

    }

    public async getLeaderboard() {

    }

    public async updateLeaderboard() {

    }

    public async resetLeaderboard() {

    }

    private async openConnection() {
        return await this.mongoose.connect(this.connectionString);
    }
}