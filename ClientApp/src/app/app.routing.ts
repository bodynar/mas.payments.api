import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { HomeComponent } from './components/home/home.component';
import { NotFoundComponent } from './pages/notFound/notFound.component';

import { MeasurementsComponent } from './areas/measurement/component/measurement.component';
import { AddMeasurementComponent } from './areas/measurement/components/addMeasurement/addMeasurement.component';
import { AddMeasurementTypeComponent } from './areas/measurement/components/addMeasurementType/addMeasurementType.component';
import { MeasurementListComponent } from './areas/measurement/components/measurementList/measurementList.component';
import { MeasurementTypesComponent } from './areas/measurement/components/measurementTypes/measurementTypes.component';

import { PaymentsComponent } from './areas/payments/component/payments.component';
import { AddPaymentComponent } from './areas/payments/components/addPayment/addPayment.component';
import { AddPaymentTypeComponent } from './areas/payments/components/addPaymentType/addPaymentType.component';
import { PaymentListComponent } from './areas/payments/components/paymentList/paymentList.component';
import { PaymentTypesComponent } from './areas/payments/components/paymentTypes/paymentTypes.component';
import { UpdatePaymentComponent } from './areas/payments/components/updatePayment/updatePayment.component';

import { StatisticsComponent } from './areas/statistics/component/statistics.component';
import { StatsComponent } from './areas/statistics/components/stats/stats.component';

const routes: Routes = [
    {
        path: '',
        redirectTo: 'app',
        pathMatch: 'full'
    },
    {
        path: 'app',
        component: AppComponent,
        children: [
            {
                path: '',
                component: HomeComponent
            },
            {
                path: 'payments',
                component: PaymentsComponent,
                children: [
                    {
                        path: '',
                        component: PaymentListComponent
                    },
                    {
                        path: 'add',
                        component: AddPaymentComponent
                    },
                    {
                        path: 'update',
                        component: UpdatePaymentComponent
                    },
                    {
                        path: 'addType',
                        component: AddPaymentTypeComponent
                    },
                    {
                        path: 'types',
                        component: PaymentTypesComponent
                    },
                ]
            },
            {
                path: 'measurements',
                component: MeasurementsComponent,
                children: [
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
                ]
            },
            {
                path: 'stats',
                component: StatisticsComponent,
                children: [
                    {
                        path: '',
                        component: StatsComponent
                    }
                ]
            }
        ]
    },
    {
        path: '404',
        component: NotFoundComponent
    },
    {
        path: '**',
        redirectTo: '404'
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    declarations: [],
})
class AppRoutingModule { }

export { AppRoutingModule };