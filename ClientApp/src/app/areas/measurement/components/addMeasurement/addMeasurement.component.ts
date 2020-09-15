import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';

import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';
import { filter, switchMap, takeUntil, switchMapTo, delay } from 'rxjs/operators';

import { yearsRange } from 'common/utils/years';
import { months } from 'static/months';

import BaseRoutingComponent from 'common/components/BaseRoutingComponent';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';

import { AddMeasurementRequest } from 'models/request/measurement/addMeasurementRequest';
import MeasurementTypeResponse from 'models/response/measurements/measurementTypeResponse';

@Component({
    templateUrl: 'addMeasurement.template.pug'
})
export class AddMeasurementComponent extends BaseRoutingComponent {

    public addMeasurementRequest: AddMeasurementRequest =
        {};

    public isLoading$: Subject<boolean> =
        new BehaviorSubject(false);

    public measurementTypes$: Subject<Array<MeasurementTypeResponse>> =
        new ReplaySubject(1);

    public months$: Subject<Array<{ id?: number, name: string }>> =
        new ReplaySubject(1);

    public years$: Subject<Array<{ id?: number, name: string }>> =
        new ReplaySubject(1);

    public whenSubmittedForm$: Subject<NgForm> =
        new ReplaySubject(1);

    constructor(
        private measurementService: IMeasurementService,
        private notificationService: INotificationService,
        routerService: IRouterService,
    ) {
        super(routerService);

        this.whenComponentInit$
            .pipe(
                switchMapTo(this.measurementService.getMeasurementTypes()),
                takeUntil(this.whenComponentDestroy$),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }

                    return response.success;
                })
            )
            .subscribe(({ result }) => {
                this.measurementTypes$.next([{
                    name: '',
                    systemName: '',
                }, ...result]);

                const currentDate: Date =
                    new Date();

                this.months$.next(months);
                this.years$.next(yearsRange(2019, currentDate.getFullYear() + 5));

                this.addMeasurementRequest.month = currentDate.getMonth().toString();
                this.addMeasurementRequest.year = currentDate.getFullYear();
            });

        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid),
                switchMap(_ => {
                    this.isLoading$.next(true);
                    return this.measurementService.addMeasurement(this.addMeasurementRequest);
                }),
                delay(1.5 * 1000),
                filter(response => {
                    this.isLoading$.next(false);
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    } else {
                        this.notificationService.success('Measurement was successfully added.');
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