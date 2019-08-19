import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { HomeComponent } from './components/home/home.component';
import { NotFoundComponent } from './pages/notFound/notFound.component';

import { MeasurementsComponent } from './areas/measurement/component/measurement.component';
import { PaymentsComponent } from './areas/payments/component/payments.component';
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
                loadChildren: () => import('./areas/payments/payments.module').then(x => x.PaymentsModule)
                // fix ts1323
            },
            {
                path: 'measurements',
                component: MeasurementsComponent,
                loadChildren: () => import('./areas/measurement/measurement.module').then(x => x.MeasurementModule)
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