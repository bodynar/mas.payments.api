import { isNullOrEmpty } from 'common/utils/common';

interface Month {
    id: number;
    name: string;
}

const months: Array<Month> = [
    {
        id: 0,
        name: 'January'
    },
    {
        id: 1,
        name: 'February'
    },
    {
        id: 2,
        name: 'March'
    },
    {
        id: 3,
        name: 'April'
    },
    {
        id: 4,
        name: 'May'
    },
    {
        id: 5,
        name: 'June'
    },
    {
        id: 6,
        name: 'July'
    },
    {
        id: 7,
        name: 'August'
    },
    {
        id: 8,
        name: 'September'
    },
    {
        id: 9,
        name: 'October'
    },
    {
        id: 10,
        name: 'November'
    },
    {
        id: 11,
        name: 'December'
    },
];

const emptyMonth: Month = { name: '', id: -1 };

const getMonthName = (monthNumber: number): string => {
    if (monthNumber < 0 || monthNumber > 12) {
        throw new Error('Month number must be in (0, 12] range.');
    }

    const month: Month | undefined = months.find(x => x.id === monthNumber);

    return month?.name;
};

const getShortMonthName = (monthNumber: number): string => {
    const monthName: string = getMonthName(monthNumber);

    return isNullOrEmpty(monthName) ? monthName : monthName.substr(0, 3);
}

export {
    Month,
    months,
    emptyMonth,
    getMonthName,
    getShortMonthName
};