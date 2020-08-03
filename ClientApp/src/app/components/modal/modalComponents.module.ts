import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { IModalService } from './IModalService';
import { ModalService } from './modal.service';

import { ConfirmInModalComponent } from './components/confirm/confirm.component';
import { TextInModalComponent } from './components/text/text.component';

@NgModule({
    imports: [
        CommonModule
    ],
    exports: [],
    declarations: [
        TextInModalComponent,
        ConfirmInModalComponent,
    ],
    providers: [
        { provide: IModalService, useClass: ModalService },
    ],
    entryComponents: [
        TextInModalComponent,
        ConfirmInModalComponent,
    ]
})
export class ModalComponentsModule { }