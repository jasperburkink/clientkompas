export default interface SearchUserQueryDto { 
    id: number;
    firstname: string;
    prefixlastname?: string;
    lastname: string;
    fullname: string;
    deactivateddatetime?: boolean;
}