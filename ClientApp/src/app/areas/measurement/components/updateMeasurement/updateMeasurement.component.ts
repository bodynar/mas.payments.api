import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, map, switchMap, takeUntil, tap } from 'rxjs/operators';

import { yearsRange } from 'common/utils/years';
import { months } from 'static/months';
import { isNullOrUndefined } from 'common/utils/common';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';

import BaseComponent from 'common/components/BaseComponent';

import { AddMeasurementRequest } from 'models/request/measurement/addMeasurementRequest';
import MeasurementTypeResponse from 'models/response/measurements/measurementTypeResponse';

@Component({
    templateUrl: 'updateMeasurement.template.pug'
})
class UpdateMeasurementComponent extends BaseComponent implements OnInit {

    public measurementRequest: AddMeasurementRequest =
        {};

    public measurementTypes$: Subject<Array<MeasurementTypeResponse>> =
        new ReplaySubject(1);

    public whenSubmittedForm$: Subject<NgForm> =
        new ReplaySubject(1);

    public months$: Subject<Array<{ id?: number, name: string }>> =
        new ReplaySubject(1);

    public years$: Subject<Array<{ id?: number, name: string }>> =
        new ReplaySubject(1);


    private measurementId: number;

    constructor(
        private activatedRoute: ActivatedRoute,
        private measurementService: IMeasurementService,
        private notificationService: INotificationService,
        routerService: IRouterService,
    ) {
        super(routerService);
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
                    month: (parseInt(result.month) - 1).toString()
                }
            );

        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid),
                tap(_ => {
                    this.measurementRequest.month = (parseInt(this.measurementRequest.month) + 1).toString();
                }),
                switchMap(_ => this.measurementService.updateMeasurement(this.measurementId, this.measurementRequest)),
                filter(response => {
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

    public ngOnInit(): void {
        this.measurementService
            .getMeasurementTypes()
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) => this.measurementTypes$.next([{
                name: '',
                systemName: '',
            }, ...result]));

        const currentDate = new Date();

        this.months$.next(months);
        this.years$.next(yearsRange(2019, currentDate.getFullYear() + 5));
    }

    public onFormSubmit(form: NgForm): void {
        this.whenSubmittedForm$.next(form);
    }
}

export { UpdateMeasurementComponent };