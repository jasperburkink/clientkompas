export default interface LoginCommandDto { 
    success: boolean;
    bearertoken?: string;
    refreshtoken?: string;
    userid?: string;
    twofactorpendingtoken: string;
    expiresat: string;
}