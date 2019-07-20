import { Component } from '@angular/core';

import { MenuItem } from 'models/menuItem';

@Component({
    selector: 'app-menu',
    templateUrl: 'menu.template.pug',
    styleUrls: ['menu.style.styl']
})
class MenuComponent {

    public menuItems: Array<MenuItem> =
        [
            {
                name: 'home',
                link: '',
                description: 'home sweet home',
                isActive: true
            },
            {
                name: 'Payments',
                link: 'payments'
            }
        ];

    public searchPattern: string =
        '';

    public onStartSearch() {

        if (this.searchPattern.endsWith('.py')) {
            alert('oh you little hacker');
        } else {
            console.log('Search will be available soon..');
        }
    }
}

export { MenuComponent };