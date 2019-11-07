class AuthenticateRequest {
    login: string;
    password?: string;
    passwordHash: string;
}

export { AuthenticateRequest };