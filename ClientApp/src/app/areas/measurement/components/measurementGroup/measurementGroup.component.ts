import { Component, EventEmitter, Input, Output } from '@angular/core';

import { ReplaySubject, Subject } from 'rxjs';

import { getMonthName } from 'static/months';

import BaseComponent from 'common/components/BaseComponent';

import { MeasurementsResponse, MeasurementsResponseMeasurement } from 'models/response/measurements';

@Component({
    selector: 'app-measurement-group',
    templateUrl: 'measurementGroup.template.pug',
    styleUrls: ['measurementGroup.style.styl']
})
export class MeasurementGroupComponent extends BaseComponent {
    @Input()
    public measurementGroup: MeasurementsResponse;

    @Input()
    public isSentFlagActive: boolean;

    @Input()
    public showAsGroups: boolean;

    @Output()
    public deleteClick: EventEmitter<number> =
        new EventEmitter();

    @Output()
    public editClick: EventEmitter<number> =
        new EventEmitter();

    @Output()
    public typeClick: EventEmitter<number> =
        new EventEmitter();

    @Output()
    public sendFlagClick: EventEmitter<{
        checked: boolean,
        id: number,
    }> =
        new EventEmitter();


    public onDeleteClick$: Subject<number> =
        new ReplaySubject();

    public onEditClick$: Subject<number> =
        new ReplaySubject();

    public onTypeClick$: Subject<number> =
        new ReplaySubject();

    public onSendFlagClick$: Subject<{
        checked: boolean,
        id: number,
    }> =
        new ReplaySubject();

    public measurements: Array<MeasurementsResponseMeasurement> = [];
    public isGroupCollapsed: boolean = false;
    public isDescSortOrder: boolean = false;

    public currentSortColumn: string;
    public formattedGroupName: string;

    private currentSortOrder: 'asc' | 'desc' =
        'asc';

    constructor(
    ) {
        super();

        this.onDeleteClick$.subscribe(event => this.deleteClick.emit(event));
        this.onEditClick$.subscribe(event => this.editClick.emit(event));
        this.onTypeClick$.subscribe(event => this.typeClick.emit(event));
        this.onSendFlagClick$.subscribe(event => this.sendFlagClick.emit(event));

        this.whenComponentInit$
            .subscribe(() => {
                const monthName: string =
                    getMonthName(+this.measurementGroup.month);

                this.formattedGroupName = `${monthName} ${this.measurementGroup.year}`;
                this.onSortColumn('type');
            });
    }

    public toggleState(): void {
        this.isGroupCollapsed = !this.isGroupCollapsed;
    }

    public onSortColumn(columnName: string): void {
        let sortingFunc: (left: MeasurementsResponseMeasurement, right: MeasurementsResponseMeasurement) => number;

        if (this.currentSortColumn !== columnName) {
            this.currentSortOrder = 'asc';
            this.currentSortColumn = columnName;
        }

        switch (columnName) {
            case 'value':
                if (this.currentSortOrder === 'asc') {
                    sortingFunc = (left, right) => left.measurement - right.measurement;
                } else {
                    sortingFunc = (left, right) => right.measurement - left.measurement;
                }
                break;
            case 'sent':
                if (this.currentSortOrder === 'asc') {
                    sortingFunc = (left, right) => (left.isSent ? 1 : 0) - (right.isSent ? 1 : 0);
                } else {
                    sortingFunc = (left, right) => (right.isSent ? 1 : 0) - (left.isSent ? 1 : 0);
                }
                break;
            default:
                if (this.currentSortOrder === 'asc') {
                    sortingFunc = (left, right) => left.meterMeasurementTypeId - right.meterMeasurementTypeId;
                } else {
                    sortingFunc = (left, right) => right.meterMeasurementTypeId - left.meterMeasurementTypeId;
                }
                break;
        }

        this.currentSortOrder =
            this.currentSortOrder === 'asc'
                ? 'desc'
                : 'asc';

        this.currentSortColumn = columnName;
        this.isDescSortOrder = this.currentSortOrder === 'desc';

        const sortedMeasurements: Array<MeasurementsResponseMeasurement> =
            this.measurementGroup.measurements.sort(sortingFunc);

        this.measurements = sortedMeasurements;
    }
}