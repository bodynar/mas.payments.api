const months: Array<{ id?: number, name: string }> = [
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

const getMonthName = (monthNumber: number): string => {
    if (monthNumber <= 0 || monthNumber > 12) {
        throw new Error('Month number must be in (0, 12] range.');
    }

    return months[monthNumber - 1].name;
};

export { months, getMonthName };