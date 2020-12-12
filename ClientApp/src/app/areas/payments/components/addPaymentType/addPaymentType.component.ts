import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';

import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';
import { delay, filter, switchMap, takeUntil } from 'rxjs/operators';

import BaseRoutingComponent from 'common/components/BaseRoutingComponent';

import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';
import { IRouterService } from 'services/IRouterService';

import { AddPaymentTypeRequest } from 'models/request/payment/addPaymentTypeRequest';

@Component({
    templateUrl: 'addPaymentType.template.pug'
})
export class AddPaymentTypeComponent extends BaseRoutingComponent {

    public isLoading$: Subject<boolean> =
        new BehaviorSubject(false);

    public addPaymentTypeRequest: AddPaymentTypeRequest =
        {
            color: '#f04747'
        };

    public whenSubmittedForm$: Subject<NgForm> =
        new ReplaySubject(1);

    constructor(
        private paymentService: IPaymentService,
        private notificationService: INotificationService,
        routerService: IRouterService,
    ) {
        super(routerService);
        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid),
                switchMap(_ => {
                    this.isLoading$.next(true);
                    return this.paymentService.addPaymentType(this.addPaymentTypeRequest);
                }),
                delay(1.5 * 1000),
                filter(response => {
                    this.isLoading$.next(false);
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    } else {
                        this.notificationService.success('Payment type was successfully added.');
                    }

                    return response.success;
                })
            )
            .subscribe(_ => this.routerService.navigateArea(['types']));
    }

    public onFormSubmit(form: NgForm): void {
        this.whenSubmittedForm$.next(form);
    }
}