export class RefreshToken {
    private token: string;

    constructor(token: string) {
        this.token = token;
    }

    getToken(): string {
        return this.token;
    }

    static serialize(refreshToken: RefreshToken): string {
        return JSON.stringify({ token: refreshToken.getToken() });
    }

    static deserialize(data: string): RefreshToken {
        const parsed = JSON.parse(data);
        return new RefreshToken(parsed.token);
    }
}