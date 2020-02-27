import { prop, getModelForClass } from '@typegoose/typegoose';
export class Leaderboard {

    @prop({required: true})
    guildId: string;
    
    @prop({required: true})
    userId: string;

    @prop()
    expletive: string;

    @prop()
    occurrence: number;

}

export const LeaderboardEntity = getModelForClass(Leaderboard);