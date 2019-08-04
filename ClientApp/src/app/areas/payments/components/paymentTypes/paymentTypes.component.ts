import { Component, OnDestroy, OnInit } from '@angular/core';

import { Subject } from 'rxjs';
import { filter, switchMap, switchMapTo, takeUntil } from 'rxjs/operators';

import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';

import { PaymentTypeResponse } from 'models/response/paymentTypeResponse';

@Component({
    templateUrl: 'paymentTypes.template.pug',
    styleUrls: ['paymentTypes.style.styl']
})
class PaymentTypesComponent implements OnInit, OnDestroy {
    public paymentTypes$: Subject<Array<PaymentTypeResponse>> =
        new Subject();

    private whenDeleteCalled$: Subject<number> =
        new Subject();

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private paymentService: IPaymentService,
        private notificationService: INotificationService,
    ) {
        this.whenDeleteCalled$
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
            )
            .subscribe(paymentTypes => this.paymentTypes$.next(paymentTypes));
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

    public onDeleteClick(item: PaymentTypeResponse): void {
        this.whenDeleteCalled$.next(item.id);
    }
}

export { PaymentTypesComponent };