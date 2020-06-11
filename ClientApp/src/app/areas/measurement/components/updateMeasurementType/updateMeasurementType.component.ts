import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, map, switchMap, takeUntil, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';
import { IRouterService } from 'services/IRouterService';

import { AddMeasurementTypeRequest } from 'models/request/measurement/addMeasurementTypeRequest';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';

@Component({
    templateUrl: 'updateMeasurementType.template.pug'
})
class UpdateMeasurementTypeComponent implements OnInit, OnDestroy {

    public measurementTypeRequest: AddMeasurementTypeRequest =
        {};

    public paymentTypes$: Subject<Array<PaymentTypeResponse>> =
        new ReplaySubject(1);

    public whenSubmittedForm$: Subject<NgForm> =
        new ReplaySubject(1);


    private measurementTypeId: number;

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private activatedRoute: ActivatedRoute,
        private measurementService: IMeasurementService,
        private paymentService: IPaymentService,
        private notificationService: INotificationService,
        private routerService: IRouterService,
    ) {
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
                switchMap(_ => this.measurementService.updateMeasurementType(this.measurementTypeId, this.measurementTypeRequest)),
                filter(response => {
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
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public onFormSubmit(form: NgForm): void {
        this.whenSubmittedForm$.next(form);
    }
}

export { UpdateMeasurementTypeComponent };