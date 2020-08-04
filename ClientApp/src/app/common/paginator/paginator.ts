import { isNullOrUndefined } from 'util';

import PaginatorConfig from './paginatorConfig';

export function getPaginatorConfig(items: Array<any>, pageSize: number): PaginatorConfig {
    let config: PaginatorConfig = {
        enabled: false,
        pageSize: pageSize,
        pagesCount: 0,
    };

    if (!isNullOrUndefined(items) && items.length > 0) {
        const pagesCount: number = Math.ceil(items.length / pageSize);

        if (pagesCount > 3) {
            config = {
                enabled: true,
                pageSize: pageSize,
                pagesCount: pagesCount
            };
        }
    }

    return config;
}

export function generatePageNumbers(currentPage: number, pagesCount: number): Array<number> {
    const result = [];

    if (pagesCount > 6) {
        const leftFromPointer: Array<number> =
            [];

        for (let i = currentPage - 2; i < currentPage; i++) {
            if (i > 0) {
                leftFromPointer.push(i);
            }
        }
        const rightFromPointerAmount: number = leftFromPointer.length === 0 ? 4 : 3;
        const rightFromPointer: Array<number> =
            [];

        for (let i = currentPage; i <= currentPage + rightFromPointerAmount && i <= pagesCount; i++) {
            if (i > 0) {
                rightFromPointer.push(i);
            }
        }

        return [...leftFromPointer, ...rightFromPointer];
    }
    else {
        for (let i = 1; i <= pagesCount; i++) {
            result.push(i);
        }
    }

    return result;
}