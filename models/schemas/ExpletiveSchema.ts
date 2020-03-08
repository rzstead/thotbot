import {prop, getModelForClass } from '@typegoose/typegoose';

export class UserOccurences {
    @prop({required:true})
    userId: string;

    @prop({required: true})
    occurences: number;
}

export class Expletive {
    @prop({required: true})
    id: string;
    
    @prop({required: true})
    guildId: string;

    @prop({required: true})
    expletive: string;

    @prop({required: true})
    totalOccurences: number;

    @prop({ref:UserOccurences})
    userOccurences: UserOccurences[]
}

export const ExpletiveEntity = getModelForClass(Expletive);
