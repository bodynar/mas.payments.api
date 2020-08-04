import { Component, OnInit } from '@angular/core';

import { isNullOrUndefined } from 'common/utils/common';

import { userSideMenu } from 'static/siteMenu';

import { IRouterService } from 'services/IRouterService';

import { MenuItem } from 'models/menuItem';

@Component({
    templateUrl: 'user.template.pug',
    styleUrls: ['user.style.styl']
})
class UserComponent implements OnInit {
    public menuItems: Array<MenuItem>;

    constructor(
        private routerService: IRouterService
    ) {
        this.menuItems = userSideMenu;
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

    public onMenuItemClick(menuItem: string): void {
        if (menuItem !== '') {
            this.routerService.navigate(['app', 'user', menuItem]);
        } else {
            this.routerService.navigate(['app', 'user']);
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