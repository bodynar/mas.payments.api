import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { Subject } from 'rxjs';

import MeasurementResponse from 'models/response/measurements/measurementResponse';

import { getMonthName } from 'src/static/months';

@Component({
    selector: 'app-measurement-item',
    templateUrl: 'measurement.template.pug',
    styleUrls: ['measurement.style.styl']
})
class MeasurementComponent implements OnInit {
    @Input()
    public measurement: MeasurementResponse;

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

    constructor(
    ) {
    }

    public ngOnInit(): void {
        this.isSentFlagActive.subscribe();
    }

    public formatMonth(monthNumber: number): string {
        return getMonthName(monthNumber);
    }

    public onChecked({ target }: Event): void {
        const checked: boolean =
            target instanceof HTMLInputElement
            && target.checked;

        this.sendFlagClick.emit({
            checked: checked,
            id: this.measurement.id,
        });
    }

    public getMeasurementTypeClass(paymentTypeName: string): string {
        // todo: remove method and update model

        switch (paymentTypeName.toLowerCase()) {
            case 'холодная вода':
                return 'cold-water';
            case 'горячая вода':
                return 'hot-water';
            case 'электричество (день)':
                return 'electricity-day';
            case 'электричество (ночь)':
                return 'electricity-night';
            default:
                return '';
        }
    }
}

export { MeasurementComponent };