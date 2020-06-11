import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, switchMap, takeUntil } from 'rxjs/operators';

import { yearsRange } from 'src/common/years';
import { months } from 'src/static/months';
import { isNullOrUndefined } from 'util';

import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';
import { IRouterService } from 'services/IRouterService';

import { AddPaymentRequest } from 'models/request/payment/addPaymentRequest';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';

@Component({
    templateUrl: 'addPayment.template.pug'
})
class AddPaymentComponent implements OnInit, OnDestroy {

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

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private paymentService: IPaymentService,
        private routerService: IRouterService,
        private notificationService: INotificationService,
    ) {
        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid && this.isFormValid(this.addPaymentRequest)),
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


    public ngOnInit(): void {
        this.paymentService
            .getPaymentTypes()
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) => this.paymentTypes$.next([{
                name: '',
                systemName: '',
            }, ...result]));

        const currentDate = new Date();

        this.months$.next(months);
        this.years$.next(yearsRange(2019, currentDate.getFullYear() + 5));

        this.addPaymentRequest.month = currentDate.getMonth().toString();
        this.addPaymentRequest.year = currentDate.getFullYear();
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public onFormSubmit(form: NgForm): void {
        this.whenSubmittedForm$.next(form);
    }

    private isFormValid(value: AddPaymentRequest): boolean {
        let isFormValid: boolean =
            true;

        if (!isNullOrUndefined(value.amount)) {
            const measurementValue: number =
                parseFloat(`${value.amount}`);

            if (Number.isNaN(measurementValue)) {
                isFormValid = false;
            }
        }

        return isFormValid;
    }
}

export { AddPaymentComponent };