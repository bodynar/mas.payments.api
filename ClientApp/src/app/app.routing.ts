import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { HomeComponent } from './components/home/home.component';
import { NotFoundComponent } from './pages/notFound/notFound.component';

import { MeasurementsComponent } from './areas/measurement/component/measurement.component';
import { AddMeasurementComponent } from './areas/measurement/components/addMeasurement/addMeasurement.component';
import { AddMeasurementTypeComponent } from './areas/measurement/components/addMeasurementType/addMeasurementType.component';
import { MeasurementListComponent } from './areas/measurement/components/measurementList/measurementList.component';

import { PaymentsComponent } from './areas/payments/component/payments.component';
import { AddPaymentComponent } from './areas/payments/components/addPayment/addPayment.component';
import { AddPaymentTypeComponent } from './areas/payments/components/addPaymentType/addPaymentType.component';
import { PaymentListComponent } from './areas/payments/components/paymentList/paymentList.component';

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
                        path: 'addPayment',
                        component: AddPaymentComponent
                    },
                    {
                        path: 'addPaymentType',
                        component: AddPaymentTypeComponent
                    }
                ]
            },
            {
                path: 'measures',
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
                    }
                ]
            },
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