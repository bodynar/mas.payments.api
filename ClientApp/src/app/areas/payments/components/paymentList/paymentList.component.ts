import { Component } from '@angular/core';

import { Subject } from 'rxjs';
import { filter, switchMap, switchMapTo, takeUntil, tap, delay, map } from 'rxjs/operators';

import { isNullOrUndefined } from 'common/utils/common';

import { emptyYear, Year, yearsRange } from 'common/utils/years';
import { emptyMonth, Month, months } from 'static/months';

import { BaseRoutingComponentWithModalComponent } from 'common/components/BaseComponentWithModal';

import { getPaginatorConfig } from 'sharedComponents/paginator/paginator';
import PaginatorConfig from 'sharedComponents/paginator/paginatorConfig';

import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';
import { IRouterService } from 'services/IRouterService';

import { IModalService } from 'src/app/components/modal/IModalService';

import { PaymentsFilter } from 'models/request/payment';
import PaymentResponse from 'models/response/payments/paymentResponse';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';

@Component({
    templateUrl: 'paymentList.template.pug',
    styleUrls: ['paymentList.style.styl'],
})
export class PaymentListComponent extends BaseRoutingComponentWithModalComponent {
    public paginatorConfig: PaginatorConfig;

    public filters: PaymentsFilter =
        new PaymentsFilter();

    public payments$: Subject<Array<PaymentResponse>> =
        new Subject();

    public paymentTypes$: Subject<Array<PaymentTypeResponse>> =
        new Subject();

    public isLoading: boolean =
        false;

    public hasData: boolean =
        false;

    public isDescSortOrder: boolean =
        false;

    public amountFilterType: string =
        '';

    public currentSortColumn: string
        = 'month';

    public months: Array<Month> =
        [emptyMonth, ...months];

    public years: Array<Year> =
        [emptyYear, ...yearsRange(2019)];


    private whenSubmitFilters$: Subject<null> =
        new Subject();

    private whenPaymentDelete$: Subject<number> =
        new Subject();

    private whenPaymentEdit$: Subject<number> =
        new Subject();

    private currentSortOrder: 'asc' | 'desc' =
        'asc';

    private payments: Array<PaymentResponse> =
        [];

    private pageSize: number =
        10;

    private currentPage: number =
        0;

    constructor(
        private paymentService: IPaymentService,
        private notificationService: INotificationService,
        modalService: IModalService,
        routerService: IRouterService,
    ) {
        super(routerService, modalService);

        this.whenComponentInit$
            .pipe(
                tap(_ => {
                    this.payments$.next([]);
                    this.isLoading = true;
                }),
                switchMapTo(this.paymentService.getPaymentTypes()),
                takeUntil(this.whenComponentDestroy$),
                tap(_ => this.isLoading = false),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) => {
                this.paymentTypes$.next([{
                    name: '',
                    systemName: '',
                }, ...result]);

                this.whenSubmitFilters$.next();
            });

