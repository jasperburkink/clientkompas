export default interface WorkingContract {
    organizationid: number;
    function: string;
    contracttype: number;
    fromdate?: Date;
    todate?: Date;
}