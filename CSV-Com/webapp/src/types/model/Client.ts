import BenefitForm from "./BenefitForm";
import Diagnosis from "./Diagnosis";
import DriversLicence from "./DriversLicence";
import EmergencyPerson from "./EmergencyPerson";
import WorkingContract from "./WorkingContract";

export default interface Client {
    id: number;
    firstname: string;
    initials: string;
    prefixlastname?: string;
    lastname: string;
    gender: number;
    streetname: string;
    housenumber: string;
    housenumberaddition?: string;
    postalcode: string;
    residence: string;
    telephonenumber: string;
    dateofbirth?: Date;
    emailaddress: string;
    maritalstatus?: number;
    driverslicences: DriversLicence[];
    doelgroepregister: boolean;
    diagnoses?: Diagnosis[];
    benefitforms?: BenefitForm[];
    remarks?: string;
    emergencypeople?: EmergencyPerson[];
    workingcontracts?: WorkingContract[];
}