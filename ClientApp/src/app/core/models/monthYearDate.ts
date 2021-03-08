import { isNullOrUndefined } from 'common/utils/common';

export default class MonthYear {
    public month: number = -1;
    public year: number = -1;

    public static fromToday(): MonthYear {
        const today: Date = new Date();

        return new MonthYear(today.getMonth(), today.getFullYear());
    }

    constructor(month: number = -1, year: number = -1) {
        this.month = month;
        this.year = year;
    }

    public set(value: MonthYear): void {
        if (!isNullOrUndefined(value)) {
            this.month = value.month;
            this.year = value.year;
        }
    }

    public toDate(): Date {
        return new Date(this.year, this.month, 1);
    }
}