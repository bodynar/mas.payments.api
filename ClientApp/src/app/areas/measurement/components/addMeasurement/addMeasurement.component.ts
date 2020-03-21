import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, switchMap, takeUntil } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';
import { months } from 'src/static/months';
import { years } from 'src/common/years';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';

import { AddMeasurementRequest } from 'models/request/addMeasurementRequest';
import { MeasurementTypeResponse } from 'models/response/measurementTypeResponse';

@Component({
    templateUrl: 'addMeasurement.template.pug'
})
class AddMeasurementComponent implements OnInit, OnDestroy {

    public addMeasurementRequest: AddMeasurementRequest =
      {};

    public measurementTypes$: Subject<Array<MeasurementTypeResponse>> =
      new ReplaySubject(1);

    public months$: Subject<Array<{ id?: number, name: string }>> =
      new ReplaySubject(1);

    public years$: Subject<Array<{ id?: number, name: string }>> =
      new ReplaySubject(1);

    public whenSubmittedForm$: Subject<NgForm> =
        new ReplaySubject(1);

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private measurementService: IMeasurementService,
        private notificationService: INotificationService,
        private routerService: IRouterService,
    ) {
        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid && this.isFormValid()),
                switchMap(_ => this.measurementService.addMeasurement(this.addMeasurementRequest)),
                filter(withoutError => {
                    if (!withoutError) {
                        this.notificationService.error('Error due saving data. Please, try again later');
                    } else {
                        this.notificationService.success('Measurement was successfully added.');
                    }

                    return withoutError;
                })
            )
            .subscribe(_ => this.routerService.navigateUp());
    }

    public ngOnInit(): void {
        this.measurementService
            .getMeasurementTypes()
            .pipe(takeUntil(this.whenComponentDestroy$))
            .subscribe(measurementTypes => this.measurementTypes$.next([{
                name: '',
                systemName: '',
            }, ...measurementTypes]));

      const currentDate = new Date();

      this.months$.next(months);
      this.years$.next(years(currentDate.getFullYear() - 40, currentDate.getFullYear() + 40));

      this.addMeasurementRequest.month = currentDate.getMonth().toString();
      this.addMeasurementRequest.year = currentDate.getFullYear();
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public onFormSubmit(form: NgForm): void {
        this.whenSubmittedForm$.next(form);
    }

    private isFormValid(): boolean {
        // todo: fix => make normal validator on field
        let isFormValid: boolean =
            true;

        if (!isNullOrUndefined(this.addMeasurementRequest.measurement)) {
            const measurementValue: number =
                parseFloat(`${this.addMeasurementRequest.measurement}`);

            if (Number.isNaN(measurementValue)) {
                isFormValid = false;
            }
        }
        return isFormValid;
    }
}

export { AddMeasurementComponent };