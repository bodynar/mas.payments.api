import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, switchMap, takeUntil, switchMapTo, delay } from 'rxjs/operators';

import BaseRoutingComponent from 'common/components/BaseRoutingComponent';

import { generateGuid, isNullOrUndefined } from 'common/utils/common';
import { Color } from 'common/utils/colors';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';

import { AddMeasurementsModel, MeasurementModel } from 'models/measurement/addMeasurement';
import { MeasurementTypeResponse } from 'models/response/measurements';
import MonthYear from 'models/monthYearDate';


@Component({
    templateUrl: 'addMeasurement.template.pug',
    styleUrls: ['addMeasurement.style.styl']
})
export class AddMeasurementComponent extends BaseRoutingComponent {
    public actionColors: Array<Color> =
        [{ red: 183, green: 223, blue: 105 }, { red: 255, green: 111, blue: 94 }];

    public addMeasurementRequest: AddMeasurementsModel =
        {
            date: new MonthYear(),
            measurements: [{
                id: generateGuid(),
                measurement: 0,
                measurementTypeId: -1
            }]
        };

    public isLoading: boolean =
        false;

    public measurementTypes: Array<MeasurementTypeResponse> =
        [{ id: undefined, name: '', systemName: '' }];

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
            .subscribe(({ result }) => this.measurementTypes.push(...result));

        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(_ => this.validateMeasurements(this.addMeasurementRequest.measurements)),
                switchMap(_ => {
                    this.isLoading = true;
                    return this.measurementService.addMeasurement(this.addMeasurementRequest);
                }),
                delay(1.5 * 1000),
                filter(response => {
                    this.isLoading = false;
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

    public onMeasurementAdd(): void {
        this.addMeasurementRequest.measurements.push({
            id: generateGuid(),
            measurement: 0,
            measurementTypeId: -1
        });
    }

    public onMeasurementRemove(id: string): void {
        const index: number =
            this.addMeasurementRequest.measurements.findIndex(x => x.id === id);

        if (index >= 0) {
            this.addMeasurementRequest.measurements.splice(index, 1);
        }

    }

    private validateMeasurements(measurements: Array<MeasurementModel>): boolean {
        measurements.forEach(item => {
            item.isValid =
                !isNullOrUndefined(item.measurementTypeId)
                && +item.measurementTypeId !== -1
                && !isNullOrUndefined(item.measurement)
                && item.measurement > 0;
        });

        return measurements.every(x => x.isValid);
    }
}