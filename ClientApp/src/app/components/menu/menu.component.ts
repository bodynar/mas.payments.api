import { Component, OnDestroy, OnInit } from '@angular/core';

import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { siteMenu } from 'src/static/siteMenu';

import { MenuItem } from 'models/menuItem';

import { IAuthService } from 'services/IAuthService';
import { IRouterService } from 'services/IRouterService';

@Component({
    selector: 'app-menu',
    templateUrl: 'menu.template.pug',
    styleUrls: ['menu.style.styl']
})
class MenuComponent implements OnInit, OnDestroy {
    public isAuthenticated$: Subject<boolean> =
        new BehaviorSubject(false);

    public menuItems$: Subject<Array<MenuItem>> =
        new ReplaySubject(1);

    public searchPattern: string =
        '';

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    private menuItems: Array<MenuItem>;

    constructor(
        private routerService: IRouterService,
        private authService: IAuthService,
    ) {
        this.menuItems = siteMenu;
        this.menuItems$.next(this.menuItems);

        this.authService
            .isAuthenticated()
            .subscribe(isAuthenticated => this.isAuthenticated$.next(isAuthenticated));
    }

    public ngOnInit(): void {
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
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public onStartSearch() {
        if (this.searchPattern.trim() === '404') {
            this.routerService.navigate(['somePathWhichDoesntExist']);
        }
        if (this.searchPattern.endsWith('.py')) {
            alert('oh you little hacker');
        } else {
            console.log('Search will be available soon..');
        }
    }

    public onMenuItemClick(menuItemLink: string) {
        this.routerService.navigate(['app', menuItemLink]);
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

export { MenuComponent };