import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, map, switchMap, takeUntil, tap, switchMapTo, delay } from 'rxjs/operators';

import { Year, yearsRange } from 'common/utils/years';
import { Month, months } from 'static/months';
import { isNullOrUndefined } from 'common/utils/common';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';

import BaseRoutingComponent from 'common/components/BaseRoutingComponent';

import { AddMeasurementRequest } from 'models/request/measurement/addMeasurementRequest';
import MeasurementTypeResponse from 'models/response/measurements/measurementTypeResponse';

@Component({
    templateUrl: 'updateMeasurement.template.pug'
})
export class UpdateMeasurementComponent extends BaseRoutingComponent {

    public measurementRequest: AddMeasurementRequest =
        {};

    public isLoading: boolean =
        false;

    public measurementTypes$: Subject<Array<MeasurementTypeResponse>> =
        new ReplaySubject(1);

    public whenSubmittedForm$: Subject<NgForm> =
        new ReplaySubject(1);

    public months: Array<Month> =
        months;

    public years: Array<Year> =
        yearsRange(2019);

    private measurementId: number;

    constructor(
        private activatedRoute: ActivatedRoute,
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
                }),
            )
            .subscribe(({ result }) => {
                this.measurementTypes$.next([{
                    name: '',
                    systemName: '',
                }, ...result]);
            });

        this.activatedRoute
            .queryParams
            .pipe(
                filter(params => !isNullOrUndefined(params['id']) && params['id'] !== 0),
                map(params => params['id']),
                tap(id => this.measurementId = id),
                switchMap(id => this.measurementService.getMeasurement(id)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) =>
                this.measurementRequest = {
                    year: result.year,
                    comment: result.comment,
                    measurement: result.measurement,
                    meterMeasurementTypeId: result.meterMeasurementTypeId,
                    month: (parseInt(result.month, 10) - 1).toString()
                }
            );

        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid),
                tap(_ => {
                    this.measurementRequest.month = (parseInt(this.measurementRequest.month, 10) + 1).toString();
                }),
                switchMap(_ => {
                    this.isLoading = true;
                    return this.measurementService.updateMeasurement(this.measurementId, this.measurementRequest);
                }),
                delay(1.5 * 1000),
                filter(response => {
                    this.isLoading = false;
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    } else {
                        this.notificationService.success('Measurement was successfully updated.');
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