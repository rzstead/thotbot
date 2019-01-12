import { IConfig } from 'config';
import { Mongoose } from 'mongoose';
import { ErrorHandler } from '../services/errorHandler';
import { LeaderboardModel, ExpletiveModel } from '../models/schemas/index';

export class MongoRepo {

    config: IConfig = require('config');
    mongoose: Mongoose = require('mongoose');
    connectionString: string = this.config.get('mongo.connectionString');

    public async addExpletive(guildId: string, expletive: string) {
        var result;
        await this.openConnection()
            .then(async () =>{
                let expletiveModel = new ExpletiveModel();
                expletiveModel.guildId = guildId;
                expletiveModel.expletive = expletive;
                expletiveModel.totalOccurence = 0;

                await expletiveModel.save();
                
                result = await ExpletiveModel.find();

            }, ErrorHandler.handleError)
            .catch(ErrorHandler.handleError);
        
        return result;
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