import { Component, Input } from '@angular/core';

import { IModalService } from '../../IModalService';

import IModalComponent, { IModalBodyOptions } from '../../types/modalComponent.interface';

@Component({
    templateUrl: 'confirm.template.pug',
    styleUrls: ['../modal.style.styl']
})
export class ConfirmInModalComponent implements IModalComponent {
    @Input()
    public confirmBtnText: string
        = 'Ok';

    @Input()
    public cancelBtnText: string
        = 'Cancel';

    @Input()
    public body: IModalBodyOptions =
        {
            content: '',
            isHtml: false
        };

    @Input()
    public title: string;

    constructor(
        public modalService: IModalService
    ) {
    }
}