import { Component, OnDestroy, OnInit } from '@angular/core';

import { BehaviorSubject, Subject, ReplaySubject } from 'rxjs';
import { delay, filter, switchMap, switchMapTo, takeUntil, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { months } from 'src/static/months';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
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

    public isMeasurementsSentFlagActive$: Subject<boolean> =
        new BehaviorSubject(false);

    public selectedMeasurementsCount$: Subject<string> =
        new Subject();

    public months: Array<{ name: string, id?: number }>;

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

    private selectedMeasurementsToSend: Array<number> =
        [];

    constructor(
        private measurementService: IMeasurementService,
        private routerService: IRouterService,
        private notificationService: INotificationService,
    ) {
        this.months = [{ name: '' }, ...months];

        this.whenSubmitFilters$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                tap(_ => this.isLoading$.next(true)),
                switchMap(_ => this.measurementService.getMeasurements(this.filters)),
                delay(2 * 1000), // todo: configure this value to UX
                tap(_ => this.isLoading$.next(false))
            )
            .subscribe(measurements => this.measurements$.next(measurements));

        this.whenMeasurementDelete$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(id => id !== 0),
                switchMap(id => this.measurementService.deleteMeasurement(id)),
                filter(hasError => {
                    if (hasError) {
                        this.notificationService.error('Error due deleting type. Try again later');
                    }
                    return !hasError;
                }),
                switchMapTo(this.measurementService.getMeasurements(this.filters)),
                tap(_ => this.notificationService.success('Delete performed sucessfully'))
            )
            .subscribe(measurements => this.measurements$.next(measurements));

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
        this.applyFilters();
    }

    public onSendFlagClick(event: {
        checked: boolean,
        id: number,
    }): void {
        if (event.checked) {
            this.selectedMeasurementsToSend.push(event.id);
        } else {
            this.selectedMeasurementsToSend.splice(
                this.selectedMeasurementsToSend.indexOf(event.id),
                1
            );
        }
        const count: string =
            this.selectedMeasurementsToSend.length > 0
                ? `(${this.selectedMeasurementsToSend.length})`
                : '';
        this.selectedMeasurementsCount$.next(count);
    }

    public clearFilters(): void {
        this.filters = {};

        this.applyFilters();
    }

    public onSelectMeasurementsClick(isMeasurementsSentFlagVisible: boolean): void {
        this.isMeasurementsSentFlagActive$.next(isMeasurementsSentFlagVisible);

        if (!isMeasurementsSentFlagVisible) {
            this.selectedMeasurementsToSend = [];
        }
    }
}

export { MeasurementListComponent };