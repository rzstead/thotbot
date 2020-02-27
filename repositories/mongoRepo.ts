import { IConfig } from 'config';
import { Mongoose, Connection } from 'mongoose';
import { LeaderboardEntity, ExpletiveEntity, Expletive, Leaderboard } from '../models/schemas/index';

export class MongoRepo {

    config: IConfig = require('config');
    mongoose: Mongoose = require('mongoose');
    connectionString: string = this.config.get('mongo.connectionString');

    public async addExpletives(guildId: string, expletives: string[]) {
        let mongoose = await this.openConnection();
        var result = new Array<any>();
        for (let index = 0; index < expletives.length; index++) {
            const expletive = expletives[index];
            let expletiveModel = new ExpletiveEntity();
            expletiveModel.guildId = guildId;
            expletiveModel.expletive = expletive;
            expletiveModel.totalOccurence = 0;
            if (!(await this.expletiveExists(guildId, expletive))) {
                console.log(`did not find expletive ${expletive}, adding...`)
                result.push(await expletiveModel.save());
            }
        }
        this.closeConnection(mongoose.connection);
        return result;
    }

    public async removeExpletives(guildId: string, expletives: string[]) {
        let mongoose = await this.openConnection();
        for (let index = 0; index < expletives.length; index++) {
            const expletive = expletives[index];
            if (await this.expletiveExists(guildId, expletive)) {
                console.log(`expletive ${expletive} exists, removing...`)
                await ExpletiveEntity.deleteOne({ 'expletive': expletive, 'guildId': guildId })
            }
        }
        this.closeConnection(mongoose.connection);
    }

    public async getExpletives(guildId: string): Promise<Expletive[]> {
        let mongoose = await this.openConnection();
        let result = await ExpletiveEntity.find({ 'guildId': guildId });
        await this.closeConnection(mongoose.connection);
        
        return result.map<Expletive>(expletiveEntity =>{
            return { guildId: expletiveEntity.guildId, expletive: expletiveEntity.expletive, totalOccurence: expletiveEntity.totalOccurence };
        });
    }

    public async getExpletiveData(guildId: string, userId: string, expletives: string[]): Promise<Leaderboard[]> {
        let mongoose = await this.openConnection();
        let resultSet: Leaderboard[] = [];
        for (let i = 0; i < expletives.length; i++) {
            const expletive = expletives[i];
            let result = await LeaderboardEntity.findOne({ 'guildId': guildId, 'userId': userId, 'expletive': expletive });
            let expletiveData: Leaderboard = { expletive: result.expletive, guildId: result.guildId, userId: result.userId, occurrence: result.occurrence };
            resultSet.push(expletiveData);
        }
        await this.closeConnection(mongoose.connection);
        return resultSet;
    }

    public async updateLeaderboard() {

    }

    public async resetLeaderboard() {

    }

    public async addBackScanData(expletiveData: Leaderboard[]) {

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

}
