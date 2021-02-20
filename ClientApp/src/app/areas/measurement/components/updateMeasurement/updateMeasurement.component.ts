import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, switchMap, takeUntil, tap, switchMapTo, delay } from 'rxjs/operators';

import { isNullOrUndefined } from 'common/utils/common';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';

import BaseRoutingComponent from 'common/components/BaseRoutingComponent';

import { UpdateMeasurementRequest } from 'models/request/measurement';
import MeasurementTypeResponse from 'models/response/measurements/measurementTypeResponse';
import MonthYear from 'models/monthYearDate';

@Component({
    templateUrl: 'updateMeasurement.template.pug'
})
export class UpdateMeasurementComponent extends BaseRoutingComponent {

    public measurementRequest: UpdateMeasurementRequest =
        { date: MonthYear.fromToday(), measurement: 0, meterMeasurementTypeId: -1 };

    public isLoading: boolean =
        false;

    public measurementTypes: Array<MeasurementTypeResponse> =
        [{ name: '', systemName: '' }];

    public whenSubmittedForm$: Subject<NgForm> =
        new ReplaySubject(1);

    private measurementId: number;

    constructor(
        activatedRoute: ActivatedRoute,
        measurementService: IMeasurementService,
        notificationService: INotificationService,
        routerService: IRouterService,
    ) {
        super(routerService);

        this.whenComponentInit$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                switchMapTo(activatedRoute.queryParams),
                filter(params => !isNullOrUndefined(params['id']) && params['id'] > 0),
                tap(params => this.measurementId = params['id']),
                switchMap(_ => measurementService.getMeasurement(this.measurementId)),
                filter(response => {
                    if (!response.success) {
                        notificationService.error(response.error);
                    }
                    return response.success;
                }),
                tap(({ result }) => {
                    this.measurementRequest.comment = result.comment;
                    this.measurementRequest.measurement = result.measurement;
                    this.measurementRequest.meterMeasurementTypeId = result.meterMeasurementTypeId;

                    this.measurementRequest.date.set(result.date);
                }),
                switchMapTo(measurementService.getMeasurementTypes()),
                filter(response => {
                    if (!response.success) {
                        notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) => this.measurementTypes.push(...result));

        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid),
                switchMap(_ => {
                    this.isLoading = true;
                    return measurementService.updateMeasurement(this.measurementId, this.measurementRequest);
                }),
                delay(1.5 * 1000),
                filter(response => {
                    this.isLoading = false;
                    if (!response.success) {
                        notificationService.error(response.error);
                    } else {
                        notificationService.success('Measurement was successfully updated.');
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