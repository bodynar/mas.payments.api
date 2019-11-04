abstract class IAuthService {
    abstract setAuthToken(token: string): void;

    abstract getAuthToken(): string;
}

export { IAuthService };