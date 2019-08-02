import { Component, OnDestroy, OnInit } from '@angular/core';

import { BehaviorSubject, Subject } from 'rxjs';
import { delay, switchMap, takeUntil, tap } from 'rxjs/operators';

import { IPaymentService } from 'services/IPaymentService';
import { IRouterService } from 'services/IRouterService';

import { PaymentsFilter } from 'models/paymentsFilter';
import { PaymentResponse } from 'models/response/paymentResponse';
import { PaymentTypeResponse } from 'models/response/paymentTypeResponse';

@Component({
    templateUrl: 'paymentList.template.pug',
    styleUrls: ['paymentList.style.styl']
})
class PaymentListComponent implements OnInit, OnDestroy {
    public filters: PaymentsFilter =
        { };

    public payments$: Subject<Array<PaymentResponse>> =
        new Subject();

    public paymentTypes$: Subject<Array<PaymentTypeResponse>> =
        new Subject();

    public isLoading$: Subject<boolean> =
        new BehaviorSubject(true);

    public months: Array<{ name: string, id?: number }>;

    public actions: Array<string> =
        ['addPayment', 'addPaymentType'];

    public amountFilterType$: BehaviorSubject<string> =
        new BehaviorSubject('');


    private whenSubmitFilters$: Subject<null> =
        new Subject();

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private paymentService: IPaymentService,
        private routerService: IRouterService,
    ) {
        this.months = [
            {
                name: ''
            },
            {
                id: 1,
                name: 'January'
            },
            {
                id: 2,
                name: 'February'
            },
            {
                id: 3,
                name: 'March'
            },
            {
                id: 4,
                name: 'April'
            },
            {
                id: 5,
                name: 'May'
            },
            {
                id: 6,
                name: 'June'
            },
            {
                id: 7,
                name: 'July'
            },
            {
                id: 8,
                name: 'August'
            },
            {
                id: 9,
                name: 'September'
            },
            {
                id: 10,
                name: 'October'
            },
            {
                id: 11,
                name: 'November'
            },
            {
                id: 12,
                name: 'December'
            },
        ];

        this.whenSubmitFilters$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                tap(_ => this.isLoading$.next(true)),
                switchMap(_ => this.paymentService.getPayments(this.filters)),
                delay(2 * 1000), // todo: configure this value to UX
                tap(_ => this.isLoading$.next(false))
            )
            .subscribe(payments => this.payments$.next(payments));
    }

    public ngOnInit(): void {
        this.whenSubmitFilters$.next();

        this.paymentService
            .getPaymentTypes()
            .pipe(takeUntil(this.whenComponentDestroy$))
            .subscribe(paymentTypes => this.paymentTypes$.next([{
                name: ''
            }, ...paymentTypes]));
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public onActionClick(actionName: string): void {
        this.routerService.navigateDeep([actionName]);
    }

    public applyFilters(): void {
        this.whenSubmitFilters$.next();
    }

    public amountFilterChanged(filterType: string): void {
        const availableTypes: Array<string> =
            ['between', 'exactly'];

        if (availableTypes.includes(filterType.toLowerCase())) {
            this.filters.amount = { };
        } else {
            this.filters.amount = undefined;
        }
        this.amountFilterType$.next(filterType.toLowerCase());
    }
}

export { PaymentListComponent };