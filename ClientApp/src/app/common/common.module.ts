import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { ColorValidatorDirective } from './directives/colorValidator/colorValidator.directive';
import { HexColorValidatorDirective } from './directives/hexColorValidator/hexColorValidator.directive';
import { PositiveNumberValidatorDirective } from './directives/positiveNumberValidator/positiveNumber.directive';

import { PaginatorComponent } from './components/paginator/component/paginator.component';
import { BadgeComponent } from './components/badge/badge.component';
import { NoItemsComponent } from './components/noItems/noItems.component';
import { MonthSelectorComponent } from './components/monthSelector/monthSelector.component';
import { ActionIconComponent } from './components/actionIcon/actionIcon.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule
    ],
    exports: [
        PaginatorComponent,
        BadgeComponent,
        NoItemsComponent,
        MonthSelectorComponent,
        ActionIconComponent,

        ColorValidatorDirective,
        HexColorValidatorDirective,
        PositiveNumberValidatorDirective,
    ],
    declarations: [
        PaginatorComponent,
        BadgeComponent,
        NoItemsComponent,
        MonthSelectorComponent,
        ActionIconComponent,

        ColorValidatorDirective,
        HexColorValidatorDirective,
        PositiveNumberValidatorDirective,
    ],
    providers: [],
})
export class AppCommonModule { }