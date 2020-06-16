import { Component, OnDestroy, OnInit } from '@angular/core';

import { Subject } from 'rxjs';
import { filter, switchMap, switchMapTo, takeUntil, tap } from 'rxjs/operators';

import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';

import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';
import { IRouterService } from 'services/IRouterService';

@Component({
    templateUrl: 'paymentTypes.template.pug',
    styleUrls: ['paymentTypes.style.styl']
})
class PaymentTypesComponent implements OnInit, OnDestroy {
    public paymentTypes$: Subject<Array<PaymentTypeResponse>> =
        new Subject();

    private whenTypeDelete$: Subject<number> =
        new Subject();

    private whenTypeEdit$: Subject<number> =
        new Subject();

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    private paymentTypes: Array<PaymentTypeResponse> =
        [];

    constructor(
        private paymentService: IPaymentService,
        private notificationService: INotificationService,
        private routerService: IRouterService,
    ) {
        this.whenTypeDelete$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(id => id !== 0),
                filter(id => this.confirmUserDelete(id)),
                switchMap(id => this.paymentService.deletePaymentType(id)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
                switchMapTo(this.paymentService.getPaymentTypes()),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
                tap(_ => this.notificationService.success('Delete performed sucessfully'))
            )
            .subscribe(({ result }) => this.paymentTypes$.next(result));

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
        this.paymentService
            .getPaymentTypes()
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) => {
                this.paymentTypes = result;
                this.paymentTypes$.next(result);
            });
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

    private confirmUserDelete(id: number): boolean {
        const { hasRelatedPayments, hasRelatedMeasurementTypes } = this.paymentTypes.find(x => x.id === id);

        const relatedObjectsName: string =
            hasRelatedPayments && hasRelatedMeasurementTypes
                ? 'payments and measurement types'
                : hasRelatedPayments
                    ? 'payments'
                    : hasRelatedMeasurementTypes
                        ? 'measurement types'
                        : '';

        return !(hasRelatedPayments || hasRelatedMeasurementTypes)
            || confirm(`Type related with ${relatedObjectsName}.\nAre you sure want to delete it?`);
    }
}

export { PaymentTypesComponent };