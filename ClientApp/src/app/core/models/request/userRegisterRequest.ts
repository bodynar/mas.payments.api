class UserRegisterRequest {
    login: string;
    password?: string;
    passwordHash: string;
    email: string;
    firstName: string;
    lastName: string;
}

export { UserRegisterRequest };