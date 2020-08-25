import { Component } from '@angular/core';

import { BehaviorSubject, of, ReplaySubject, Subject } from 'rxjs';
import { filter, map, switchMap, takeUntil, tap, switchMapTo } from 'rxjs/operators';

import { isNullOrUndefined } from 'common/utils/common';

import { yearsRange } from 'common/utils/years';
import { months } from 'static/months';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';
import { IModalService } from 'src/app/components/modal/IModalService';

import BaseRoutingComponent from 'common/components/BaseRoutingComponent';

import { getPaginatorConfig } from 'sharedComponents/paginator/paginator';
import PaginatorConfig from 'sharedComponents/paginator/paginatorConfig';

import MeasurementsFilter from 'models/measurementsFilter';
import MeasurementsResponse, { MeasurementsResponseMeasurement } from 'models/response/measurements/measurementsResponse';
import MeasurementTypeResponse from 'models/response/measurements/measurementTypeResponse';
import { ConfirmInModalComponent } from 'src/app/components/modal/components/confirm/confirm.component';

@Component({
    templateUrl: 'measurementList.template.pug'
})
export class MeasurementListComponent extends BaseRoutingComponent {
    public filters: MeasurementsFilter =
        new MeasurementsFilter();

    public paginatorConfig$: Subject<PaginatorConfig> =
        new ReplaySubject(1);

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

    public isMeasurementsSentFlagVisible: boolean =
        false;

    private whenMeasurementDelete$: Subject<number> =
        new Subject();

    private whenMeasurementEdit$: Subject<number> =
        new Subject();

    private whenSubmitFilters$: Subject<null> =
        new Subject();

    private onSendMeasurementsClick$: Subject<Array<{ id: number, isSent: boolean }>>
        = new Subject();

    private selectedMeasurementsToSend: Array<{ id: number, isSent: boolean }> =
        [];

    private measurements: Array<MeasurementsResponseMeasurement> =
        [];

    private measurementGroups: Array<MeasurementsResponse> =
        [];

    private pageSize: number =
        3;

    constructor(
        private measurementService: IMeasurementService,
        private notificationService: INotificationService,
        private modalService: IModalService,
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
            .subscribe(({ result }) => {
                this.measurementTypes$.next([
                    {
                        name: '',
                        systemName: '',
                    }, ...result
                ]);
                this.whenSubmitFilters$.next();
            });

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
                tap(_ => this.isLoading$.next(false)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }

                    return response.success;
                })
            )
            .subscribe(({ result }) => {
                this.measurements = [].concat(...result.map(x => x.measurements));
                this.measurementGroups = result;

                const paginatorConfig: PaginatorConfig =
                    getPaginatorConfig(this.measurementGroups, this.pageSize);

                if (paginatorConfig.enabled) {
                    this.onPageChange(0);
                } else {
                    this.measurements$.next(this.measurementGroups);
                }

                this.paginatorConfig$.next(paginatorConfig);
            });

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
                    { queryParams: { id } }
                )
            );

        this.onSendMeasurementsClick$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(array => array.length > 0 && !array.some(x => isNullOrUndefined(x) || x.id === 0)),
                switchMap(array => {
                    const hasAlreadySentItems: boolean =
                        array.some(x => x.isSent);

                    if (hasAlreadySentItems) {
                        return this.modalService.show(ConfirmInModalComponent, {
                            size: 'medium',
                            title: 'Measurements already sent',
                            body: {
                                content: 'Some of measurements is already sent.\nDo you want to send them again?',
                                isHtml: false,
                            },
                            additionalParameters: {
                                confirmBtnText: 'Yes',
                                cancelBtnText: 'No',
                            }
                        }).pipe(
                            map(response => response as boolean),
                            map(filterValue => ({ array, filterValue }))
                        );
                    } else {
                        return of({ array, filterValue: true });
                    }
                }),
                filter(({ filterValue }) => filterValue),
                tap(_ => {
                    this.isLoading$.next(true);
                    this.isMeasurementsSentFlagActive$.next(false);
                    this.isAnyMeasurementSelectedToSend$.next(false);
                }),
                map(({ array }) => array.map(x => x.id)),
                switchMap(array => this.measurementService.sendMeasurements(array)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    } else {
                        this.notificationService.success('Measurements sent');
                    }
                    return true;
                }),
                tap(_ => {
                    this.isLoading$.next(false);
                }),
            )
            .subscribe(_ => this.whenSubmitFilters$.next(null));

        this.selectedMeasurementsCount$.next('');

        this.isMeasurementsSentFlagActive$
            .subscribe(isFlagVisible => this.isMeasurementsSentFlagVisible = isFlagVisible);
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
        const passedMeasurement: MeasurementsResponseMeasurement =
            this.measurements.filter(x => x.id === measurement.id).pop();

        const addedMeasurement: {
            isSent: boolean,
            id: number,
        } = this.selectedMeasurementsToSend.filter(x => x.id === measurement.id).pop();

        if (measurement.checked && isNullOrUndefined(addedMeasurement)) {
            this.selectedMeasurementsToSend.push({
                id: measurement.id,
                isSent: passedMeasurement.isSent
            });
        } else {
            if (!isNullOrUndefined(addedMeasurement)) {
                this.selectedMeasurementsToSend.splice(
                    this.selectedMeasurementsToSend.indexOf(addedMeasurement),
                    1
                );
            }
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

    public onPageChange(pageNumber: number): void {
        const slicedItems: Array<MeasurementsResponse> =
            this.measurementGroups.slice(this.pageSize * pageNumber, (pageNumber + 1) * this.pageSize);

        this.measurements$.next(slicedItems);
    }
}