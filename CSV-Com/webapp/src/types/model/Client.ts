import EmergencyPerson from "./EmergencyPerson";
import WorkingContract from "./WorkingContract";

export default interface Client {
    identificationnumber: number;
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
    driverslicences: string;
    diagnoses?: string;
    benefitform?: string;
    remarks?: string;
    emergencypeople?: EmergencyPerson[];
    workingcontracts?: WorkingContract[];
}

export function getCompleteClientName(client: Client): string {
    return `${client.firstname}${client.prefixlastname ? ` ${client.prefixlastname}` : ''} ${client.lastname}`;
};