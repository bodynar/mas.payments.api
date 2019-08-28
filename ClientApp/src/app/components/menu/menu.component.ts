import { Component, OnDestroy, OnInit } from '@angular/core';

import { ReplaySubject, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { siteMenu } from 'src/static/siteMenu';

import { MenuItem } from 'models/menuItem';

import { IRouterService } from 'services/IRouterService';

@Component({
    selector: 'app-menu',
    templateUrl: 'menu.template.pug',
    styleUrls: ['menu.style.styl']
})
class MenuComponent implements OnInit, OnDestroy {
    public menuItems$: Subject<Array<MenuItem>> =
        new ReplaySubject(1);

    public searchPattern: string =
        '';

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    private menuItems: Array<MenuItem>;

    constructor(
        private routerService: IRouterService,
    ) {
        this.menuItems = siteMenu;
        this.menuItems$.next(this.menuItems);
    }

    public ngOnInit(): void {
        this.routerService
            .whenRouteChange()
            .pipe(takeUntil(this.whenComponentDestroy$))
            .subscribe(([, rootPath]) => this.highlightMenuItem(rootPath));

        const currentRoute: string =
            this.routerService.getCurrentRoute();

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
        this.routerService.navigate([menuItemLink]);
    }

    private highlightMenuItem(menuItemName: string): void {
        this.menuItems.forEach(item => item.isActive = false);

        if (isNullOrUndefined(menuItemName)) {
            return;
        }

        let clickedMenuItem: string =
            menuItemName;

        if (!menuItemName.startsWith('app/')) {
            clickedMenuItem = `app/${menuItemName}`;
        }

        const menuItem: MenuItem =
            this.menuItems
                .filter(item => item.link === clickedMenuItem)
                .pop();

        if (!isNullOrUndefined(menuItem)) {
            menuItem.isActive = true;
        }
    }
}

export { MenuComponent };