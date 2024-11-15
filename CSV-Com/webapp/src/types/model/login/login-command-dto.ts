export default interface LoginCommandDto { 
    success: boolean;
    bearertoken?: string;
    refreshtoken?: string;
    userid?: string;
    twofactorauthenticationenabled: boolean;
}