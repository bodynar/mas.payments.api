import { Component, OnDestroy } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Subject } from 'rxjs';
import { filter, switchMap, takeUntil } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { INotificationService } from 'services/INotificationService';
import { IUserService } from 'services/IUserService';

import TestMailMessageRequest from 'models/request/user/testMailMessageRequest';

@Component({
    templateUrl: 'testMailMessage.template.pug'
})
class TestMailMessageComponent implements OnDestroy {

    public mailMessageRequest: TestMailMessageRequest =
        {
            recipient: ''
        };

    private whenSendRequest$: Subject<NgForm> =
        new Subject();

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private userService: IUserService,
        private notificationService: INotificationService,
    ) {
        this.whenSendRequest$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid),
                filter(_ => !isNullOrUndefined(this.mailMessageRequest.recipient) && this.mailMessageRequest.recipient !== ''),
                switchMap(_ => this.userService.sendTestMailMessage(this.mailMessageRequest)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }

                    return response.success;
                })
            )
            .subscribe(_ => {
                this.notificationService.success('Message was sucessfully added to queue');
            });
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public onFormSend(form: NgForm): void {
        this.whenSendRequest$.next(form);
    }
}

export { TestMailMessageComponent };