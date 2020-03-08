import { IConfig } from 'config';
import { Mongoose, Connection, DocumentQuery, mongo } from 'mongoose';
import { LeaderboardEntity, ExpletiveEntity, Expletive, Leaderboard } from '../models/schemas/index';

export class MongoRepo {

    config: IConfig = require('config');
    mongoose: Mongoose = require('mongoose');
    connectionString: string = this.config.get('mongo.connectionString');

    public async addExpletives(guildId: string, expletives: string[]): Promise<Expletive[]> {
        let mongoose = await this.openConnection();
        let result: Expletive[] = [];

        let results = expletives.map(async expletive => {
            if (!(await this.expletiveExists(guildId, expletive))) {
                console.log(`did not find expletive ${expletive}, adding...`)
                return await ExpletiveEntity.create({
                    guildId: guildId,
                    expletive: expletive,
                    totalOccurences: 0
                });
            }
        });

        result = await Promise.all(results);

        this.closeConnection(mongoose.connection);
        return result;
    }

    public async removeExpletives(guildId: string, expletives: string[]) {
        let mongoose = await this.openConnection();

        let results = expletives.map(async expletive => {
            if (await this.expletiveExists(guildId, expletive)) {
                console.log(`expletive ${expletive} exists, removing...`)
                return await ExpletiveEntity.deleteOne({ 'expletive': expletive, 'guildId': guildId })
            }
        });

        await Promise.all(results);

        this.closeConnection(mongoose.connection);
    }

    public async getExpletives(guildId: string): Promise<Expletive[]> {
        let mongoose = await this.openConnection();
        let result = await ExpletiveEntity.find({ 'guildId': guildId });
        await this.closeConnection(mongoose.connection);

        return result.map(expletiveEntity => {
            return { id:expletiveEntity.id,
                     guildId: expletiveEntity.guildId, 
                     expletive: expletiveEntity.expletive, 
                     totalOccurences: expletiveEntity.totalOccurences,
                     userOccurences: expletiveEntity.userOccurences
                    };
        });
    }

    public async updateExpletives(expletives: Expletive[]): Promise<Expletive[]> {
        let mongoose = await this.openConnection();

        let results = expletives.map(async expletive => {
            return ExpletiveEntity.findByIdAndUpdate(expletive.id, expletive);
        })
        
        let result = Promise.all(results);

        await this.closeConnection(mongoose.connection);

        return result;
    }

    public async getExpletivesByUser(guildId: string, userId: string): Promise<Expletive[]> {
        let mongoose = await this.openConnection();

        let results = ExpletiveEntity.find({ 'guildId': guildId, 'userId': userId });

        await this.closeConnection(mongoose.connection);
        return results;
    }

    private async openConnection() {
        return await this.mongoose.connect(this.connectionString, { useNewUrlParser: true });
    }

    private async closeConnection(connection: Connection) {
        await connection.close();
    }

    private async expletiveExists(guildId: string, expletive: string) {
        return await ExpletiveEntity.findOne({ 'guildId': guildId, 'expletive': expletive }) != undefined;
    }

    // public async addBackScanData(expletiveData: Leaderboard[]) {

    // }
}
