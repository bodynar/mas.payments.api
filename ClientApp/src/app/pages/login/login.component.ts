import { Component, OnDestroy } from '@angular/core';
import { NgForm } from '@angular/forms';

import { BehaviorSubject, Subject } from 'rxjs';
import { delay, filter, switchMap, takeUntil, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { AuthenticateRequest } from 'models/request/authenticateRequest';

import { IAuthService } from 'services/IAuthService';
import { IRouterService } from 'services/IRouterService';

@Component({
    templateUrl: 'login.template.pug',
    styleUrls: ['login.style.styl']
})
class LoginComponent implements OnDestroy {
    public userLoginRequest: AuthenticateRequest =
        new AuthenticateRequest();

    public isBusy$: Subject<boolean> =
        new BehaviorSubject(false);

    public whenUserAttemptAuth$: Subject<NgForm> =
        new Subject();


    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private routerService: IRouterService,
        private authService: IAuthService,
    ) {
        this.whenUserAttemptAuth$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(form => form.valid),
                tap(_ => this.isBusy$.next(true)),
                switchMap(_ => this.authService.authenticate(this.userLoginRequest)),
                delay(1 * 1500),
                tap(_ => this.isBusy$.next(false)),
                filter(token => {
                    const isTokenValid: boolean =
                        !isNullOrUndefined(token) && token !== '';

                    // display login errors

                    return isTokenValid;
                }),
                tap(token => this.authService.setAuthToken(token)),
                delay(1 * 1500),
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