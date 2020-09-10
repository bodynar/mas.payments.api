import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, switchMap, takeUntil, switchMapTo } from 'rxjs/operators';

import BaseRoutingComponent from 'common/components/BaseRoutingComponent';

import { yearsRange } from 'common/utils/years';
import { months } from 'static/months';

import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';
import { IRouterService } from 'services/IRouterService';

import { AddPaymentRequest } from 'models/request/payment/addPaymentRequest';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';

@Component({
    templateUrl: 'addPayment.template.pug'
})
export class AddPaymentComponent extends BaseRoutingComponent {

    public addPaymentRequest: AddPaymentRequest =
        {};

    public paymentTypes$: Subject<Array<PaymentTypeResponse>> =
        new ReplaySubject(1);

    public whenSubmittedForm$: Subject<NgForm> =
        new ReplaySubject(1);

    public months$: Subject<Array<{ id?: number, name: string }>> =
        new ReplaySubject(1);

    public years$: Subject<Array<{ id?: number, name: string }>> =
        new ReplaySubject(1);

    constructor(
        private paymentService: IPaymentService,
        private notificationService: INotificationService,
        routerService: IRouterService,
    ) {
        super(routerService);

        this.whenComponentInit$
            .pipe(
                switchMapTo(this.paymentService.getPaymentTypes()),
                takeUntil(this.whenComponentDestroy$),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) => {
                this.paymentTypes$.next([{
                    name: '',
                    systemName: '',
                }, ...result]);

                const currentDate = new Date();

                this.months$.next(months);
                this.years$.next(yearsRange(2019, currentDate.getFullYear() + 5));

                this.addPaymentRequest.month = currentDate.getMonth().toString();
                this.addPaymentRequest.year = currentDate.getFullYear();
            });

        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid),
                switchMap(_ => this.paymentService.addPayment(this.addPaymentRequest)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    } else {
                        this.notificationService.success('Payment was successfully added.');
                    }

                    return response.success;
                })
            )
            .subscribe(_ => this.routerService.navigateUp());
    }

    public onFormSubmit(form: NgForm): void {
        this.whenSubmittedForm$.next(form);
    }
}