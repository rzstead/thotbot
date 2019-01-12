import { Typegoose, prop } from 'typegoose';
export class ExpletiveSchema extends Typegoose{

    @prop({required: true})
    guildId: string;

    @prop()
    expletive: string;

    @prop()
    totalOccurence: number;

}

export const ExpletiveModel = new ExpletiveSchema().getModelForClass(ExpletiveSchema);
