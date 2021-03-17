import { Component, Input } from '@angular/core';

import { Subject } from 'rxjs';

import { getMonthName } from 'static/months';

import BaseComponent from 'common/components/BaseComponent';

import { MeasurementsResponseMeasurement } from 'models/response/measurements';

@Component({
    selector: 'app-measurement-item',
    templateUrl: 'measurement.template.pug',
    styleUrls: ['measurement.style.styl']
})
export class MeasurementComponent extends BaseComponent {
    @Input()
    public measurement: MeasurementsResponseMeasurement;

    @Input()
    public isGroupedItem: boolean;

    @Input()
    public isSentFlagActive: boolean;

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
        super();

        this.whenComponentInit$
            .subscribe(() => {
                this.formattedDate = `${getMonthName(this.measurement.month)} ${this.measurement.year}`;
            });
    }

    public onChecked({ target }: Event): void {
        const checked: boolean =
            target instanceof HTMLInputElement
            && target.checked;

        this.isStateChaged = true;

        this.onSendFlagClick.next({
            checked,
            id: this.measurement.id,
        });
    }
}