export default interface ResendTwoFactorAuthenticationTokenCommand { 
    userid: string;
    twofactorpendingtoken: string;
}