export default interface ResetPasswordCommand {
    emailaddress: string;
    token: string;
    newpassword: string;
    newpasswordrepeat: string;
}