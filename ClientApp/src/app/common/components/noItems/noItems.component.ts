import { Component, Input } from '@angular/core';

import BaseComponent from '../BaseComponent';

import { isNullOrEmpty } from 'common/utils/common';

@Component({
    selector: 'app-no-items',
    templateUrl: './noItems.template.pug',
    styleUrls: ['./noItems.style.styl']
})
export class NoItemsComponent extends BaseComponent {
    @Input()
    public additionalText?: string
        = undefined;

    public hasAdditionalText: boolean =
        isNullOrEmpty(this.additionalText);

    constructor() {
        super();
    }
}