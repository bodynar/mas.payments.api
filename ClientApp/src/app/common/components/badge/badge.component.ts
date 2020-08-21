import { Component, Input } from '@angular/core';

import { getFontColor } from 'common/utils/colors';

@Component({
    selector: 'app-badge',
    templateUrl: 'badge.template.pug',
    styleUrls: ['badge.style.styl']
})
export class BadgeComponent {
    @Input()
    public mainColor: string;

    @Input()
    public fontColor: string;

    @Input()
    public text: string;

    @Input()
    public adjustFontColor: boolean =
        false;

    public isHovered: boolean =
        false;

    constructor() {}

    // todo: calculate font color depending on bg
    public getFontColor(): string {
        return getFontColor(this.mainColor);
    }
}