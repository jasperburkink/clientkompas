export default interface SearchUserQueryDto { 
    id: number;
    firstname: string;
    prefixlastname?: string;
    lastname: string;
    fullname: string;
    isdeactivated: boolean;
}