import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, switchMap, takeUntil, switchMapTo, delay } from 'rxjs/operators';

import BaseRoutingComponent from 'common/components/BaseRoutingComponent';

import { generateGuid } from 'common/utils/common';
import { Color } from 'common/utils/colors';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';

import { AddMeasurementRequest } from 'models/request/measurement';
import { MeasurementTypeResponse } from 'models/response/measurements';
import { MonthSelectorValue } from 'common/components/monthSelector/monthSelector.component';


// TODO: Fix binding when add\delete array of items
// items being nulling values
// - Add label to date
@Component({
    templateUrl: 'addMeasurement.template.pug',
    styleUrls: ['addMeasurement.style.styl']
})
export class AddMeasurementComponent extends BaseRoutingComponent {
    public currentDate: Date =
        new Date();

    public actionColors: Array<Color> =
        [{ red: 183, green: 223, blue: 105 }, { red: 255, green: 111, blue: 94 }];

    public dateInitialValue: MonthSelectorValue =
        { month: this.currentDate.getMonth(), year: this.currentDate.getFullYear() };

    public addMeasurementRequest: AddMeasurementRequest =
        { date: new Date(), measurements: [] };

    public isLoading: boolean =
        false;

    public measurementTypes: Array<MeasurementTypeResponse> =
        [{ id: -1, name: '', systemName: '' }];

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
                filter(({ valid }) => valid),
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

    public onDateChange(dateValue: MonthSelectorValue): void {
        this.addMeasurementRequest.date = new Date(dateValue.year, dateValue.month, 20);
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
}