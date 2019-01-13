import { MongoRepo } from "../repositories/MongoRepo";
import { User } from "discord.js";
import { IBoardData } from "../models/interfaces/IBoardData";

export class CommandService {

    repo = new MongoRepo();

    public async addExpletives(guildId: string, expletives: string[]) {
        return await this.repo.addExpletives(guildId, expletives);
    }

    public async removeExpletives(guildId: string, expletives: string[]) {
        return await this.repo.removeExpletives(guildId, expletives);

    }

    public async getExpletives(guildId: string) {
        return await this.repo.getExpletives(guildId);
    }

    public async backScan(backScanData: IBoardData[]){
        return await this.repo.addBackScanData(backScanData);
    }

    public async getLeaderboard() {

    }

    public async updateLeaderboard() {

    }

    public async resetLeaderboard() {

    }
}