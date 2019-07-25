import { Component, OnDestroy, OnInit } from '@angular/core';

import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { MeasurementResponse } from 'models/response/measurementResponse';

import { IMeasurementService } from 'services/IMeasurementService';
import { IRouterService } from 'services/IRouterService';

@Component({
    templateUrl: 'measurementList.template.pug',
    styleUrls: ['measurementList.style.styl']
})
class MeasurementListComponent implements OnInit, OnDestroy {
    public measurements$: Subject<Array<MeasurementResponse>> =
        new Subject();

    public actions: Array<string> =
        ['addMeasurement', 'addMeasurementType'];

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private measurementService: IMeasurementService,
        private routerService: IRouterService,
    ) {
    }

    public ngOnInit(): void {
        this.measurementService
            .getMeasurements()
            .pipe(
                takeUntil(this.whenComponentDestroy$)
            )
            .subscribe(measurements => this.measurements$.next(measurements));
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public onActionClick(actionName: string): void {
        this.routerService.navigateDeep([actionName]);
    }
}

export { MeasurementListComponent };