const yearsRange = function (start: number, end: number): Array<{ id?: number, name: string }> {
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

export { yearsRange };