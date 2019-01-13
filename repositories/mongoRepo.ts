import { IConfig } from 'config';
import { Mongoose, Connection } from 'mongoose';
import { ErrorHandler } from '../services/errorHandler';
import { LeaderboardModel, ExpletiveModel, ExpletiveSchema } from '../models/schemas/index';

export class MongoRepo {

    config: IConfig = require('config');
    mongoose: Mongoose = require('mongoose');
    connectionString: string = this.config.get('mongo.connectionString');

    public async addExpletives(guildId: string, expletives: string[]) {
        let mongoose = await this.openConnection();        
        var result = new Array<any>();
        for (let index = 0; index < expletives.length; index++) {
            const expletive = expletives[index];
            let expletiveModel = new ExpletiveModel();
                expletiveModel.guildId = guildId;
                expletiveModel.expletive = expletive;
                expletiveModel.totalOccurence = 0;
            if(!(await this.expletiveExists(guildId, expletive))){
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
            if(await this.expletiveExists(guildId, expletive)){
                console.log(`expletive ${expletive} exists, removing...`)
                await ExpletiveModel.deleteOne({'expletive': expletive, 'guildId': guildId })
            }
        }
        this.closeConnection(mongoose.connection);
    }

    public async getExpletives(guildId: string) {
        let mongoose = await this.openConnection();
        let result = await ExpletiveModel.find({ 'guildId': guildId });
        await this.closeConnection(mongoose.connection);
        return result;
    }

    public async getLeaderboard() {

    }

    public async updateLeaderboard() {

    }

    public async resetLeaderboard() {

    }

    public async addBackScanData(expletiveData: IBoardData[]){

    }

    private async openConnection() {
        return await this.mongoose.connect(this.connectionString, {useNewUrlParser: true});
    }

    private async closeConnection(connection: Connection){
        await connection.close();
    }

    private async expletiveExists(guildId: string, expletive: string){
        return await ExpletiveModel.findOne({'guildId': guildId, 'expletive': expletive}) != undefined;
    }
    
}
