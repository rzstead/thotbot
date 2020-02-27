import {prop, getModelForClass } from '@typegoose/typegoose';
export class Expletive {
    
    @prop({required: true})
    guildId: string;

    @prop()
    expletive: string;

    @prop()
    totalOccurence: number;
}

export const ExpletiveEntity = getModelForClass(Expletive);
