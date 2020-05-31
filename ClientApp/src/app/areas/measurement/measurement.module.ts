import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { MeasurementRoutingModule } from './measurement.routing';

import { MeasurementsComponent } from './component/measurement.component';

import { AddMeasurementComponent } from './components/addMeasurement/addMeasurement.component';
import { MeasurementComponent } from './components/measurement/measurement.component';
import { MeasurementListComponent } from './components/measurementList/measurementList.component';
import { UpdateMeasurementComponent } from './components/updateMeasurement/updateMeasurement.component';

import { AddMeasurementTypeComponent } from './components/addMeasurementType/addMeasurementType.component';
import { MeasurementGroupComponent } from './components/measurementGroup/measurementGroup.component';
import { MeasurementTypesComponent } from './components/measurementTypes/measurementTypes.component';
import { UpdateMeasurementTypeComponent } from './components/updateMeasurementType/updateMeasurementType.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
        MeasurementRoutingModule
    ],
    exports: [],
    declarations: [
        MeasurementsComponent,
        AddMeasurementComponent,
        AddMeasurementTypeComponent,
        UpdateMeasurementComponent,
        MeasurementListComponent,
        MeasurementGroupComponent,
        MeasurementComponent,
        MeasurementTypesComponent,
        UpdateMeasurementTypeComponent
    ],
    providers: [],
})
class MeasurementModule { }

export { MeasurementModule };