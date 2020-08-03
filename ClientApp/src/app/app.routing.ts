import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { HomeComponent } from './components/home/home.component';
import { NotFoundComponent } from './pages/notFound/notFound.component';

import { routes as MeasurementRoutes } from './areas/measurement/measurement.routing';
import { routes as PaymentRoutes } from './areas/payments/payments.routing';
import { routes as StatisticsRoutes } from './areas/statistics/statistics.routing';
import { routes as UserRoutes } from './areas/user/user.routing';

import { MeasurementsComponent } from './areas/measurement/component/measurement.component';
import { PaymentsComponent } from './areas/payments/component/payments.component';
import { StatisticsComponent } from './areas/statistics/component/statistics.component';
import { UserComponent } from './areas/user/component/user.component';

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
                children: [...PaymentRoutes]
            },
            {
                path: 'measurements',
                component: MeasurementsComponent,
                children: [...MeasurementRoutes]
            },
            {
                path: 'stats',
                component: StatisticsComponent,
                children: [...StatisticsRoutes]
            },
            {
                path: 'user',
                component: UserComponent,
                children: [...UserRoutes]
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

export { AppRoutingModule }