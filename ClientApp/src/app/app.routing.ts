import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './components/app/app.component';
import { HomeComponent } from './components/home/home.component';
import { NotFoundComponent } from './pages/notFound/notFound.component';

import { MeasurementsComponent } from './areas/measurement/component/measurement.component';
import { PaymentsComponent } from './areas/payments/component/payments.component';
import { StatisticsComponent } from './areas/statistics/component/statistics.component';
import { UserComponent } from './areas/user/component/user.component';
import { ConfirmRegistrationComponent } from './areas/user/components/confirmRegistration/confirmRegistration.component';

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
            },
            {
                path: 'measurements',
                component: MeasurementsComponent,
                loadChildren: () => import('./areas/measurement/measurement.module').then(x => x.MeasurementModule)
            },
            // {
            //     path: 'stats',
            //     component: StatisticsComponent,
            //     loadChildren: () => import('./areas/statistics/statistics.module').then(x => x.StatisticsModule)
            // },
            {
                path: 'user',
                component: UserComponent,
                loadChildren: () => import('./areas/user/user.module').then(x => x.UserModule)
            },
            {
                path: 'confirm',
                component: ConfirmRegistrationComponent
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