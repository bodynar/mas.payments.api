import { Component, OnDestroy, OnInit } from '@angular/core';

import { ApexAxisChartSeries, ApexTitleSubtitle, ApexXAxis } from 'ng-apexcharts';

import { Subject } from 'rxjs';
import { filter, switchMap, takeUntil } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';
import { IStatisticsService } from 'services/IStatisticsService';

import { yearsRange } from 'common/utils/years';
import { getMonthName, months } from 'static/months';

import PaymentStatisticsFilter from 'models/request/stats/paymentStatisticsFilter';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';
import { GetPaymentsStatisticsResponse } from 'models/response/stats/paymentStatsResponse';

@Component({
    selector: 'app-stats-payments',
    templateUrl: 'paymentStats.template.pug'
})
export class PaymentStatsComponent implements OnInit, OnDestroy {
    public chart: {
        series: ApexAxisChartSeries,
        xaxis: ApexXAxis,
        title: ApexTitleSubtitle
    } = {
            series: [],
            xaxis: {
                type: 'category',
                categories: [
                    ...months.map(x => x.name)
                ],
                title: { text: 'Month' }
            },
            title: { text: 'Payment statistics' }
        };

    public paymentTypes$: Subject<Array<PaymentTypeResponse>> =
        new Subject();

    public statisticsFilter: PaymentStatisticsFilter =
        new PaymentStatisticsFilter();

    public years: Array<{ name: string, id?: number }>;

    private whenSubmitForm$: Subject<null> =
        new Subject();

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    private paymentTypes: Array<PaymentTypeResponse>
        = [];

    constructor(
        private paymentService: IPaymentService,
        private statisticsService: IStatisticsService,
        private notificationService: INotificationService,
    ) {
        this.years = yearsRange(2019, new Date().getFullYear() + 5);
        this.statisticsFilter.year = this.years[0].id;

        this.whenSubmitForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                switchMap(_ => this.statisticsService.getPaymentStatistics(this.statisticsFilter)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) => this.onPaymentsStatsRecieved(result));
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
                this.paymentTypes$.next(this.paymentTypes);

                this.statisticsFilter.paymentTypeId = result.slice(0, 1).pop().id;

                this.whenSubmitForm$.next(null);
            });
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public onFormSubmit(): void {
        this.whenSubmitForm$.next(null);
    }

    public onPaymentsStatsRecieved(stats: GetPaymentsStatisticsResponse): void {
        const paymentTypeName: string =
            this.paymentTypes.filter(x => x.id === stats.paymentTypeId).pop().name;

        const hasAnyData: boolean =
            stats.statisticsData.some(x => !isNullOrUndefined(x.amount));

        if (hasAnyData) {
            this.chart.series = [{
                name: `${paymentTypeName} for ${stats.year}`,
                data: [...stats.statisticsData.map(x => ({ x: getMonthName(x.month), y: x.amount }))]
            }];
        } else {
            this.chart.series = [{
                name: `${paymentTypeName} for ${stats.year}`,
                data: []
            }];
        }
    }
}