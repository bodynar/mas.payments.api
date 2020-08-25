import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, switchMap, takeUntil } from 'rxjs/operators';

import BaseComponent from 'common/components/BaseComponent';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';

import { AddMeasurementTypeRequest } from 'models/request/measurement/addMeasurementTypeRequest';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';
import { IPaymentService } from 'services/IPaymentService';

@Component({
    templateUrl: 'addMeasurementType.template.pug'
})
export class AddMeasurementTypeComponent extends BaseComponent implements OnInit {
    public addMeasurementTypeRequest: AddMeasurementTypeRequest =
        {
            color: '#f04747'
        };

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

        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid),
                switchMap(_ => this.measurementService.addMeasurementType(this.addMeasurementTypeRequest)),
                filter(response => {
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
                })
            )
            .subscribe(({ result }) => this.paymentTypes$.next([{
                name: '',
                systemName: '',
            }, ...result]));
    }

    public onFormSubmit(form: NgForm): void {
        this.whenSubmittedForm$.next(form);
    }
}