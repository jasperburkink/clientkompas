import { Moment } from "moment";

export default interface GetUserDto {
    firstname: string;
    prefixlastName?: string;
    lastname: string;
    fullname: string;
    emailaddress: string;
    role: string; 
    telephonenumber: string;
    deactivationdatetime?: Moment;
    createdbyuserdescription? :string;
}