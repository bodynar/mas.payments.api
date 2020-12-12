import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { StatsComponent } from './components/stats/stats.component';

const routes: Routes = [
    {
        path: '',
        component: StatsComponent
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
    declarations: [],
})
export class StatisticsRoutingModule { }