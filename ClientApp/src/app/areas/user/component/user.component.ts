import { Component, OnInit } from '@angular/core';

import { isNullOrUndefined } from 'util';

import { IRouterService } from 'services/IRouterService';

import { MenuItem } from 'models/menuItem';

@Component({
    templateUrl: 'user.template.pug',
    styleUrls: ['user.style.styl']
})
class UserComponent implements OnInit {
    public menuItems: Array<MenuItem> =
        [
            {
                name: 'User card',
                link: '',
                isActive: false,
                isEnabled: true,
            },
            {
                name: 'Test mail message',
                link: 'mailTest',
                isActive: false,
                isEnabled: true,
            }
        ];

    constructor(
        private routerService: IRouterService
    ) {
    }

    public ngOnInit(): void {
        const currentRoute: string =
            this.routerService
                .getCurrentRoute()
                .split('/')
                .slice(2)
                .join('/');

        this.highlightMenuItem(currentRoute);
    }

    public onModuleNameClick(): void {
        this.routerService.navigate(['app', 'user']);
    }

    public onMenuItemClick(menuItem: string): void {
        if (isNullOrUndefined(menuItem) || menuItem === '') {
            this.onModuleNameClick();
        }
        else {
            this.routerService.navigate(['app', 'user', menuItem]);
        }

        this.highlightMenuItem(menuItem);
    }


    private highlightMenuItem(menuItemName: string): void {
        this.menuItems.forEach(item => item.isActive = false);

        if (isNullOrUndefined(menuItemName)) {
            return;
        }

        const menuItem: MenuItem =
            this.menuItems
                .filter(item => item.link === menuItemName)
                .pop();

        if (!isNullOrUndefined(menuItem)) {
            menuItem.isActive = true;
        }
    }
}

export { UserComponent };