import { Component, Input, OnInit } from '@angular/core';

import { Subject } from 'rxjs';

import { getMonthName } from 'src/static/months';

import { MeasurementsResponseMeasurement } from 'models/response/measurements/measurementsResponse';

@Component({
    selector: 'app-measurement-item',
    templateUrl: 'measurement.template.pug',
    styleUrls: ['measurement.style.styl']
})
class MeasurementComponent implements OnInit {
    @Input()
    public measurement: MeasurementsResponseMeasurement;

    @Input()
    public isGroupedItem: boolean;

    @Input()
    public isSentFlagActive: Subject<boolean>;

    @Input()
    public onDeleteClick: Subject<number>;

    @Input()
    public onEditClick: Subject<number>;

    @Input()
    public onTypeClick: Subject<number>;

    @Input()
    public onSendFlagClick: Subject<{
        checked: boolean,
        id: number,
    }>;

    public isStateChaged: boolean =
        false;

    public formattedDate: string;

    constructor(
    ) {
    }

    public ngOnInit(): void {
        this.isSentFlagActive.subscribe();

        this.formattedDate = `${getMonthName(this.measurement.month)} ${this.measurement.year}`;
    }

    public onChecked({ target }: Event): void {
        const checked: boolean =
            target instanceof HTMLInputElement
            && target.checked;

        this.isStateChaged = true;

        this.onSendFlagClick.next({
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