import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';

import { getMonthName } from 'src/static/months';

import MeasurementsResponse, { MeasurementsResponseMeasurement } from 'models/response/measurements/measurementsResponse';

@Component({
    selector: 'app-measurement-group',
    templateUrl: 'measurementGroup.template.pug',
    styleUrls: ['measurementGroup.style.styl']
})
export class MeasurementGroupComponent implements OnInit {
    @Input()
    public measurementGroup: MeasurementsResponse;

    @Input()
    public isSentFlagActive$: Subject<boolean>;

    @Input()
    public showAsGroups: Subject<boolean>;

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

    public measurements$: Subject<Array<MeasurementsResponseMeasurement>> =
        new ReplaySubject();

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

    public isGroupCollapsed$: Subject<boolean> =
        new BehaviorSubject(false);

    public isDescSortOrder$: Subject<boolean> =
        new BehaviorSubject(false);

    public currentSortColumn$: Subject<string> =
        new ReplaySubject();

    public formattedGroupName: string;

    private collapsed: boolean =
        false;

    private currentSortOrder: 'asc' | 'desc' =
        'asc';

    private currentSortColumn: string;

    constructor(
    ) {
        this.onDeleteClick$.subscribe(event => this.deleteClick.emit(event));
        this.onEditClick$.subscribe(event => this.editClick.emit(event));
        this.onTypeClick$.subscribe(event => this.typeClick.emit(event));
        this.onSendFlagClick$.subscribe(event => this.sendFlagClick.emit(event));
    }

    public ngOnInit(): void {
        this.showAsGroups.subscribe();

        const monthName: string =
            getMonthName(+this.measurementGroup.month);

        this.formattedGroupName = `${monthName} ${this.measurementGroup.year}`;
        this.onSortColumn('type');
    }

    public toggleState(): void {
        this.collapsed = !this.collapsed;
        this.isGroupCollapsed$.next(this.collapsed);
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

        this.currentSortColumn$.next(columnName);
        this.isDescSortOrder$.next(this.currentSortOrder === 'desc');

        const sortedMeasurements: Array<MeasurementsResponseMeasurement> =
            this.measurementGroup.measurements.sort(sortingFunc);

        this.measurements$.next(sortedMeasurements);
    }
}