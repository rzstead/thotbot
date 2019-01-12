import { MongoRepo } from "../repositories/MongoRepo";

export class CommandService {

    repo = new MongoRepo();

    public async addExpletive(guildId: string, expletive: string) {
        return await this.repo.addExpletive(guildId, expletive);
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
}