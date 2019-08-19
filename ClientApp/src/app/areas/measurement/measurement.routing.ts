import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AddMeasurementComponent } from './components/addMeasurement/addMeasurement.component';
import { AddMeasurementTypeComponent } from './components/addMeasurementType/addMeasurementType.component';
import { MeasurementListComponent } from './components/measurementList/measurementList.component';
import { MeasurementTypesComponent } from './components/measurementTypes/measurementTypes.component';

const routes: Routes = [
    {
        path: '',
        component: MeasurementListComponent
    },
    {
        path: 'addMeasurement',
        component: AddMeasurementComponent
    },
    {
        path: 'addMeasurementType',
        component: AddMeasurementTypeComponent
    },
    {
        path: 'measurementTypes',
        component: MeasurementTypesComponent
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
    declarations: [],
})
export class MeasurementRoutingModule { }