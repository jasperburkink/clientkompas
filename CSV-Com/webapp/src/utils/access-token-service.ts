import { BearerToken } from "types/common/bearer-token";

export default class AccessTokenService {
    private static instance: AccessTokenService | null = null;

    private readonly TokenName = 'token';
    private readonly TwoFactorPendingTokenName = 'twofactorpendingtoken';

    private constructor() {}

    public static getInstance(): AccessTokenService {
        if (!AccessTokenService.instance) {
            AccessTokenService.instance = new AccessTokenService();
        }
        return AccessTokenService.instance;
    }    

    // Storage methods
    public getAccessToken(): BearerToken | null {
        return sessionStorage.getItem(this.TokenName) ? BearerToken.deserialize(sessionStorage.getItem(this.TokenName)!) : null;
    }

    public setAccessToken(token: BearerToken): void {
        sessionStorage.setItem(this.TokenName, BearerToken.serialize(token));
    }

    public removeAccessToken(): void {
        sessionStorage.removeItem(this.TokenName);
    }

    public getTwoFactorPendingToken(): string | null {
        return sessionStorage.getItem(this.TwoFactorPendingTokenName) || null;
    }

    public setTwoFactorPendingToken(token: string): void {
        sessionStorage.setItem(this.TwoFactorPendingTokenName, token);
    }

    public removeTwoFactorPendingToken(): void {
        sessionStorage.removeItem(this.TwoFactorPendingTokenName);
    }

    public getUserId(): string {
        let accessToken = this.getAccessToken();

        if(!accessToken){
            return '';
        }

        return accessToken.getUserId();
    }
}