import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MeasurementComponent } from './areas/measurement/component/measurement.component';
import { PaymentsComponent } from './areas/payments/component/payments.component';
import { AppComponent } from './components/app/app.component';
import { HomeComponent } from './components/home/home.component';
import { NotFoundComponent } from './pages/notFound/notFound.component';

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
                component: PaymentsComponent
            },
            {
                path: 'measures',
                component: MeasurementComponent
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