        this.whenSubmitFilters$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                tap(_ => this.isLoading = true),
                switchMap(_ => this.paymentService.getPayments(this.filters)),
                delay(1.5 * 1000),
                tap(_ => this.isLoading = false),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
                tap(({ result }) => this.hasData = result.length > 0),
            )
            .subscribe(({ result }) => {
                this.payments = result;

                const paginatorConfig: PaginatorConfig =
                    getPaginatorConfig(this.payments, this.pageSize);

                this.paginatorConfig = paginatorConfig;

                if (paginatorConfig.enabled) {
                    this.onPageChange(0);
                } else {
                    this.payments$.next(this.payments);
                }

                this.onSortColumn(this.currentSortColumn, this.currentSortOrder === 'desc' ? 'asc' : 'desc');
            });

        this.whenPaymentDelete$
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
                switchMap(id => this.paymentService.deletePayment(id)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
                tap(_ => this.notificationService.success('Delete performed sucessfully'))
            )
            .subscribe(_ => this.whenSubmitFilters$.next(null));

        this.whenPaymentEdit$
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
    }

    public onActionClick(actionName: string): void {
        this.routerService.navigateDeep([actionName]);
    }

    public onSubmitClick(): void {
        this.whenSubmitFilters$.next();
    }

    public onAmountFilterChanged(filterType: '' | 'between' | 'exactly'): void {
        const availableTypes: Array<string> =
            ['between', 'exactly'];

        if (availableTypes.includes(filterType.toLowerCase())) {
            this.filters.amount = {};
        } else {
            this.filters.amount = undefined;
        }

        this.amountFilterType = filterType;
    }

    public onDeleteRecordClick(paymentId: number): void {
        this.whenPaymentDelete$.next(paymentId);
    }

    public onEditRecordClick(paymentId: number): void {
        this.whenPaymentEdit$.next(paymentId);
    }

    public onTypeClick(paymentTypeId: number): void {
        if (!isNullOrUndefined(paymentTypeId) && paymentTypeId !== 0) {
            this.filters.paymentTypeId = paymentTypeId;

            this.onSubmitClick();
        }
    }

    public clearFilters(): void {
        this.filters = {};
        this.amountFilterType = '';
        this.currentSortOrder = 'asc';
        this.currentSortColumn = '';

        this.currentSortColumn = '';
        this.isDescSortOrder = false;

        this.onSubmitClick();
    }

    public onSortColumn(columnName: string, sortOrder?: 'asc' | 'desc'): void {
        if (this.currentSortColumn !== columnName) {
            this.currentSortOrder = 'asc';
            this.currentSortColumn = columnName;
        }

        const usedStoredValue: boolean =
            isNullOrUndefined(sortOrder);

        if (usedStoredValue) {
            this.currentSortOrder =
                this.currentSortOrder === 'asc'
                    ? 'desc'
                    : 'asc';

            this.isDescSortOrder = this.currentSortOrder === 'desc';
        }

        this.currentSortColumn = columnName;
        this.onPageChange(this.currentPage);
    }

    public onPageChange(pageNumber: number): void {
        const sortingFunc: (left: PaymentResponse, right: PaymentResponse) => number =
            this.getSortFunction(this.currentSortColumn);

        let items: Array<PaymentResponse>;

        if (this.paginatorConfig.enabled) {
            items =
                this.payments
                    .sort(sortingFunc)
                    .slice(this.pageSize * pageNumber, (pageNumber + 1) * this.pageSize);
        } else {
            items = this.payments.sort(sortingFunc);
        }

        this.payments$.next(items);
        this.currentPage = pageNumber;
    }

    private getSortFunction(columnName: string, sortOrder?: 'asc' | 'desc'): (left: PaymentResponse, right: PaymentResponse) => number {
        let sortingFunc: (left: PaymentResponse, right: PaymentResponse) => number;

        sortOrder = sortOrder || this.currentSortOrder;

        switch (columnName) {
            case 'month':
                if (sortOrder === 'asc') {
                    sortingFunc = (left, right) => +left.month - (+right.month);
                } else {
                    sortingFunc = (left, right) => +right.month - (+left.month);
                }
                break;

            case 'year':
                if (sortOrder === 'asc') {
                    sortingFunc = (left, right) => left.year - right.year;
                } else {
                    sortingFunc = (left, right) => right.year - left.year;
                }
                break;

            case 'type':
                if (sortOrder === 'asc') {
                    sortingFunc = (left, right) => left.paymentTypeId - right.paymentTypeId;
                } else {
                    sortingFunc = (left, right) => right.paymentTypeId - left.paymentTypeId;
                }
                break;
            case 'value':
                if (sortOrder === 'asc') {
                    sortingFunc = (left, right) => left.amount - right.amount;
                } else {
                    sortingFunc = (left, right) => right.amount - left.amount;
                }
                break;
            default:
                if (sortOrder === 'asc') {
                    sortingFunc = (left, right) => left.year - right.year;
                } else {
                    sortingFunc = (left, right) => right.year - left.year;
                }
                break;
        }

        return sortingFunc;
    }
}