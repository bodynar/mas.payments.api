import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';

import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';
import { filter, switchMap, takeUntil, switchMapTo, delay } from 'rxjs/operators';

import BaseRoutingComponent from 'common/components/BaseRoutingComponent';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';

import { AddMeasurementTypeRequest } from 'models/request/measurement/addMeasurementTypeRequest';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';
import { IPaymentService } from 'services/IPaymentService';

@Component({
    templateUrl: 'addMeasurementType.template.pug'
})
export class AddMeasurementTypeComponent extends BaseRoutingComponent {
    public addMeasurementTypeRequest: AddMeasurementTypeRequest =
        {
            color: '#f04747'
        };

    public isLoading$: Subject<boolean> =
        new BehaviorSubject(false);

    public paymentTypes$: Subject<Array<PaymentTypeResponse>> =
        new ReplaySubject(1);

    public whenSubmittedForm$: Subject<NgForm> =
        new ReplaySubject(1);

    constructor(
        private measurementService: IMeasurementService,
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
                })
            )
            .subscribe(({ result }) => this.paymentTypes$.next([{
                name: '',
                systemName: '',
            }, ...result]));

        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid),
                switchMap(_ => {
                    this.isLoading$.next(true);
                    return this.measurementService.addMeasurementType(this.addMeasurementTypeRequest);
                }),
                delay(1.5 * 1000),
                filter(response => {
                    this.isLoading$.next(false);
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    } else {
                        this.notificationService.success('Measurement type was successfully added.');
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