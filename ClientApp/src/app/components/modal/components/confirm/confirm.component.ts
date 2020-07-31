import { Component, Input } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import IModalComponent from '../../types/modalComponent.interface';

@Component({
    templateUrl: 'confirm.template.pug'
})
export class ConfirmInModalComponent extends IModalComponent {
    @Input()
    public confirmBtnText: string
        = 'Ok';

    @Input()
    public message: string =
        '';

    @Input()
    public title: string;

    constructor(
        public activeModal: NgbActiveModal
    ) {
        super();
        const a: typeof NgbActiveModal = NgbActiveModal;
    }
}