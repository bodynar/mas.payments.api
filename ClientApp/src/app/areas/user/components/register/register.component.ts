import { Component, OnDestroy } from '@angular/core';

import { Subject } from 'rxjs';
import { delay, filter, switchMap, takeUntil } from 'rxjs/operators';

import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';
import { IUserService } from 'services/IUserService';

import { UserRegisterRequest } from 'models/request/userRegisterRequest';

@Component({
    templateUrl: 'register.template.pug',
    styleUrls: ['register.style.styl']
})
class RegisterComponent implements OnDestroy {
    public userRegistrationRequest: UserRegisterRequest =
        new UserRegisterRequest();


    private whenFormSubmit$: Subject<null> =
        new Subject();

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private userService: IUserService,
        private notificationService: INotificationService,
        private routerService: IRouterService,
    ) {
        this.whenFormSubmit$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                switchMap(_ => this.userService.register(this.userRegistrationRequest)),
                delay(1 * 1000),
                filter(isSuccess => {
                    if (!isSuccess) {
                        this.notificationService.error('Error due registration. Please, try again later');
                    } else {
                        this.notificationService
                            .success(`Registration successfully. Please, check ${this.userRegistrationRequest.email} mailbox`);
                    }

                    return isSuccess;
                })
            )
            .subscribe();
    }

    public onFormSubmit(): void {
        this.whenFormSubmit$.next(null);
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }
}

export { RegisterComponent };