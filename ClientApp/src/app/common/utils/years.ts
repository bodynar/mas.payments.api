const currentYear: number = new Date().getFullYear();

interface Year {
    id: number;
    name: number;
}

const emptyYear: Year = { id: 0, name: undefined };

const yearsRange = (start?: number, end?: number): Array<Year> => {
    start = start || currentYear - 20;
    end = end || currentYear + 20;

    if (start < 0 || end < 0) {
        throw new Error('Year range must be positive.');
    }

    if (start > end) {
        throw new Error('End year must be lesser than start year.');
    }

    const tempArray = [];

    for (let i = start; i <= end; i++) {
        tempArray.push(i);
    }

    return tempArray.map((item, index) => ({
        id: item,
        name: item
    }));
};

export {
    yearsRange,
    emptyYear,
    Year
};