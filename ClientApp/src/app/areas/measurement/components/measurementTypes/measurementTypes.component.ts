import { Component, OnDestroy, OnInit } from '@angular/core';

import { Subject } from 'rxjs';
import { filter, switchMap, switchMapTo, takeUntil } from 'rxjs/operators';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';

import { MeasurementTypeResponse } from 'models/response/measurementTypeResponse';

@Component({
    templateUrl: 'measurementTypes.template.pug',
    styleUrls: ['measurementTypes.style.styl']
})
class MeasurementTypesComponent implements OnInit, OnDestroy {
    public measurementTypes$: Subject<Array<MeasurementTypeResponse>> =
        new Subject();

    private whenDeleteCalled$: Subject<number> =
        new Subject();

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private measurementService: IMeasurementService,
        private notificationService: INotificationService,
    ) {
        this.whenDeleteCalled$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(id => id !== 0),
                switchMap(id => this.measurementService.deleteMeasurementType(id)),
                filter(hasError => {
                    if (hasError) {
                        this.notificationService.error('Error due deleting type. Try again later');
                    }
                    return !hasError;
                }),
                switchMapTo(this.measurementService.getMeasurementTypes()),
            )
            .subscribe(measurementTypes => this.measurementTypes$.next(measurementTypes));
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

    public onDeleteClick(item: MeasurementTypeResponse): void {
        this.whenDeleteCalled$.next(item.id);
    }
}

export { MeasurementTypesComponent };