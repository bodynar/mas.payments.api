import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { PaginatorComponent } from './paginator/component/paginator.component';

@NgModule({
    imports: [
        CommonModule
    ],
    exports: [PaginatorComponent],
    declarations: [
        PaginatorComponent
    ],
    providers: [],
})
export class AppCommonModule { }