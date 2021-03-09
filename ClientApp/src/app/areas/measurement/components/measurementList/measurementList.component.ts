import { Component } from '@angular/core';

import { Subject } from 'rxjs';
import { filter, map, switchMap, takeUntil, tap, delay } from 'rxjs/operators';

import { isNullOrUndefined } from 'common/utils/common';

import { emptyYear, Year, yearsRange } from 'common/utils/years';
import { emptyMonth, Month, months } from 'static/months';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';
import { IModalService } from 'src/app/components/modal/IModalService';

import { BaseRoutingComponentWithModalComponent } from 'common/components/BaseComponentWithModal';

import { getPaginatorConfig } from 'sharedComponents/paginator/paginator';
import PaginatorConfig from 'sharedComponents/paginator/paginatorConfig';

import MeasurementsFilter from 'models/request/measurement/measurementsFilter';
import { MeasurementsResponse, MeasurementsResponseMeasurement, MeasurementTypeResponse } from 'models/response/measurements';

@Component({
    templateUrl: 'measurementList.template.pug'
})
export class MeasurementListComponent extends BaseRoutingComponentWithModalComponent {
    public filters: MeasurementsFilter =
        new MeasurementsFilter();

    public paginatorConfig: PaginatorConfig;

    public pageItems: Array<MeasurementsResponse> = [];
    public measurementTypes: Array<MeasurementTypeResponse> = [];

    public hasData: boolean = false;
    public isLoading: boolean = false;
    public isMeasurementsSentFlagActive: boolean = false;
    public isAnyMeasurementSelectedToSend: boolean = false;
    public isFilterApplied: boolean = false;
    public selectedMeasurementsCount: string = ''; // TODO: switch to number and figure out about template format

    public months: Array<Month> =
        [emptyMonth, ...months];

    public years: Array<Year> =
        [emptyYear, ...yearsRange(2019, new Date().getFullYear() + 5)];

    // #region private members

    private whenMeasurementDelete$: Subject<number> =
        new Subject();

    private whenMeasurementEdit$: Subject<number> =
        new Subject();

    private whenSubmitFilters$: Subject<null> =
        new Subject();

    private onSendMeasurementsClick$: Subject<Array<{ id: number, isSent: boolean }>>
        = new Subject();

    private selectedMeasurementsToSend: Array<{ id: number, isSent: boolean }> = [];
    private measurementGroups: Array<MeasurementsResponse> = [];
    private measurements: Array<MeasurementsResponseMeasurement> = [];

    private pageSize: number = 3;

    // #endregion

    constructor(
        private measurementService: IMeasurementService,
        private notificationService: INotificationService,
        modalService: IModalService,
        routerService: IRouterService,
    ) {
        super(routerService, modalService);

        this.whenComponentInit$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                switchMap(() => {
                    this.isLoading = true;
                    return this.measurementService.getMeasurementTypes();
                }),
                delay(1.5 * 1000),
                filter(response => {
                    this.isLoading = false;
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }

                    return response.success;
                })
            )
            .subscribe(({ result }) => {
                this.measurementTypes = [{ name: '', systemName: '', }, ...result];
                this.whenSubmitFilters$.next();
            });

        this.whenSubmitFilters$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                tap(_ => {
                    this.isLoading = true;
                    this.filters.setIsEmpty();
                    this.isFilterApplied = this.filters.isEmpty;
                }),
                switchMap(_ => this.measurementService.getMeasurements(this.filters)),
                delay(1.5 * 1000),
                filter(response => {
                    this.isLoading = false;
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
                    this.pageItems = this.measurementGroups;
                }

                this.hasData = this.measurements.length !== 0;
                this.paginatorConfig = paginatorConfig;
            });

        this.whenMeasurementDelete$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(id => id !== 0),
                switchMap(id =>
                    this.confirmDelete()
                        .pipe(
                            filter(isConfirm => isConfirm),
                            map(_ => id)
                        )
                ),
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

                    const confirmMessage: string =
                        hasAlreadySentItems
                            ? 'Some of measurements is already sent.\nDo you want to send them again?'
                            : 'Are you sure want to selected measurements?';

                    return this.confirmInModal('Confirm sending', confirmMessage)
                        .pipe(map(isConfirm => ({ array, isConfirm })));
                }),
                filter(({ isConfirm }) => isConfirm),
                map(({ array }) => array.map(x => x.id)),
                switchMap(array => {
                    this.isLoading = true;
                    return this.measurementService.sendMeasurements(array);
                }),
                delay(1.5 * 1000),
                filter(response => {
                    this.isLoading = false;
                    this.isMeasurementsSentFlagActive = false;
                    this.isAnyMeasurementSelectedToSend = false;

                    if (!response.success) {
                        this.notificationService.error(response.error);
                    } else {
                        this.notificationService.success('Measurements sent');
                    }
                    return true;
                }),
            )
            .subscribe(_ => this.whenSubmitFilters$.next(null));
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

        this.isAnyMeasurementSelectedToSend = this.selectedMeasurementsToSend.length > 0;
        this.selectedMeasurementsCount = this.isAnyMeasurementSelectedToSend ? `(${this.selectedMeasurementsToSend.length})` : '';
    }

    public clearFilters(): void {
        this.filters.clear();

        this.applyFilters();
    }

    public onSelectMeasurementsClick(isMeasurementsSentFlagVisible: boolean): void {
        this.isMeasurementsSentFlagActive = isMeasurementsSentFlagVisible;

        if (!isMeasurementsSentFlagVisible) {
            this.selectedMeasurementsToSend = [];
            this.isAnyMeasurementSelectedToSend = false;
            this.selectedMeasurementsCount = '';
        }
    }

    public onPageChange(pageNumber: number): void {
        const slicedItems: Array<MeasurementsResponse> =
            this.measurementGroups.slice(this.pageSize * pageNumber, (pageNumber + 1) * this.pageSize);

        this.pageItems = slicedItems;
    }
}