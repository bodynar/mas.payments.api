import { Component, OnDestroy, OnInit } from '@angular/core';

import { Subject } from 'rxjs';
import { filter, switchMap, switchMapTo, takeUntil, tap } from 'rxjs/operators';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';

import MeasurementTypeResponse from 'models/response/measurements/measurementTypeResponse';

@Component({
    templateUrl: 'measurementTypes.template.pug',
    styleUrls: ['measurementTypes.style.styl']
})
class MeasurementTypesComponent implements OnInit, OnDestroy {
    public measurementTypes$: Subject<Array<MeasurementTypeResponse>> =
        new Subject();

    private whenTypeDelete$: Subject<number> =
        new Subject();

    private whenTypeEdit$: Subject<number> =
        new Subject();

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private measurementService: IMeasurementService,
        private notificationService: INotificationService,
        private routerService: IRouterService,
    ) {
        this.whenTypeDelete$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(id => id !== 0),
                switchMap(id => this.measurementService.deleteMeasurementType(id)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
                switchMapTo(this.measurementService.getMeasurementTypes()),
                tap(_ => this.notificationService.success('Delete performed sucessfully'))
            )
            .subscribe(measurementTypes => this.measurementTypes$.next(measurementTypes));

        this.whenTypeEdit$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(id => id !== 0),
            )
            .subscribe(id => this.routerService.navigateArea(
                ['updateType'],
                { queryParams: { 'id': id } }
            ));
    }

    public ngOnInit(): void {
        this.measurementService
            .getMeasurementTypes()
            .pipe(takeUntil(this.whenComponentDestroy$))
            .subscribe(measurementTypes => this.measurementTypes$.next(measurementTypes));
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public onDeleteClick(typeId: number): void {
        this.whenTypeDelete$.next(typeId);
    }

    public onEditClick(typeId: number): void {
        this.whenTypeEdit$.next(typeId);
    }

    public onAddClick(): void {
        this.routerService.navigateArea(['addType']);
    }

    public getPaymentTypeClass(paymentTypeName: string): string {
        // todo: remove method and update model

        switch (paymentTypeName.toLowerCase()) {
            case 'жкх':
                return 'house';
            case 'электричество':
                return 'electricity';
            case 'интернет':
                return 'internet';
            default:
                return '';
        }
    }
}

export { MeasurementTypesComponent };