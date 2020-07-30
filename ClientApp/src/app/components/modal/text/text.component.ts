import { Component, Input } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    templateUrl: 'text.template.pug'
})

export class TextInModalComponent {
    @Input()
    public title: string;

    @Input()
    public text: string;

    @Input()
    public isHtml: boolean =
        false;

    constructor(
        public activeModal: NgbActiveModal
    ) { }
}