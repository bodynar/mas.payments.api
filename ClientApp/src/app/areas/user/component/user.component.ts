import { Component } from '@angular/core';

import { takeUntil, filter, map } from 'rxjs/operators';
import { Subject } from 'rxjs';

import BaseComponent from 'common/components/BaseComponent';

import { isNullOrUndefined } from 'common/utils/common';
import { ArrayUtils } from 'common/utils/array';

import { userSideMenu } from 'static/siteMenu';

import { IRouterService } from 'services/IRouterService';

import { MenuItem } from 'models/menuItem';

@Component({
    templateUrl: 'user.template.pug',
    styleUrls: ['user.style.styl']
})
export class UserComponent extends BaseComponent {
    public menuItems: Array<MenuItem>;

    private whenDestroyComponent$: Subject<null> =
        new Subject();

    private readonly userRoutePath: Array<string> =
        ['app', 'user'];

    constructor(
        private routerService: IRouterService
    ) {
        super();

        this.whenComponentInit$
            .subscribe(() => {
                this.menuItems = userSideMenu;

                this.routerService
                    .whenRouteChange()
                    .pipe(
                        takeUntil(this.whenDestroyComponent$),
                        filter(routePath =>
                            ArrayUtils.startsWith<string>(routePath, this.userRoutePath)
                            && (routePath.length === 3 || routePath.length === 2)),
                        map(routePath => routePath.length === 3 ? routePath.pop() : '')
                    )
                    .subscribe(menuItem => this.highlightMenuItem(menuItem));

                const currentRoute: string =
                    this.routerService
                        .getCurrentRoute()
                        .split('/')
                        .slice(2)
                        .join('/');

                this.highlightMenuItem(currentRoute);
            });
    }

    public onMenuItemClick(menuItem: string): void {
        if (menuItem !== '') {
            this.routerService.navigate([...this.userRoutePath, menuItem]);
        } else {
            this.routerService.navigate(this.userRoutePath);
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