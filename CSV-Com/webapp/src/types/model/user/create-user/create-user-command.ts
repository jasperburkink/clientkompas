export default interface CreateUserCommand { 
    firstname: string;
    prefixlastname?: string;
    lastname: string;
    emailaddress: string;
    telephonenumber: string;
    rolename: string;
}