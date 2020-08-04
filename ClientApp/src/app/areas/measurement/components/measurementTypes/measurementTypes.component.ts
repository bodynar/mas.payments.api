import { Component, OnDestroy, OnInit } from '@angular/core';

import { ReplaySubject, Subject, of } from 'rxjs';
import { filter, switchMap, switchMapTo, takeUntil, tap, map } from 'rxjs/operators';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IRouterService } from 'services/IRouterService';
import { IModalService } from 'src/app/components/modal/IModalService';

import PaginatorConfig from 'common/paginator/paginatorConfig';

import MeasurementTypeResponse from 'models/response/measurements/measurementTypeResponse';
import { getPaginatorConfig } from 'common/paginator/paginator';
import { ConfirmInModalComponent } from 'src/app/components/modal/components/confirm/confirm.component';

@Component({
    templateUrl: 'measurementTypes.template.pug',
    styleUrls: ['measurementTypes.style.styl']
})
class MeasurementTypesComponent implements OnInit, OnDestroy {
    public measurementTypes$: Subject<Array<MeasurementTypeResponse>> =
        new Subject();

    public paginatorConfig$: Subject<PaginatorConfig> =
        new ReplaySubject(1);

    private whenTypeDelete$: Subject<number> =
        new Subject();

    private whenTypeEdit$: Subject<number> =
        new Subject();

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    private whenGetMeasurementTypes$: Subject<null> =
        new Subject();

    private measurementTypes: Array<MeasurementTypeResponse> =
        [];

    private pageSize: number =
        10;

    constructor(
        private measurementService: IMeasurementService,
        private notificationService: INotificationService,
        private routerService: IRouterService,
        private modalService: IModalService,
    ) {
        this.whenGetMeasurementTypes$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                switchMapTo(this.measurementService.getMeasurementTypes()),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }

                    return response.success;
                }),
            )
            .subscribe(({ result }) => {
                this.measurementTypes = result;

                const paginatorConfig: PaginatorConfig =
                    getPaginatorConfig(this.measurementTypes, this.pageSize);

                if (paginatorConfig.enabled) {
                    this.onPageChange(0);
                } else {
                    this.measurementTypes$.next(this.measurementTypes);
                }

                this.paginatorConfig$.next(paginatorConfig);
            });

        this.whenTypeDelete$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(id => id !== 0),
                switchMap(id => {
                    if (this.measurementTypes.find(x => x.id === id).hasRelatedMeasurements) {
                        return this.modalService.show(ConfirmInModalComponent, {
                            size: 'medium',
                            title: 'Warning! Measurement type related with measurement.',
                            body: {
                                isHtml: false,
                                content: 'Measurement type related with measurement.\nAre you sure want to delete it?'
                            },
                            additionalParameters: {
                                confirmBtnText: 'Yes',
                                cancelBtnText: 'No',
                            }
                        }).pipe(
                            map(response => response as boolean),
                            map(response => ({ id: id, canDelete: response }))
                        );
                    } else {
                        return of(({ id: id, canDelete: true }));
                    }
                }),
                filter(({ canDelete }) => canDelete),
                switchMap(({ id }) => this.measurementService.deleteMeasurementType(id)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    } else {
                        this.notificationService.success('Delete performed sucessfully');
                    }

                    return response.success;
                }),
            )
            .subscribe(() => this.whenGetMeasurementTypes$.next(null));

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
        this.whenGetMeasurementTypes$.next(null);
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

    public onPageChange(pageNumber: number): void {
        const slicedItems: Array<MeasurementTypeResponse> =
            this.measurementTypes.slice(this.pageSize * pageNumber, (pageNumber + 1) * this.pageSize);

        this.measurementTypes$.next(slicedItems);
    }
}

export { MeasurementTypesComponent };