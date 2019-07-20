import { NgModule } from '@angular/core';

import { MeasurementComponent } from './component/measurement.component';
import { AddMeasurementComponent } from './components/addMeasurement/addMeasurement.component';
import { AddMeasurementTypeComponent } from './components/addMeasurementType/addMeasurementType.component';

@NgModule({
    imports: [],
    exports: [],
    declarations: [
        MeasurementComponent, AddMeasurementComponent, AddMeasurementTypeComponent
    ],
    providers: [],
})
class MeasurementModule { }

export { MeasurementModule };