import { BearerToken } from "types/common/bearer-token";
import RefreshTokenCommandDto from "types/model/refresh-token/refresh-token-command-dto";
import AccessTokenService from "./access-token-service";

export default class RefreshTokenService {
    private static instance: RefreshTokenService | null = null;
    private isRefreshingToken: boolean = false;
    private refreshSubscribers: ((token: RefreshTokenCommandDto) => void)[] = [];
    private apiUrl = process.env.REACT_APP_API_URL; // Voeg de API-URL hier toe

    private readonly TokenName = 'refreshToken';

    private constructor() {}

    public static getInstance(): RefreshTokenService {
        if (!RefreshTokenService.instance) {
            RefreshTokenService.instance = new RefreshTokenService();
        }
        return RefreshTokenService.instance;
    }

    private onTokenRefreshed(token: RefreshTokenCommandDto) {
        this.refreshSubscribers.forEach(callback => callback(token));
        this.refreshSubscribers = [];
    }

    private addRefreshSubscriber(callback: (token: RefreshTokenCommandDto) => void) {
        this.refreshSubscribers.push(callback);
    }

    public async refreshAccessToken(): Promise<RefreshTokenCommandDto | null> {
        if (this.isRefreshingToken) {
            return new Promise((resolve) => {
                this.addRefreshSubscriber(resolve);
            });
        }

        this.isRefreshingToken = true;

        try {
            const refreshToken = this.getRefreshToken();
            if (!refreshToken) {
                window.location.href = '/unauthorized';
                return null;
            }

            const response = await fetch(`${this.apiUrl}Authentication/RefreshToken`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ "RefreshToken": refreshToken })
            });

            const data: RefreshTokenCommandDto = await response.json();

            if (response.ok && data.bearertoken && data.refreshtoken) {
                AccessTokenService.getInstance().setAccessToken(new BearerToken(data.bearertoken));
                this.setRefreshToken(data.refreshtoken);

                this.isRefreshingToken = false;
                this.onTokenRefreshed(data);

                return data;
            } else {
                window.location.href = '/unauthorized';
                return null;
            }

        } catch (err) {
            console.error('Error refreshing token:', err);
            throw err;
        } finally {
            this.isRefreshingToken = false;
        }
    }

    // Storage methods
    public setRefreshToken(token: string): void {
        localStorage.setItem(this.TokenName, token);
    }

    public getRefreshToken(): string | null {
        return localStorage.getItem(this.TokenName) || null;
    }

    public removeRefreshToken(): void {
        localStorage.removeItem(this.TokenName);
        AccessTokenService.getInstance().removeAccessToken();
    }
}