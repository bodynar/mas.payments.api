import { Component, EventEmitter, Input, Output } from '@angular/core';

import { MeasurementResponse } from 'models/response/measurementResponse';

import { getMonthName } from 'src/static/months';

@Component({
    selector: 'app-measurement-item',
    templateUrl: 'measurement.template.pug',
    styleUrls: ['measurement.style.styl']
})
class MeasurementComponent {
    @Input()
    public measurement: MeasurementResponse;

    @Output()
    public deleteClick: EventEmitter<number> =
        new EventEmitter();

    @Output()
    public editClick: EventEmitter<number> =
        new EventEmitter();

    @Output()
    public typeClick: EventEmitter<number> =
        new EventEmitter();

    constructor(
    ) { }

    public formatMonth(monthNumber: number): string {
      return getMonthName(monthNumber);
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