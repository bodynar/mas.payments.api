import { Component, EventEmitter, HostListener, Input, Output } from '@angular/core';

import BaseComponent from '../BaseComponent';

import { emptyMonth, getMonthName, Month, months as monthArray } from 'static/months';
import { emptyYear, Year, yearsRange } from 'common/utils/years';
import { generateGuid, isNullOrUndefined } from 'common/utils/common';

const today: Date = new Date();

@Component({
    selector: 'app-month-selector',
    templateUrl: 'monthSelector.template.pug',
    styleUrls: ['./monthSelector.style.styl']
})
export class MonthSelectorComponent extends BaseComponent {
    @Input()
    public todayBtnCaption: string = 'Today';

    @Input()
    public clearBtnCaption: string = 'Clear';

    @Input()
    public canClear: boolean = true;

    @Input()
    public startYear?: number = 1900;

    @Input()
    public endYear?: number = 2999;

    @Input()
    public year?: number;

    @Output()
    public yearChange: EventEmitter<number> =
        new EventEmitter(true);

    @Input()
    public month?: number;

    @Output()
    public monthChange: EventEmitter<number> =
        new EventEmitter(true);

    public months: Array<Month>;
    public years: Array<Year>;

    public isDropPanelVisible: boolean =
        false;

    public componentUid: string =
        generateGuid();

    constructor(
    ) {
        super();

        this.whenComponentInit$.subscribe(() => {
            this.months = this.canClear ? [emptyMonth, ...monthArray] : monthArray;
            this.years = this.canClear ? [emptyYear, ...yearsRange(this.startYear, this.endYear)] : yearsRange(this.startYear, this.endYear);

            if (isNullOrUndefined(this.month)) {
                this.month = this.canClear ? emptyMonth.id : today.getMonth();
                this.monthChange.emit(this.month);
            } else if (this.month === emptyMonth.id && !this.canClear) {
                this.month = today.getMonth();
                this.monthChange.emit(this.month);
            }
            if (isNullOrUndefined(this.year)) {
                this.year = this.canClear ? emptyYear.id : today.getFullYear();
                this.yearChange.emit(this.year);
            } else if (this.year === emptyYear.id && !this.canClear) {
                this.year = today.getFullYear();
                this.yearChange.emit(this.year);
            }
        });
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

    public getTitle(): string {
        return this.isEmpty() ? 'Not selected' : `${getMonthName(+this.month)} ${this.year}`;
    }

    public canClickArrow(direction: 'left' | 'right'): boolean {
        if (this.isEmpty()) {
            return false;
        }

        if (direction === 'left') {
            return +this.year !== +this.startYear || +this.month !== 0;
        } else {
            return +this.year !== +this.endYear || +this.month !== 11;
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
            this.year;

        let month: number =
            isDecrementAction
                ? this.month - 1
                : this.month + 1;

        if (month < 0 || month >= 12) {
            month = month % 12;

            if (month < 0) {
                year -= 1;
                month = 11;
            } else if (month === 0) {
                year += 1;
            }
        }

        this.setMonthValueAndNotify(year, month);
    }

    public onTodayBtnClick(): void {
        this.setMonthValueAndNotify(today.getFullYear(), today.getMonth());

        this.isDropPanelVisible = false;
    }

    public getControlId(controlName?: string): string {
        controlName = controlName || 'root';
        return `${controlName}-${this.componentUid}`;
    }

    public onClearBtnClick(): void {
        if (this.canClear) {
            this.setMonthValueAndNotify(emptyYear.id, emptyMonth.id);

            this.isDropPanelVisible = false;
        }
    }

    public isEmpty(): boolean {
        return +this.year === emptyYear.id || +this.month === emptyMonth.id;
    }

    private isMonthSelectDropPanelItem(element: HTMLElement): boolean {
        let result: boolean =
            !isNullOrUndefined(element)
            && element.classList.contains('month-selector')
            && element.id === this.getControlId();

        if (!result
            && !isNullOrUndefined(element.parentElement)
            && element.tagName !== 'HTML'
            && element.tagName !== 'BODY'
        ) {
            result = this.isMonthSelectDropPanelItem(element.parentElement);
        }

        return result;
    }

    private setMonthValueAndNotify(year: number, month: number): void {
        this.year = year;
        this.month = month;

        this.yearChange.emit(this.year);
        this.monthChange.emit(this.month);
    }
}