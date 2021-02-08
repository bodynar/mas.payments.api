import { Component, EventEmitter, Input, Output } from '@angular/core';

import BaseComponent from 'common/components/BaseComponent';

import { blackHex, Color, getFontColor } from 'common/utils/colors';

@Component({
    selector: 'app-action-icon',
    templateUrl: 'actionIcon.template.pug',
    styleUrls: ['actionIcon.style.styl']
})
export class ActionIconComponent extends BaseComponent {
    @Input()
    public icon: string;

    @Input()
    public color: Color;

    // tslint:disable-next-line: no-output-native
    @Output()
    public click: EventEmitter<null> =
        new EventEmitter();

    public backgroundColor?: string;

    public fontColor: string
        = blackHex;

    public fontIcon: string;

    constructor() {
        super();

        this.whenComponentInit$
            .subscribe(_ => this.fontIcon = `oi-${this.icon}`);
    }

    public onMouseEvent(isOnElement: boolean): void {
        this.backgroundColor =
            isOnElement
                ? `rgba(${this.color.red}, ${this.color.green}, ${this.color.blue}, 0.25)`
                : undefined;

        if (!isOnElement) {
            this.fontColor = blackHex;
        }
    }

    public onMouseClick({ buttons }: MouseEvent, isClicking: boolean): void {
        if (buttons === 1) {
            if (isClicking) {
                this.backgroundColor = `rgba(${this.color.red}, ${this.color.green}, ${this.color.blue}, 0.75)`;
                this.fontColor = getFontColor(this.color);

                this.click.emit(null);
            } else {
                this.backgroundColor = undefined;
                this.fontColor = blackHex;
            }
        }
    }
}