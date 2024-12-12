export default interface ResendTwoFactorAuthenticationTokenCommandDto { 
    success: boolean;
    userid: string;
    twofactorpendingtoken: string;
    expiresat: string;
}