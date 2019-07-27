import { Component, OnDestroy, OnInit } from '@angular/core';

import { BehaviorSubject, Subject } from 'rxjs';
import { delay, switchMap, takeUntil, tap } from 'rxjs/operators';

import { IMeasurementService } from 'services/IMeasurementService';
import { IRouterService } from 'services/IRouterService';

import { MeasurementsFilter } from 'models/measurementsFilter';
import { MeasurementResponse } from 'models/response/measurementResponse';
import { MeasurementTypeResponse } from 'models/response/measurementTypeResponse';

@Component({
    templateUrl: 'measurementList.template.pug',
    styleUrls: ['measurementList.style.styl']
})
class MeasurementListComponent implements OnInit, OnDestroy {
    public filters: MeasurementsFilter =
        {};

    public measurements$: Subject<Array<MeasurementResponse>> =
        new Subject();

    public measurementTypes$: Subject<Array<MeasurementTypeResponse>> =
        new Subject();

    public isLoading$: Subject<boolean> =
        new BehaviorSubject(true);

    public months: Array<{ name: string, id?: number }>;

    public actions: Array<string> =
        ['addMeasurement', 'addMeasurementType'];


    private whenSubmitFilters$: Subject<null> =
        new Subject();

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private measurementService: IMeasurementService,
        private routerService: IRouterService,
    ) {
        this.months = [
            {
                name: ''
            },
            {
                id: 1,
                name: 'January'
            },
            {
                id: 2,
                name: 'February'
            },
            {
                id: 3,
                name: 'March'
            },
            {
                id: 4,
                name: 'April'
            },
            {
                id: 5,
                name: 'May'
            },
            {
                id: 6,
                name: 'June'
            },
            {
                id: 7,
                name: 'July'
            },
            {
                id: 8,
                name: 'August'
            },
            {
                id: 9,
                name: 'September'
            },
            {
                id: 10,
                name: 'October'
            },
            {
                id: 11,
                name: 'November'
            },
            {
                id: 12,
                name: 'December'
            },
        ];

        this.whenSubmitFilters$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                tap(_ => this.isLoading$.next(true)),
                switchMap(_ => this.measurementService.getMeasurements(this.filters)),
                delay(2 * 1000), // todo: configure this value to UX
                tap(_ => this.isLoading$.next(false))
            )
            .subscribe(measurements => this.measurements$.next(measurements));

        this.whenSubmitFilters$.next();
    }

    public ngOnInit(): void {
        this.measurementService
            .getMeasurements()
            .pipe(takeUntil(this.whenComponentDestroy$))
            .subscribe(measurements => this.measurements$.next(measurements));

        this.measurementService
            .getMeasurementTypes()
            .pipe(takeUntil(this.whenComponentDestroy$))
            .subscribe(measurementTypes => this.measurementTypes$.next(measurementTypes));
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public onActionClick(actionName: string): void {
        this.routerService.navigateDeep([actionName]);
    }

    public applyFilters(): void {
        this.whenSubmitFilters$.next();
    }
}

export { MeasurementListComponent };