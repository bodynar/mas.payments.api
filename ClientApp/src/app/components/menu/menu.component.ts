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
                description: 'home sweet home'
            },
            {
                name: 'Payments',
                link: 'payments'
            }
        ];


}

export { MenuComponent };