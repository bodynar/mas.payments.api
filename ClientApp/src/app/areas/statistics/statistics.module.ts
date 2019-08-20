import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { StatisticsRoutingModule } from './statistics.routing';

import { StatisticsComponent } from './component/statistics.component';
import { StatsComponent } from './components/stats/stats.component';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        StatisticsRoutingModule
    ],
    exports: [],
    declarations: [
        StatisticsComponent, StatsComponent
    ],
    providers: [],
})
class StatisticsModule { }

export { StatisticsModule };