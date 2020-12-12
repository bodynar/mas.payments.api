import { Component, EventEmitter, Input, Output } from '@angular/core';

import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';
import { generatePageNumbers } from '../paginator';

import BaseComponent from 'common/components/BaseComponent';

@Component({
    selector: 'app-paginator',
    templateUrl: 'paginator.template.pug'
})
export class PaginatorComponent extends BaseComponent {

    @Input()
    public pagesCount: number;

    @Input()
    public startPage?: number;

    @Output()
    public pageChange: EventEmitter<number> =
        new EventEmitter();

    public pageNumbers$: Subject<Array<number>> =
        new ReplaySubject(1);

    public canGoBack$: Subject<boolean> =
        new BehaviorSubject(false);

    public canGoForward$: Subject<boolean> =
        new BehaviorSubject(false);

    public currentPage$: Subject<number> =
        new ReplaySubject(1);

    private currentPage: number =
        0;

    constructor(
    ) {
        super();

        this.whenComponentInit$
            .subscribe(() => {
                const currentPage: number =
                    this.startPage || 0;

                if (this.pagesCount === 0) {
                    throw new Error('[Paginator]: Pages count must be greater than 0.');
                }
                if (currentPage > this.pagesCount) {
                    throw new Error('[Paginator]: Current page cannot be greated than pages count.');
                }

                this.updateValues(currentPage);
            });
    }

    public onArrowClick(isForward: boolean): void {
        const pageNumber: number =
            this.currentPage + (isForward ? 1 : -1);

        this.updateValues(pageNumber);

        this.pageChange.emit(pageNumber);
    }

    public onPageClick(pageNumber: number): void {
        if (--pageNumber > this.pagesCount || pageNumber < 0) {
            throw new Error(`[Paginator]: Cannot navigate to ${pageNumber} page.`);
        }

        this.updateValues(pageNumber);

        this.pageChange.emit(pageNumber);
    }

    private updateValues(currentPage: number): void {
        this.canGoBack$.next(currentPage > 0);
        this.canGoForward$.next(currentPage < this.pagesCount - 1);
        this.currentPage$.next(currentPage);

        const pageNumbers: Array<number> =
            generatePageNumbers(currentPage, this.pagesCount);

        this.pageNumbers$.next(pageNumbers);

        this.currentPage = currentPage;
    }
}