import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { TextInModalComponent } from './text/text.component';

@NgModule({
    imports: [
        CommonModule
    ],
    exports: [],
    declarations: [TextInModalComponent],
    providers: [],
    entryComponents: [TextInModalComponent]
})
export class ModalComponentsModule { }