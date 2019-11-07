abstract class IHasherService {
    abstract generateHash(value: string): string;
}

export { IHasherService };