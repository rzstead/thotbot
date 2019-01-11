import { IConfig } from 'config';
import { Mongoose, Connection } from 'mongoose';

export class MongoRepo {
    config: IConfig = require('config');
    mongoose: Mongoose = require('mongoose');
    connectionString: string = this.config.get('mongo.connectionString');

    public async addExpletive() {
        this.openConnection();
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