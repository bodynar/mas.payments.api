import { Component, Input } from '@angular/core';

import { getFontColorFromString } from 'common/utils/colors';

@Component({
    selector: 'app-badge',
    templateUrl: 'badge.template.pug',
    styleUrls: ['badge.style.styl']
})
export class BadgeComponent {
    @Input()
    public mainColor: string;

    @Input()
    public fontColor: string =
        '#000';

    @Input()
    public text: string;

    @Input()
    public adjustFontColor: boolean =
        true;

    @Input()
    public clickable: boolean =
        false;

    public isHovered: boolean =
        false;

    constructor(
    ) {
    }

    public getFontColor(): string {
        return getFontColorFromString(this.mainColor);
    }
}