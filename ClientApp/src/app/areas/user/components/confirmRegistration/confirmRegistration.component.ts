import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, Subject } from 'rxjs';
import { delay, filter, map, switchMap, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';
import { IUserService } from 'services/IUserService';

@Component({
    templateUrl: 'confirmRegistration.template.pug',
    styleUrls: ['confirmRegistration.style.styl']
})
class ConfirmRegistrationComponent {
    public isBusy$: BehaviorSubject<boolean> =
        new BehaviorSubject(false);

    public token: string =
        '';

    private whenTokenSubmitted$: Subject<string> =
        new Subject();

    constructor(
        private userService: IUserService,
        private activatedRoute: ActivatedRoute,
        private notificationService: INotificationService,
        private routerService: IRouterService,
    ) {
        this.activatedRoute
            .queryParams
            .pipe(
                filter(params => !isNullOrUndefined(params['token']) && params['token'] !== ''),
                map(params => params['token']),
                tap(token => this.token = token),
            )
            .subscribe(token => this.whenTokenSubmitted$.next(token));

        this.whenTokenSubmitted$
            .pipe(
                filter(token => !isNullOrUndefined(token) && token !== ''),
                tap(_ => this.isBusy$.next(true)),
                switchMap(token => this.userService.confirmRegistration(token)),
                delay(1 * 1000),
                tap(_ => this.isBusy$.next(false)),
                filter(result => {
                    if (!result) {
                        this.notificationService.error('Error due confirming registration. Please, try again later');
                    } else {
                        this.notificationService.success('Registration confirmed successfully. You may log in');
                    }

                    return result;
                })
            )
            .subscribe(_ => {
                this.routerService.navigate(['user', 'login']);
            });
    }

    public onTokenSubmit(): void {
        this.whenTokenSubmitted$.next(this.token);
    }
}

export { ConfirmRegistrationComponent };