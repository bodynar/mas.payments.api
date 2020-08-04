import { Component, OnDestroy, OnInit } from '@angular/core';

import { ReplaySubject, Subject, of, Observable } from 'rxjs';
import { filter, switchMap, switchMapTo, takeUntil, tap, map } from 'rxjs/operators';

import { getPaginatorConfig } from 'common/paginator/paginator';
import PaginatorConfig from 'common/paginator/paginatorConfig';

import { ConfirmInModalComponent } from 'src/app/components/modal/components/confirm/confirm.component';

import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';

import { IRouterService } from 'services/IRouterService';
import { IModalService } from 'src/app/components/modal/IModalService';

import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';

@Component({
    templateUrl: 'paymentTypes.template.pug',
    styleUrls: ['paymentTypes.style.styl']
})
class PaymentTypesComponent implements OnInit, OnDestroy {
    public paymentTypes$: Subject<Array<PaymentTypeResponse>> =
        new Subject();

    public paginatorConfig$: Subject<PaginatorConfig> =
        new ReplaySubject(1);

    private whenTypeDelete$: Subject<number> =
        new Subject();

    private whenTypeEdit$: Subject<number> =
        new Subject();

    private whenGetPaymentTypes$: Subject<null> =
        new Subject();

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    private paymentTypes: Array<PaymentTypeResponse> =
        [];

    private pageSize: number =
        5;

    constructor(
        private paymentService: IPaymentService,
        private notificationService: INotificationService,
        private routerService: IRouterService,
        private modalService: IModalService,
    ) {
        this.whenGetPaymentTypes$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                switchMapTo(this.paymentService.getPaymentTypes()),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) => {
                this.paymentTypes = result;

                const paginatorConfig: PaginatorConfig =
                    getPaginatorConfig(this.paymentTypes, this.pageSize);

                if (paginatorConfig.enabled) {
                    this.onPageChange(0);
                } else {
                    this.paymentTypes$.next(this.paymentTypes);
                }

                this.paginatorConfig$.next(paginatorConfig);
            });

        this.whenTypeDelete$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(id => id !== 0),
                switchMap(id => this.hasRelatedObjects(id)),
                filter(({ canDelete }) => canDelete),
                switchMap(({ id }) => this.paymentService.deletePaymentType(id)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    } else {
                        this.notificationService.success('Delete performed sucessfully');
                    }
                    return response.success;
                }),
            )
            .subscribe(() => this.whenGetPaymentTypes$.next(null));

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
        this.whenGetPaymentTypes$.next(null)
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

    public onPageChange(pageNumber: number): void {
        const slicedItems: Array<PaymentTypeResponse> =
            this.paymentTypes.slice(this.pageSize * pageNumber, (pageNumber + 1) * this.pageSize);

        this.paymentTypes$.next(slicedItems);
    }

    private hasRelatedObjects(id: number): Observable<{id: number, canDelete: boolean }> {
        const { hasRelatedPayments, hasRelatedMeasurementTypes } = this.paymentTypes.find(x => x.id === id);

        if (hasRelatedPayments || hasRelatedMeasurementTypes) {
            const relatedObjectsName: string =
                hasRelatedPayments && hasRelatedMeasurementTypes
                    ? 'payments and measurement types'
                    : hasRelatedPayments
                        ? 'payments'
                        : hasRelatedMeasurementTypes
                            ? 'measurement types'
                            : '';

            return this.modalService.show(ConfirmInModalComponent, {
                size: 'medium',
                title: 'Warning! Measurement type related with measurement.',
                body: {
                    isHtml: false,
                    content: `Type related with ${relatedObjectsName}.\nAre you sure want to delete it?`
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
            return of({ id: id, canDelete: true });
        }
    }
}

export { PaymentTypesComponent };