import { Component, OnDestroy } from '@angular/core';

import { Subject } from 'rxjs';
import { filter, switchMap } from 'rxjs/operators';

import { IAuthService } from 'services/IAuthService';
import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';

@Component({
    selector: 'app-user-logout',
    templateUrl: 'logout.template.pug',
    styleUrls: ['logout.style.styl']
})
class LogoutComponent implements OnDestroy {


    private whenLogoutClicked$: Subject<null> =
        new Subject();

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private authService: IAuthService,
        private notificationService: INotificationService,
        private routerService: IRouterService
    ) {
        this.whenLogoutClicked$
            .pipe(
                filter(_ => confirm('Are you sure want to logout?')),
                switchMap(_ => this.authService.logout()),
                filter(isSuccess => {

                    if (!isSuccess) {
                        this.notificationService.error('Error due logout. Please, try again later.');
                    }

                    return isSuccess;
                })
            )
            .subscribe(_ => this.routerService.navigate([]));
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public onIconClick(): void {
        this.whenLogoutClicked$.next(null);
    }
}

export { LogoutComponent };