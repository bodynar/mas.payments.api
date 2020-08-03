import { Component, Input } from '@angular/core';

import { IModalService } from '../../IModalService';

import IModalComponent, { IModalBodyOptions } from '../../types/modalComponent.interface';

@Component({
    templateUrl: 'text.template.pug',
    styleUrls: ['../modal.style.styl']
})
export class TextInModalComponent implements IModalComponent {
    @Input()
    public title: string;

    @Input()
    public body: IModalBodyOptions =
        {
            content: '',
            isHtml: false
        };

    constructor(
        public modalService: IModalService
    ) {
    }
}