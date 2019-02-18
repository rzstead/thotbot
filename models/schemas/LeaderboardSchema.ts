import { Typegoose, prop } from 'typegoose';
export class LeaderboardSchema extends Typegoose{

    @prop({required: true})
    guildId: string;
    
    @prop({required: true})
    userId: string;

    @prop()
    expletive: string;

    @prop()
    occurrence: number;

}

export const LeaderboardModel = new LeaderboardSchema().getModelForClass(LeaderboardSchema);