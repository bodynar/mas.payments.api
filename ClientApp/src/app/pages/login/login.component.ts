import { Component, OnDestroy } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Subject } from 'rxjs';
import { filter, switchMap, takeUntil, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { UserLoginRequest } from 'models/request/userLoginRequest';

import { IRouterService } from 'services/IRouterService';
import { IUserService } from 'services/IUserService';

@Component({
    templateUrl: 'login.template.pug',
    styleUrls: ['login.style.styl']
})
class LoginComponent implements OnDestroy {
    public userLoginRequest: UserLoginRequest =
        new UserLoginRequest();


    private whenUserAttemptAuth$: Subject<NgForm> =
        new Subject();

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private routerService: IRouterService,
        private userService: IUserService,
    ) {
        this.whenUserAttemptAuth$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(form => form.valid),
                switchMap(_ => this.userService.login(this.userLoginRequest)),
                filter(token => {
                    const isTokenValid: boolean =
                        !isNullOrUndefined(token) && token !== '';

                    // display login errors

                    return isTokenValid;
                }),
                tap(token => this.userService.setAuthToken(token)),
            )
            .subscribe(_ => {
                this.routerService.navigate(['app']);
            });
    }

    public onUserTryAuth(form: NgForm): void {
        this.whenUserAttemptAuth$.next(form);
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }
}

export { LoginComponent };