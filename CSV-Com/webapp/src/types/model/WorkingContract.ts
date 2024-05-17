export default interface WorkingContract {
    organizationname: string;
    function: string;
    contracttype: number;
    fromdate?: Date;
    todate?: Date;
}