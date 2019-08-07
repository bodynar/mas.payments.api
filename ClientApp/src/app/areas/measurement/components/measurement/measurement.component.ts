import { Component, Input } from '@angular/core';

import { MeasurementResponse } from 'models/response/measurementResponse';

import { months } from 'src/static/months';

@Component({
    selector: 'app-measurement-item',
    templateUrl: 'measurement.template.pug',
    styleUrls: ['measurement.style.styl']
})
class MeasurementComponent {
    @Input()
    public measurement: MeasurementResponse;

    constructor(
    ) { }

    public formatDate(rawDate: string): string {
        const date: Date =
            new Date(rawDate);

        const month: number =
            date.getMonth();

        return `[${date.getFullYear()}] ${months[month].name}`;
    }
}

export { MeasurementComponent };