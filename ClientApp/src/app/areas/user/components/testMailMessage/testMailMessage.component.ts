import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Subject } from 'rxjs';
import { filter, switchMap, takeUntil } from 'rxjs/operators';

import BaseComponent from 'common/components/BaseComponent';

import { isNullOrUndefined } from 'common/utils/common';

import { INotificationService } from 'services/INotificationService';
import { IUserService } from 'services/IUserService';

import TestMailMessageRequest from 'models/request/user/testMailMessageRequest';

@Component({
    templateUrl: 'testMailMessage.template.pug'
})
export class TestMailMessageComponent extends BaseComponent {

    public mailMessageRequest: TestMailMessageRequest =
        {
            recipient: ''
        };

    public whenSendRequest$: Subject<NgForm> =
        new Subject();

    constructor(
        private userService: IUserService,
        private notificationService: INotificationService,
    ) {
        super();

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

    public onFormSend(form: NgForm): void {
        this.whenSendRequest$.next(form);
    }
}