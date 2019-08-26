import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { MeasurementResponse } from 'models/response/measurementResponse';

import { months } from 'src/static/months';

@Component({
    selector: 'app-measurement-item',
    templateUrl: 'measurement.template.pug',
    styleUrls: ['measurement.style.styl']
})
class MeasurementComponent implements OnInit {
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

    public date: Date;

    constructor(
    ) { }

    public ngOnInit(): void {
        this.date = new Date(this.measurement.date);
    }

    public formatDate(rawDate: string): string {
        const month: number =
            new Date(rawDate).getMonth();

        return `${months[month].name}`;
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