import { MongoRepo } from "../repositories/MongoRepo";
import { Expletive, Leaderboard } from "../models/schemas";

export class CommandService {

    repo = new MongoRepo();

    public async addExpletives(guildId: string, expletives: string[]) {
        return await this.repo.addExpletives(guildId, expletives);
    }

    public async removeExpletives(guildId: string, expletives: string[]) {
        return await this.repo.removeExpletives(guildId, expletives);

    }

    public async getExpletives(guildId: string): Promise<Expletive[]> {
        return await this.repo.getExpletives(guildId);
    }

    public async backScan(backScanData: Leaderboard[]){
        return await this.repo.addBackScanData(backScanData);
    }

    public async getLeaderboard() {

    }

    public async updateUserExpletiveCount(guildId: string, userId: string, expletiveMap: Map<string, number>) {
        let existingExpletiveData = this.repo.getExpletiveData(guildId, userId, Array.from(expletiveMap.keys()));
        
    }

    public async updateGuildWideExpletiveTotals(guildId: string, expletives: Expletive[]){

    }

    public async resetLeaderboard() {

    }
}