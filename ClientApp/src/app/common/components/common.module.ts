import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { PaginatorComponent } from './paginator/component/paginator.component';
import { BadgeComponent } from './badge/badge.component';

@NgModule({
    imports: [
        CommonModule
    ],
    exports: [
        PaginatorComponent,
        BadgeComponent
    ],
    declarations: [
        PaginatorComponent,
        BadgeComponent
    ],
    providers: [],
})
export class AppCommonModule { }