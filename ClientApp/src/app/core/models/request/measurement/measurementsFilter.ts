import { isNullOrUndefined } from 'common/utils/common';
import { emptyYear } from 'common/utils/years';
import { emptyMonth } from 'static/months';

export interface MeasurementFilter {
    month?: number;
    year?: number;
    measurementTypeId?: number;
    isEmpty?: boolean;
}

export default class MeasurementsFilter implements MeasurementFilter {
    public month?: number;
    public year?: number;
    public measurementTypeId?: number;
    public isEmpty?: boolean;

    constructor() {
        this.isEmpty = true;
    }

    public setIsEmpty(): void {
        this.isEmpty =
            this.isMonthEmpty()
            && this.isEmptyYear()
            && isNullOrUndefined(this.measurementTypeId);
    }

    public clear(): void {
        this.month = undefined;
        this.year = undefined;
        this.measurementTypeId = undefined;
    }

    private isMonthEmpty(): boolean {
        return isNullOrUndefined(this.month) || this.month === emptyMonth.id;
    }

    private isEmptyYear(): boolean {
        return isNullOrUndefined(this.year) || this.year === emptyYear.id;
    }
}