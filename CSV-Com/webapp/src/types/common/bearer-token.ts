import { jwtDecode } from 'jwt-decode';

interface JwtPayload {
    exp: number;
    sub: string;
    name: string;
    email: string;
    CVSUserId: string;
}

export class BearerToken {
    private token: string;
    private payload: JwtPayload;

    constructor(token: string) {
        this.token = token;
        this.payload = jwtDecode<JwtPayload>(token);
    }

    isExpired(): boolean {
        const currentTime = Date.now() / 1000;
        return this.payload.exp < currentTime;
    }

    getToken(): string {
        return this.token;
    }

    getUserId(): string {
        return this.payload.sub;
    }

    getUserName(): string {
        return this.payload.name;
    }

    getUserEmail(): string {
        return this.payload.email;
    }

    getCVSUserId(): string {
        return this.payload.CVSUserId;
    }

    static serialize(bearerToken: BearerToken): string {
        return JSON.stringify({ token: bearerToken.getToken() });
    }

    static deserialize(data: string): BearerToken {
        const parsed = JSON.parse(data);
        return new BearerToken(parsed.token);
    }
}