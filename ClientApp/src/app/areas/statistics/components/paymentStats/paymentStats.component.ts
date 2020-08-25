import { Component } from '@angular/core';

import { ApexAxisChartSeries, ApexTitleSubtitle, ApexXAxis } from 'ng-apexcharts';

import { Subject, BehaviorSubject } from 'rxjs';
import { filter, switchMap, takeUntil, switchMapTo, tap, delay } from 'rxjs/operators';

import { isNullOrUndefined } from 'common/utils/common';

import BaseComponent from 'common/components/BaseComponent';

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
    templateUrl: 'paymentStats.template.pug',
    styleUrls: ['../stats/stats.style.styl'],
})
export class PaymentStatsComponent extends BaseComponent {
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

    public chartDataIsLoading$: BehaviorSubject<boolean> =
        new BehaviorSubject<boolean>(false);

    public years: Array<{ name: string, id?: number }>;

    private whenSubmitForm$: Subject<null> =
        new Subject();

    private paymentTypes: Array<PaymentTypeResponse>
        = [];

    constructor(
        private paymentService: IPaymentService,
        private statisticsService: IStatisticsService,
        private notificationService: INotificationService,
    ) {
        super();

        this.whenComponentInit$
            .pipe(
                switchMapTo(this.paymentService.getPaymentTypes()),
                takeUntil(this.whenComponentDestroy$),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) => {
                this.paymentTypes = [
                    {
                        name: 'All',
                        id: 0,
                        systemName: 'all'
                    },
                    ...result
                ];
                this.years = yearsRange(2019, new Date().getFullYear() + 5);
                this.statisticsFilter.year = this.years[0].id;
                this.statisticsFilter.paymentTypeId = 0;

                this.paymentTypes$.next(this.paymentTypes);
                this.whenSubmitForm$.next(null);
            });

        this.whenSubmitForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                tap(() => this.chartDataIsLoading$.next(true)),
                switchMap(_ => this.statisticsService.getPaymentStatistics(this.statisticsFilter)),
                delay(1.5 * 1000),
                tap(() => this.chartDataIsLoading$.next(false)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) => this.onPaymentsStatsRecieved(result));
    }

    public onFormSubmit(): void {
        this.whenSubmitForm$.next(null);
    }

    public onPaymentsStatsRecieved(stats: GetPaymentsStatisticsResponse): void {
        const paymentTypeName: string =
            this.statisticsFilter.paymentTypeId
                ? this.paymentTypes.filter(x => x.id === this.statisticsFilter.paymentTypeId).pop().name
                : 'All';

        const hasAnyData: boolean =
            stats.typeStatistics.some(x => !isNullOrUndefined(x));

        if (hasAnyData) {
            this.chart.series = stats.typeStatistics.map(x => ({
                name: `${x.paymentTypeName} for ${stats.year}`,
                data: [...x.statisticsData.map(y => ({
                    x: getMonthName(y.month),
                    y: y.amount
                }))]
            }));
        } else {
            this.chart.series = [{
                name: `${paymentTypeName} for ${stats.year}`,
                data: []
            }];
        }
    }
}