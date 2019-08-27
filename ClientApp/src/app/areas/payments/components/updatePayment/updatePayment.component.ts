import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, map, switchMap, takeUntil, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';
import { IRouterService } from 'services/IRouterService';

import { AddPaymentRequest } from 'models/request/addPaymentRequest';
import { PaymentTypeResponse } from 'models/response/paymentTypeResponse';

@Component({
    templateUrl: 'updatePayment.template.pug'
})
class UpdatePaymentComponent implements OnInit, OnDestroy {

    public paymentRequest: AddPaymentRequest =
        {};

    public paymentTypes$: Subject<Array<PaymentTypeResponse>> =
        new ReplaySubject(1);

    public whenSubmittedForm$: Subject<NgForm> =
        new ReplaySubject(1);

    private paymentId: number;

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private activatedRoute: ActivatedRoute,
        private paymentService: IPaymentService,
        private routerService: IRouterService,
        private notificationService: INotificationService,
    ) {
        this.activatedRoute
            .queryParams
            .pipe(
                filter(params => !isNullOrUndefined(params['id']) && params['id'] !== 0),
                map(params => params['id']),
                tap(id => this.paymentId = id),
                switchMap(id => this.paymentService.getPayment(id))
            )
            .subscribe(params => this.paymentRequest = params);

        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid && this.isFormValid(this.paymentRequest)),
                switchMap(_ => this.paymentService.updatePayment(this.paymentId, this.paymentRequest)),
                filter(withoutError => {
                    if (!withoutError) {
                        this.notificationService.error('Error due saving data. Please, try again later');
                    } else {
                        this.notificationService.success('Payment was successfully updated.');
                    }

                    return withoutError;
                })
            )
            .subscribe(_ => this.routerService.navigateUp());
    }


    public ngOnInit(): void {
        this.paymentService
            .getPaymentTypes()
            .pipe(takeUntil(this.whenComponentDestroy$))
            .subscribe(paymentTypes => this.paymentTypes$.next([{
                name: '',
                systemName: '',
            }, ...paymentTypes]));
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

export { UpdatePaymentComponent };