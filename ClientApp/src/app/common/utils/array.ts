import { isNullOrUndefined } from './common';

export const ArrayUtils = {
    startsWith<T>(array: Array<T>, part: Array<T>): boolean {
        if (isNullOrUndefined(array) || isNullOrUndefined(part)
            || array.length === 0 || part.length === 0 || array.length < part.length) {
            return false;
        }

        let partItem: T;
        let arrayItem: T;

        for (let index = 0; index < part.length; index++) {
            arrayItem = array[index];
            partItem = part[index];

            if (arrayItem !== partItem) {
                return false;
            }
        }

        return true;
    }
};