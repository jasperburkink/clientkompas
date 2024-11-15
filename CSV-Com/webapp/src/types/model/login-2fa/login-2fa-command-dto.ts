export default interface TwoFactorAuthenticationCommandDto { 
    success: boolean;
    bearertoken?: string;
    refreshtoken?: string;
}