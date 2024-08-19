import Decimal from "decimal.js-light";

export default interface CoachingProgram {
    id: number;    
    clientfullname: string;
    title: string;
    ordernumber?: string;
    organizationname?: string;
    coachingprogramtype: string;
    begindate: Date,
    enddate: Date,
    budgetammount?: Decimal,
    hourlyrate: Decimal,
    remaininghours: Decimal
}