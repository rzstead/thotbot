import { MongoRepo } from "../repositories/MongoRepo";
import { Expletive, Leaderboard } from "../models/schemas";

export class CommandService {

    private constructor(){}

    private static repo: MongoRepo = new MongoRepo();

    public static async addExpletives(guildId: string, expletives: string[]) {
        return await this.repo.addExpletives(guildId, expletives);
    }

    public static async removeExpletives(guildId: string, expletives: string[]) {
        return await this.repo.removeExpletives(guildId, expletives);

    }

    public static async getExpletives(guildId: string): Promise<Expletive[]> {
        return await this.repo.getExpletives(guildId);
    }

    public static async backScan(backScanData: Leaderboard[]){
        return await this.repo.addBackScanData(backScanData);
    }

    public static async getLeaderboard() {

    }

    public static async updateUserExpletiveCount(guildId: string, userId: string, expletiveMap: Map<string, number>) {
        let existingExpletiveData = this.repo.getExpletiveData(guildId, userId, Array.from(expletiveMap.keys()));
        
    }

    public static async updateGuildWideExpletiveTotals(guildId: string, expletives: Expletive[]){

    }

    public static async resetLeaderboard() {

    }
}