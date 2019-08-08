import { Component, OnDestroy, OnInit } from '@angular/core';

import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { IUserService } from 'services/IUserService';

import { GetNotificationsResponse } from 'models/response/getNotificationsResponse';

@Component({
    selector: 'app-bell',
    templateUrl: 'bell.template.pug',
    styleUrls: ['bell.style.styl']
})
class BellComponent implements OnInit, OnDestroy {

    public notifications$: Subject<Array<GetNotificationsResponse>> =
        new Subject();

    public isNotificationsHidden: boolean =
        true;


    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private userService: IUserService
    ) { }

    public ngOnInit(): void {
        this.userService
            .getNotifications()
            .pipe(takeUntil(this.whenComponentDestroy$))
            .subscribe(notifications => this.notifications$.next(notifications));
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public notificationsToggle(target: HTMLElement): void {
        if (target.classList.contains('oi-bell') || target.parentElement.classList.contains('bell')) {
            this.isNotificationsHidden = !this.isNotificationsHidden;
        }
    }
}

export { BellComponent };