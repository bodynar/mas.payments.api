import { Component, Input } from '@angular/core';

import BaseComponent from 'common/baseComponent';

import { Color, whiteHex, getFontColor } from 'common/utils/colors';

@Component({
    selector: 'app-action-icon',
    templateUrl: 'actionIcon.template.html',
    styleUrls: ['actionIcon.style.styl']
})
export class ActionIconComponent extends BaseComponent {
    @Input()
    public icon: string;

    @Input()
    public color: Color;

    public backgroundColor?: string;

    public color: string =
        whiteHex;

    constructor() {
        super();
    }

    public onMouseEvent(isOnElement: boolean): void {
        this.backgroundColor =
            isOnElement
                ? `rgba(${this.color.red}, ${this.color.green}, ${this.color.blue}, 0.25)`
                : undefined;
    }

    public onMouseClick(isClicking: boolean): void {
        if (isClicking) {
            this.backgroundColor = `rgba(${this.color.red}, ${this.color.green}, ${this.color.blue}, 0.5)`;
            this.color = getFontColor(this.color);
        } else {
            this.backgroundColor = undefined;
            this.color = whiteHex;
        }
    }
}