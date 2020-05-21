import { Component, OnDestroy } from '@angular/core';

import { BehaviorSubject, Subject } from 'rxjs';
import { filter, switchMap, takeUntil } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IUserService } from 'services/IUserService';

import TestMailMessageRequest from 'models/request/user/testMailMessageRequest';
import { INotificationService } from 'services/INotificationService';

@Component({
    templateUrl: 'testMailMessage.template.pug'
})
class TestMailMessageComponent implements OnDestroy {

    public mailMessageRequest: TestMailMessageRequest =
        {
            recipient: ''
        };

    public isModelMode$: Subject<boolean> =
        new BehaviorSubject(false);


    private whenSendRequest$: Subject<TestMailMessageRequest> =
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
                filter(request => !isNullOrUndefined(request.recipient) && request.recipient !== ''),
                switchMap(request => this.userService.sendTestMailMessage(request)),
                filter(isOperationSucceeded => {
                    if (!isOperationSucceeded) {
                        this.notificationService.error('Error occurred due adding message to queue');
                    }

                    return isOperationSucceeded;
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

    public onFormSend(): void {
        this.whenSendRequest$.next(this.mailMessageRequest);
    }

    public onModelModeChange(value: boolean): void {
        this.isModelMode$.next(value);
    }
}

export { TestMailMessageComponent };