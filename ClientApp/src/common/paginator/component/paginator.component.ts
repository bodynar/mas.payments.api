import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';
import { generatePageNumbers } from '../paginator';

@Component({
    selector: 'app-paginator',
    templateUrl: 'paginator.template.pug'
})
export class PaginatorComponent implements OnInit {

    @Input('pagesCount')
    public pagesCount: number;

    @Input('startPage')
    public startPage?: number;

    @Output('pageChange')
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
    }

    public ngOnInit(): void {
        const currentPage: number =
            this.startPage || 0;

        if (this.pagesCount === 0) {
            throw new Error('[Paginator]: Pages count must be greater than 0.');
        }
        if (currentPage > this.pagesCount) {
            throw new Error('[Paginator]: Current page cannot be greated than pages count.');
        }

        const pageNumbers: Array<number> =
            generatePageNumbers(currentPage, this.pagesCount);

        this.pageNumbers$.next(pageNumbers);

        this.updateValues(currentPage);
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

        this.currentPage = currentPage;
    }
}