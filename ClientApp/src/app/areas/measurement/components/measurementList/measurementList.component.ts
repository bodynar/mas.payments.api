import { Component, OnDestroy, OnInit } from '@angular/core';

import { BehaviorSubject, Subject } from 'rxjs';
import { filter, switchMap, takeUntil, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { yearsRange } from 'src/common/years';
import { months } from 'src/static/months';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';

import MeasurementsFilter from 'models/measurementsFilter';
import MeasurementsResponse from 'models/response/measurements/measurementsResponse';
import MeasurementTypeResponse from 'models/response/measurements/measurementTypeResponse';

@Component({
    templateUrl: 'measurementList.template.pug'
})
class MeasurementListComponent implements OnInit, OnDestroy {
    public filters: MeasurementsFilter =
        new MeasurementsFilter();

    public measurements$: Subject<Array<MeasurementsResponse>> =
        new Subject();

    public measurementTypes$: Subject<Array<MeasurementTypeResponse>> =
        new Subject();

    public isLoading$: Subject<boolean> =
        new BehaviorSubject(true);

    public isMeasurementsSentFlagActive$: Subject<boolean> =
        new BehaviorSubject(false);

    public isAnyMeasurementSelectedToSend$: Subject<boolean> =
        new BehaviorSubject(false);

    public selectedMeasurementsCount$: Subject<string> =
        new Subject();

    public isFilterApplied$: Subject<boolean> =
        new BehaviorSubject(false);

    public months: Array<{ name: string, id?: number }>;

    public years: Array<{ name: string, id?: number }>;

    public actions: Array<string> =
        ['add', 'types'];

    public isMeasurementsSentFlagVisible: boolean =
        false;

    private whenMeasurementDelete$: Subject<number> =
        new Subject();

    private whenMeasurementEdit$: Subject<number> =
        new Subject();

    private whenSubmitFilters$: Subject<null> =
        new Subject();

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    private onSendMeasurementsClick$: Subject<Array<number>>
        = new Subject();

    private selectedMeasurementsToSend: Array<number> =
        [];

    constructor(
        private measurementService: IMeasurementService,
        private routerService: IRouterService,
        private notificationService: INotificationService,
    ) {
        this.months = [{ name: '' }, ...months];
        this.years = [{ name: '' }, ...yearsRange(2019, new Date().getFullYear() + 5)];

        this.whenSubmitFilters$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                tap(_ => this.isLoading$.next(true)),
                tap(_ => {
                    this.filters.setIsEmpty();
                    this.isFilterApplied$.next(this.filters.isEmpty);
                }),
                switchMap(_ => this.measurementService.getMeasurements(this.filters)),
                tap(_ => this.isLoading$.next(false))
            )
            .subscribe(measurements => this.measurements$.next(measurements));

        this.whenMeasurementDelete$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(id => id !== 0),
                switchMap(id => this.measurementService.deleteMeasurement(id)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
                tap(_ => this.notificationService.success('Delete performed sucessfully'))
            )
            .subscribe(_ => this.whenSubmitFilters$.next(null));

        this.whenMeasurementEdit$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(id => id !== 0)
            )
            .subscribe(id =>
                this.routerService.navigateDeep(
                    ['update'],
                    { queryParams: { 'id': id } }
                )
            );

        this.onSendMeasurementsClick$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(array => array.length > 0 && !array.some(x => isNullOrUndefined(x) || x === 0)),
                tap(_ => {
                    this.isLoading$.next(true);
                    this.isMeasurementsSentFlagActive$.next(false);
                }),
                switchMap(array => this.measurementService.sendMeasurements(array)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return true;
                }),
                tap(_ => {
                    this.notificationService.success('Measurements sent');
                    this.isLoading$.next(false);
                }),
            )
            .subscribe(_ => this.whenSubmitFilters$.next(null));

        this.selectedMeasurementsCount$.next('');

        this.isMeasurementsSentFlagActive$
            .subscribe(isFlagVisible => this.isMeasurementsSentFlagVisible = isFlagVisible);
    }

    public ngOnInit(): void {
        this.measurementService
            .getMeasurementTypes()
            .pipe(takeUntil(this.whenComponentDestroy$))
            .subscribe(measurementTypes => {
                this.measurementTypes$.next([
                    {
                        name: '',
                        systemName: '',
                    }, ...measurementTypes
                ]);
                this.whenSubmitFilters$.next();
            });
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

    public onDeleteRecordClick(measurementId: number): void {
        this.whenMeasurementDelete$.next(measurementId);
    }

    public onEditRecordClick(measurementId: number): void {
        this.whenMeasurementEdit$.next(measurementId);
    }

    public onTypeClick(measurementTypeId: number): void {
        if (!isNullOrUndefined(measurementTypeId) && measurementTypeId !== 0) {
            this.filters.measurementTypeId = measurementTypeId;

            this.applyFilters();
        }
    }

    public onSendMeasurementsClick(): void {
        this.onSendMeasurementsClick$.next(this.selectedMeasurementsToSend);
    }

    public onSendFlagClick(measurement: {
        checked: boolean,
        id: number,
    }): void {
        if (measurement.checked) {
            this.selectedMeasurementsToSend.push(measurement.id);
        } else {
            this.selectedMeasurementsToSend.splice(
                this.selectedMeasurementsToSend.indexOf(measurement.id),
                1
            );
        }

        this.isAnyMeasurementSelectedToSend$.next(this.selectedMeasurementsToSend.length > 0);

        const count: string =
            this.selectedMeasurementsToSend.length > 0
                ? `(${this.selectedMeasurementsToSend.length})`
                : '';
        this.selectedMeasurementsCount$.next(count);
    }

    public clearFilters(): void {
        this.filters.clear();

        this.applyFilters();
    }

    public onSelectMeasurementsClick(isMeasurementsSentFlagVisible: boolean): void {
        this.isMeasurementsSentFlagActive$.next(isMeasurementsSentFlagVisible);

        if (!isMeasurementsSentFlagVisible) {
            this.selectedMeasurementsToSend = [];
            this.isAnyMeasurementSelectedToSend$.next(false);
            this.selectedMeasurementsCount$.next('');
        }
    }
}

export { MeasurementListComponent };