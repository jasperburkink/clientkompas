import EmergencyPerson from "./EmergencyPerson";
import WorkingContractQuery from "./WorkingContractQuery";

export default interface ClientQuery {
    id: number;
    firstname: string;
    initials: string;
    prefixlastname?: string;
    lastname: string;
    gender: string;
    streetname: string;
    housenumber: string;
    housenumberaddition?: string;
    postalcode: string;
    residence: string;
    telephonenumber: string;
    dateofbirth: Date;
    emailaddress: string;
    maritalstatus: string;
    isintargetgroupregister: boolean;
    driverslicences: string;
    diagnoses?: string;
    benefitform?: string;
    remarks?: string;
    emergencypeople?: EmergencyPerson[];
    workingcontracts?: WorkingContractQuery[];
}

export function getCompleteClientName(client: ClientQuery): string {
    return `${client.prefixlastname ? `${client.prefixlastname} ` : ''}${client.lastname}, ${client.firstname}`;
};