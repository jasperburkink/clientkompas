export default interface RefreshTokenCommandDto {
    success: boolean;
    bearertoken?: string;
    refreshtoken?: string;
}