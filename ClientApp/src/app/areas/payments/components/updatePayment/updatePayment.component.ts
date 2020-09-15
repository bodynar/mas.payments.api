import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';
import { filter, map, switchMap, takeUntil, tap, switchMapTo, delay } from 'rxjs/operators';

import { yearsRange } from 'common/utils/years';
import { months } from 'static/months';
import { isNullOrUndefined } from 'common/utils/common';

import BaseRoutingComponent from 'common/components/BaseRoutingComponent';

import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';
import { IRouterService } from 'services/IRouterService';

import { AddPaymentRequest } from 'models/request/payment/addPaymentRequest';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';

@Component({
    templateUrl: 'updatePayment.template.pug'
})
export class UpdatePaymentComponent extends BaseRoutingComponent {

    public paymentRequest: AddPaymentRequest =
        {};

    public isLoading$: Subject<boolean> =
        new BehaviorSubject(false);

    public paymentTypes$: Subject<Array<PaymentTypeResponse>> =
        new ReplaySubject(1);

    public whenSubmittedForm$: Subject<NgForm> =
        new ReplaySubject(1);

    public months$: Subject<Array<{ id?: number, name: string }>> =
        new ReplaySubject(1);

    public years$: Subject<Array<{ id?: number, name: string }>> =
        new ReplaySubject(1);

    private paymentId: number;

    constructor(
        private activatedRoute: ActivatedRoute,
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
            });

        this.activatedRoute
            .queryParams
            .pipe(
                filter(params => !isNullOrUndefined(params['id']) && params['id'] !== 0),
                map(params => params['id']),
                tap(id => this.paymentId = id),
                switchMap(id => this.paymentService.getPayment(id)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) => {
                this.paymentRequest = {
                    amount: result.amount,
                    description: result.description,
                    paymentTypeId: result.paymentTypeId,
                    year: result.year,
                    month: (parseInt(result.month) - 1).toString()
                };
            });

        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid),
                tap(_ => {
                    this.paymentRequest.month = (parseInt(this.paymentRequest.month) + 1).toString();
                }),
                switchMap(_ => {
                    this.isLoading$.next(true);
                    return this.paymentService.updatePayment(this.paymentId, this.paymentRequest);
                }),
                delay(1.5 * 1000),
                filter(response => {
                    this.isLoading$.next(false);
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    } else {
                        this.notificationService.success('Payment was successfully updated.');
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