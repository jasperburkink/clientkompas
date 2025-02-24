export default interface CreateUserCommandDto { 
    id: number;
    firstname: string;
    prefixlastname?: string;
    lastname: string;
    emailaddress: string;
    telephonenumber: string;
    rolename: string;
}