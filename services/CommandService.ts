import { MongoRepo } from "../repositories/MongoRepo";

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

    public async getLeaderboard() {

    }

    public async updateLeaderboard() {

    }

    public async resetLeaderboard() {

    }
}