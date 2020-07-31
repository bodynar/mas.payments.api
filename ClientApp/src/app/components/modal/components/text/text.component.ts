import { Component, Input } from '@angular/core';

import { IModalService } from '../../IModalService';

import IModalComponent from '../../types/modalComponent.interface';

@Component({
    templateUrl: 'text.template.pug'
})
export class TextInModalComponent implements IModalComponent {
    @Input()
    public title: string;

    @Input()
    public modalBody: string;

    @Input()
    public isHtml: boolean =
        false;

    constructor(
        public modalService: IModalService
    ) {
    }
}