import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { StatisticsComponent } from './component/statistics.component';
import { StatsComponent } from './components/stats/stats.component';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
    ],
    exports: [],
    declarations: [
        StatisticsComponent, StatsComponent
    ],
    providers: [],
})
class StatisticsModule { }

export { StatisticsModule };