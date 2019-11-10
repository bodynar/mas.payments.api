import { Component, OnDestroy, OnInit } from '@angular/core';

import { BehaviorSubject, Subject } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { IRouterService } from 'services/IRouterService';
import { IUserService } from 'services/IUserService';

@Component({
    selector: 'app-user-profile',
    templateUrl: 'user.template.pug',
    styleUrls: ['user.style.styl']
})
class AppUserComponent implements OnInit, OnDestroy {
    public isUserPage$: BehaviorSubject<boolean> =
        new BehaviorSubject(false);

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private userService: IUserService,
        private routerService: IRouterService
    ) {
    }

    public ngOnInit(): void {
        this.routerService
            .whenRouteChange()
            .pipe(
                map(routeParams => {
                    const isUserPage: boolean =
                        routeParams.join('/').startsWith('app/user');

                    return isUserPage;
                }),
                tap(isUserPage => this.isUserPage$.next(isUserPage))
            )
            .subscribe();
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public onIconClick(): void {
        this.routerService.navigate(['app', 'user']);
    }
}

export { AppUserComponent };