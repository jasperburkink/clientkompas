export default interface WorkingContract {
    companyname: string;
    function: string;
    contracttype: number;
    fromdate?: Date;
    todate?: Date;
}