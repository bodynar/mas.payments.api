import { isNullOrUndefined } from 'common/utils/common';

export default class MeasurementsFilter {
    month?: number;
    year?: number;
    measurementTypeId?: number;
    isEmpty?: boolean;

    constructor() {
        this.isEmpty = true;
    }

    public setIsEmpty(): void {
        this.isEmpty =
            isNullOrUndefined(this.month)
            && isNullOrUndefined(this.year)
            && isNullOrUndefined(this.measurementTypeId);
    }

    public clear(): void {
        this.month = undefined;
        this.year = undefined;
        this.measurementTypeId = undefined;
    }
}