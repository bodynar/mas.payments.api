import { Component, OnDestroy, OnInit } from '@angular/core';

import { Subject } from 'rxjs';
import { filter, switchMap, switchMapTo, takeUntil, tap } from 'rxjs/operators';

import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';

import { PaymentTypeResponse } from 'models/response/paymentTypeResponse';
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

    constructor(
        private paymentService: IPaymentService,
        private notificationService: INotificationService,
        private routerService: IRouterService,
    ) {
        this.whenTypeDelete$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(id => id !== 0),
                switchMap(id => this.paymentService.deletePaymentType(id)),
                filter(hasError => {
                    if (hasError) {
                        this.notificationService.error('Error due deleting type. Try again later');
                    }
                    return !hasError;
                }),
                switchMapTo(this.paymentService.getPaymentTypes()),
                tap(_ => this.notificationService.success('Delete performed sucessfully'))
            )
            .subscribe(paymentTypes => this.paymentTypes$.next(paymentTypes));

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
            .pipe(takeUntil(this.whenComponentDestroy$))
            .subscribe(paymentTypes => this.paymentTypes$.next(paymentTypes));
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
}

export { PaymentTypesComponent };