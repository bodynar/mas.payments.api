import { Component, Input } from '@angular/core';

import { MeasurementResponse } from 'models/response/measurementResponse';

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
}

export { MeasurementComponent };