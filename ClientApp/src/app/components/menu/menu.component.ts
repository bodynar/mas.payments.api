import { Component } from '@angular/core';

import { ReplaySubject, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import BaseComponent from 'common/components/BaseComponent';

import { isNullOrUndefined } from 'common/utils/common';

import { siteMenu } from 'static/siteMenu';

import { MenuItem } from 'models/menuItem';

import { IRouterService } from 'services/IRouterService';

@Component({
    selector: 'app-menu',
    templateUrl: 'menu.template.pug',
    styleUrls: ['menu.style.styl']
})
export class MenuComponent extends BaseComponent {
    public menuItems$: Subject<Array<MenuItem>> =
        new ReplaySubject(1);

    public searchPattern: string =
        '';

    private menuItems: Array<MenuItem>;

    constructor(
        private routerService: IRouterService,
    ) {
        super();

        this.whenComponentInit$
            .subscribe(() => {
                this.menuItems = siteMenu;
                this.menuItems$.next(this.menuItems);

                this.routerService
                    .whenRouteChange()
                    .pipe(takeUntil(this.whenComponentDestroy$))
                    .subscribe(([, rootPath]) => this.highlightMenuItem(rootPath));

                const currentRoute: string =
                    this.routerService
                        .getCurrentRoute()
                        .split('/')
                        .slice(1)
                        .join('/');

                this.highlightMenuItem(currentRoute);
            });
    }

    public onStartSearch(): void {
        if (this.searchPattern.trim() === '404') {
            this.routerService.navigate(['somePathWhichDoesntExist']);
        }
        if (this.searchPattern.endsWith('.py')) {
            alert('oh you little hacker');
        } else {
            // console.log('Search will be available soon..');
        }
    }

    public onMenuItemClick(menuItemLink: string): void {
        if (menuItemLink !== '') {
            this.routerService.navigate(['app', menuItemLink]);
        } else {
            this.routerService.navigate(['app']);
        }
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