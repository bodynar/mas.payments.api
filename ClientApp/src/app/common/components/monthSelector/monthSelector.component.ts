import { Component, EventEmitter, HostListener, Input, Output } from '@angular/core';

import BaseComponent from '../BaseComponent';

import { getMonthName, months as monthArray } from 'static/months';
import { yearsRange } from 'common/utils/years';
import { isNullOrUndefined } from 'common/utils/common';

@Component({
    selector: 'app-month-selector',
    templateUrl: 'monthSelector.template.pug',
    styleUrls: ['./monthSelector.style.styl']
})
export class MonthSelectorComponent extends BaseComponent {

    @Input()
    public todayBtnCaption: string = 'Today';

    @Input()
    public startYear: number = 1900;

    @Input()
    public endYear: number = 2999;

    private today: Date = new Date();

    public months: Array<{ id: number, name: string }> =
        monthArray;

    public years: Array<{ id: number, name: number }> =
        yearsRange(this.startYear, this.endYear);

    @Output()
    public selectionChange: EventEmitter<{ month: number, year: number }> =
        new EventEmitter();

    public isDropPanelVisible: boolean =
        false;

    public selectedMonth: number =
        this.today.getMonth();

    public selectedYear: number =
        this.today.getFullYear();

    constructor(
    ) {
        super();
    }

    public onCheckBoxChecked(target: HTMLInputElement): void {
        if (!isNullOrUndefined(target)) {
            this.isDropPanelVisible = target.checked;
        }
    }

    @HostListener('document:click', ['$event'])
    public clickOutside(event: MouseEvent): void {
        if (this.isDropPanelVisible
            && !isNullOrUndefined(event)
            && !isNullOrUndefined(event.target)
            && event.target instanceof HTMLElement
            && !this.isMonthSelectDropPanelItem(event.target)
        ) {
            this.isDropPanelVisible = false;
        }
    }

    public onMonthChange(monthId: string): void {
        const month: number = parseInt(monthId, 10);
        const selectedMonth: { id: number, name: string } =
            this.months.find(x => x.id === month);

        if (!isNullOrUndefined(selectedMonth)) {
            this.selectedMonth = selectedMonth.id;
        }
    }

    public getCurrentSelection(): string {
        return `${getMonthName(this.selectedMonth)} ${this.selectedYear}`;
    }

    public canClickArrow(direction: 'left' | 'right'): boolean {
        if (direction === 'left') {
            return this.selectedYear !== this.startYear || this.selectedMonth !== 0;
        } else {
            return this.selectedYear !== this.endYear || this.selectedMonth !== 11;
        }
    }

    public onArrowClick(direction: 'left' | 'right'): void {
        const canClick: boolean =
            this.canClickArrow(direction);

        if (!canClick) {
            return;
        }

        const isDecrementAction: boolean =
            direction === 'left';

        let year: number =
            this.selectedYear;

        let month: number =
            isDecrementAction
                ? this.selectedMonth - 1
                : this.selectedMonth + 1;

        if (month < 0 || month >= 12) {
            month = month % 12;

            if (month < 0) {
                year -= 1;
                month = 11;
            } else if (month === 0) {
                year += 1;
            }
        }

        this.selectedYear = year;
        this.selectedMonth = month;

        this.selectionChange.emit({ month, year });
    }

    public onYearChange(yearId: string): void {
        const year: number = parseInt(yearId, 10);
        const selectedYear: { id: number, name: number } =
            this.years.find(x => x.id === year);

        if (!isNullOrUndefined(selectedYear)) {
            this.selectedYear = selectedYear.id;
        }
    }

    public onTodayBtnClick(): void {
        this.selectedYear = this.today.getFullYear();
        this.selectedMonth = this.today.getMonth();
    }

    private isMonthSelectDropPanelItem(element: HTMLElement): boolean {
        let result: boolean =
            !isNullOrUndefined(element)
            && element.classList.contains('month-selector');

        if (!result
            && !isNullOrUndefined(element.parentElement)
            && element.tagName !== 'HTML'
            && element.tagName !== 'BODY'
        ) {
            result = this.isMonthSelectDropPanelItem(element.parentElement);
        }

        return result;
    }
}