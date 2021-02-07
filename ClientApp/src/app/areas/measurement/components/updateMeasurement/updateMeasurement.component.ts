import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, map, switchMap, takeUntil, tap, switchMapTo, delay } from 'rxjs/operators';

import { isNullOrUndefined } from 'common/utils/common';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';

import BaseRoutingComponent from 'common/components/BaseRoutingComponent';

import { UpdateMeasurementRequest } from 'models/request/measurement';
import MeasurementTypeResponse from 'models/response/measurements/measurementTypeResponse';
import { MonthSelectorValue } from 'common/components/monthSelector/monthSelector.component';

@Component({
    templateUrl: 'updateMeasurement.template.pug'
})
export class UpdateMeasurementComponent extends BaseRoutingComponent {

    public measurementRequest: UpdateMeasurementRequest;
    public dateInitialValue: MonthSelectorValue;

    public isLoading: boolean =
        false;

    public measurementTypes: Array<MeasurementTypeResponse> =
        [{ name: '', systemName: '' }];

    public whenSubmittedForm$: Subject<NgForm> =
        new ReplaySubject(1);

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
            .subscribe(({ result }) => this.measurementTypes.push(...result));

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
            .subscribe(({ result }) => {
                this.measurementRequest = { ...result };
                this.dateInitialValue = {
                    month: result.date.getMonth(),
                    year: result.date.getFullYear()
                };
            });

        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid),
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

    public onDateChange(value: MonthSelectorValue): void {
        this.measurementRequest.date = new Date(value.year, value.month);
    }

    public onFormSubmit(form: NgForm): void {
        this.whenSubmittedForm$.next(form);
    }
}