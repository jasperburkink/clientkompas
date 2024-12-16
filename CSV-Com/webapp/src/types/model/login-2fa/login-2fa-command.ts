export default interface TwoFactorAuthenticationCommand { 
    userid: string;
    token: string;
    twofactorpendingtoken: string;
}