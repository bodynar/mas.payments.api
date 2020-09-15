import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';
import { filter, map, switchMap, takeUntil, tap, switchMapTo, delay } from 'rxjs/operators';

import { isNullOrUndefined } from 'common/utils/common';

import BaseRoutingComponent from 'common/components/BaseRoutingComponent';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';
import { IRouterService } from 'services/IRouterService';

import { AddMeasurementTypeRequest } from 'models/request/measurement/addMeasurementTypeRequest';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';

@Component({
    templateUrl: 'updateMeasurementType.template.pug'
})
export class UpdateMeasurementTypeComponent extends BaseRoutingComponent {

    public measurementTypeRequest: AddMeasurementTypeRequest =
        {};

    public isLoading$: Subject<boolean> =
        new BehaviorSubject(false);

    public paymentTypes$: Subject<Array<PaymentTypeResponse>> =
        new ReplaySubject(1);

    public whenSubmittedForm$: Subject<NgForm> =
        new ReplaySubject(1);


    private measurementTypeId: number;

    constructor(
        private activatedRoute: ActivatedRoute,
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
                }),
            )
            .subscribe(({ result }) => this.paymentTypes$.next([{
                name: '',
                systemName: '',
            }, ...result]));

        this.activatedRoute
            .queryParams
            .pipe(
                filter(params => !isNullOrUndefined(params['id']) && params['id'] !== 0),
                map(params => params['id']),
                tap(id => this.measurementTypeId = id),
                switchMap(id => this.measurementService.getMeasurementType(id)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) => this.measurementTypeRequest = result);

        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid),
                switchMap(_ => {
                    this.isLoading$.next(true);
                    return this.measurementService.updateMeasurementType(this.measurementTypeId, this.measurementTypeRequest);
                }),
                delay(1.5 * 1000),
                filter(response => {
                    this.isLoading$.next(false);
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    } else {
                        this.notificationService.success('Measurement type was successfully updated.');
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