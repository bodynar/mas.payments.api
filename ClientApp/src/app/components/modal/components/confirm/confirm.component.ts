import { Component, Input } from '@angular/core';

import { IModalService } from '../../IModalService';

import IModalComponent from '../../types/modalComponent.interface';

@Component({
    templateUrl: 'confirm.template.pug'
})
export class ConfirmInModalComponent implements IModalComponent {
    @Input()
    public confirmBtnText: string
        = 'Ok';

    @Input()
    public cancelBtnText: string
        = 'Cancel';

    @Input()
    public modalBody: string =
        '';

    @Input()
    public title: string;

    constructor(
        public modalService: IModalService
    ) {
    }
}