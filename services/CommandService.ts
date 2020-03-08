import { MongoRepo } from "../repositories/MongoRepo";
import { Expletive, Leaderboard } from "../models/schemas";
import { DocumentType } from "@typegoose/typegoose";

export class CommandService {

    private constructor(){}

    private static repo: MongoRepo = new MongoRepo();

    public static async addExpletives(guildId: string, expletives: string[]): Promise<Expletive[]> {
        return await this.repo.addExpletives(guildId, expletives);
    }

    public static async removeExpletives(guildId: string, expletives: string[]): Promise<void> {
        return await this.repo.removeExpletives(guildId, expletives);
    }

    public static async getExpletives(guildId: string): Promise<Expletive[]> {
        return await this.repo.getExpletives(guildId);
    }

    public static async updateExpletives(expletives: Expletive[]){
        return await this.repo.updateExpletives(expletives);
    }

    public static async getExpletivesByUser(guildId: string, userId: string) {
        return await this.repo.getExpletivesByUser(guildId, userId);
    }

    // public static async backScan(backScanData: Leaderboard[]){
    //     return await this.repo.addBackScanData(backScanData);
    // }
}