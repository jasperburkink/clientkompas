import Decimal from "decimal.js-light";

export default class CoachingProgramEdit {
    id: number;
    clientid: number;
    title: string;
    ordernumber?: string;
    organizationid?: number;
    coachingprogramtype: number;
    begindate?: Date;
    enddate?: Date;
    budgetammount?: Decimal;
    hourlyrate: Decimal;

    constructor(
        id: number,
        clientid: number,
        title: string,
        coachingprogramtype: number,        
        hourlyrate: Decimal,
        ordernumber?: string,
        organizationid?: number,
        begindate?: Date,
        enddate?: Date,
        budgetammount?: Decimal
    ) {
        this.id = id;
        this.clientid = clientid;
        this.title = title;
        this.coachingprogramtype = coachingprogramtype;
        this.begindate = begindate;
        this.enddate = enddate;
        this.hourlyrate = hourlyrate;
        this.budgetammount = budgetammount;
        this.ordernumber = ordernumber;
        this.organizationid = organizationid;
    }

    get remaininghours(): Decimal {
        if(!this.budgetammount) return new Decimal(0);

        try{

            let budgetammount = new Decimal(typeof(this.budgetammount) === 'string' ? new String(this.budgetammount).replace(',', '.') : this.budgetammount);
            let hourlyrate = new Decimal(typeof(this.hourlyrate) === 'string' ? new String(this.hourlyrate).replace(',', '.') : this.hourlyrate);
            
            return budgetammount.dividedBy(hourlyrate);
        }
        catch(err){
            console.log('Error while calculating remaininghours: ' + err);
        }

        return new Decimal(0);
    }

    updateField(fieldName: keyof CoachingProgramEdit, value: any) {
        (this[fieldName] as any) = value;
    }
}