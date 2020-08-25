import { Component } from '@angular/core';

import { BehaviorSubject } from 'rxjs';
import { map, tap, switchMapTo } from 'rxjs/operators';

import BaseComponent from 'common/components/BaseComponent';

import { IRouterService } from 'services/IRouterService';

@Component({
    selector: 'app-user',
    templateUrl: 'user.template.pug',
    styleUrls: ['user.style.styl']
})
export class AppUserComponent extends BaseComponent {
    public isUserPage$: BehaviorSubject<boolean> =
        new BehaviorSubject(false);

    constructor(
        private routerService: IRouterService
    ) {
        super();

        this.whenComponentInit$
            .pipe(
                switchMapTo(this.routerService.whenRouteChange()),
                map(routeParams => {
                    const isUserPage: boolean =
                        routeParams.join('/').startsWith('app/user');

                    return isUserPage;
                }),
                tap(isUserPage => this.isUserPage$.next(isUserPage))
            )
            .subscribe();
    }

    public onIconClick(): void {
        this.routerService.navigate(['app', 'user']);
    }
}