import { Injectable } from '@angular/core';

import { Md5 } from 'ts-md5/dist/md5';

import { IHasherService } from 'services/IHasherService';

@Injectable()
class HasherService implements IHasherService {
    constructor(
    ) {
    }

    public generateHash(value: string): string {
        return Md5.hashStr(value) as string;
    }

}

export { HasherService };