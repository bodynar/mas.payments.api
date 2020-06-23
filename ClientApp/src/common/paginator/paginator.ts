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

    for (let i = 0; i <= pagesCount; i++) {
        if (i > 0) {
            result.push(i);
        }
    }

    return result;
}