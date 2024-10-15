export default class RefreshTokenService {
    private static instance: RefreshTokenService | null = null;
    private isRefreshingToken: boolean = false;
    private refreshSubscribers: ((token: string) => void)[] = [];
    private apiUrl = process.env.REACT_APP_API_URL; // Voeg de API-URL hier toe

    private constructor() {}

    public static getInstance(): RefreshTokenService {
        if (!RefreshTokenService.instance) {
            RefreshTokenService.instance = new RefreshTokenService();
        }
        return RefreshTokenService.instance;
    }

    private onTokenRefreshed(token: string) {
        this.refreshSubscribers.forEach(callback => callback(token));
        this.refreshSubscribers = [];
    }

    private addRefreshSubscriber(callback: (token: string) => void) {
        this.refreshSubscribers.push(callback);
    }

    public async refreshAccessToken(): Promise<string | null> {
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

            const data = await response.json();

            if (response.ok && data.accessToken && data.refreshToken) {
                this.setAccessToken(data.accessToken);
                this.setRefreshToken(data.refreshToken);

                this.isRefreshingToken = false;
                this.onTokenRefreshed(data.accessToken);

                return data.accessToken;
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
        localStorage.setItem('refreshToken', token);
    }

    public getRefreshToken(): string | null {
        return localStorage.getItem('refreshToken') || null;
    }

    private setAccessToken(token: string): void {
        sessionStorage.setItem('token', token);
    }
}