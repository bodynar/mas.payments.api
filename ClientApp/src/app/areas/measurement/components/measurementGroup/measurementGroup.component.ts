import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { ReplaySubject, Subject } from 'rxjs';

import { getMonthName } from 'src/static/months';

import MeasurementsResponse from 'models/response/measurements/measurementsResponse';

@Component({
    selector: 'app-measurement-group',
    templateUrl: 'measurementGroup.template.pug',
    styleUrls: ['measurementGroup.style.styl']
})
export default class MeasurementGroupComponent implements OnInit {
    @Input()
    public measurementGroup: MeasurementsResponse;

    @Input()
    public isSentFlagActive: Subject<boolean>;

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

    public formattedGroupName: string;

    constructor(
    ) {
        this.onDeleteClick$.subscribe(event => this.deleteClick.emit(event));
        this.onEditClick$.subscribe(event => this.editClick.emit(event));
        this.onTypeClick$.subscribe(event => this.typeClick.emit(event));
        this.onSendFlagClick$.subscribe(event => this.sendFlagClick.emit(event));
    }

    public ngOnInit(): void {
        const monthName: string =
            getMonthName(+this.measurementGroup.month);

        this.formattedGroupName = `${monthName} ${this.measurementGroup.year}`;
    }
}