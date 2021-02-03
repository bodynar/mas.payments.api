import { Component, ViewChild } from '@angular/core';

import { ApexAxisChartSeries, ApexTitleSubtitle, ApexXAxis } from 'ng-apexcharts';

import { Subject } from 'rxjs';
import { filter, switchMap, takeUntil, switchMapTo, tap, delay } from 'rxjs/operators';

import { isNullOrEmpty, isNullOrUndefined } from 'common/utils/common';

import BaseComponent from 'common/components/BaseComponent';

import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';
import { IStatisticsService } from 'services/IStatisticsService';

import { Year, yearsRange } from 'common/utils/years';
import { getMonthName, months } from 'static/months';

import PaymentStatisticsFilter from 'models/request/stats/paymentStatisticsFilter';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';
import { GetPaymentsStatisticsResponse } from 'models/response/stats/paymentStatsResponse';
import { StatsChartComponent } from '../statsChart/statsChart.component';
import { MonthSelectorValue } from 'common/components/monthSelector/monthSelector.component';

@Component({
    selector: 'app-stats-payments',
    templateUrl: 'paymentStats.template.pug',
})
export class PaymentStatsComponent extends BaseComponent {
    private static chartName: string = 'Payment statistics';

    @ViewChild(StatsChartComponent, { static: false })
    private chartElement: StatsChartComponent;

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
            title: { text: PaymentStatsComponent.chartName }
        };

    public paymentTypes: Array<PaymentTypeResponse> =
        [];

    public statisticsFilter: PaymentStatisticsFilter =
        {
            paymentTypeId: 0,
            from: new Date(),
            to: new Date()
        };

    public chartDataIsLoading: boolean =
        false;

    public years: Array<Year> =
        yearsRange(2019);

    private whenSubmitForm$: Subject<null> =
        new Subject();

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
            });

        this.whenSubmitForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                tap(() => this.chartDataIsLoading = true),
                switchMap(_ => this.statisticsService.getPaymentStatistics(this.statisticsFilter)),
                delay(1.5 * 1000),
                tap(() => this.chartDataIsLoading = false),
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

    public onDateChange(dateType: 'from' | 'to', value: MonthSelectorValue): void {
        if (!isNullOrUndefined(value)) {
            this.statisticsFilter[dateType] = new Date(value.year, value.month);
        } else {
            this.statisticsFilter[dateType] = undefined;
        }
    }

    public onPaymentsStatsRecieved(stats: GetPaymentsStatisticsResponse): void {
        const hasAnyData: boolean =
            stats.typeStatistics.some(x => !isNullOrUndefined(x));

        const namePostfix: string =
            (!isNullOrUndefined(stats.from)
                ? ` from ${stats.from.toDateString()}` : '')
            + (
                !isNullOrUndefined(stats.to)
                    ? ` to ${stats.to.toDateString()}` : ''
            );

        if (!isNullOrUndefined(this.chartElement)) {
            this.chartElement.setTitle(
                PaymentStatsComponent.chartName + (
                    isNullOrEmpty(namePostfix)
                        ? '' : namePostfix));
        }

        if (hasAnyData) {
            const isInOneYear: boolean =
                new Set(stats.typeStatistics.map(x => x.statisticsData.map(y => y.year))
                    .flat())
                    .size === 1;

            this.chart.series = stats.typeStatistics.map(x => ({
                name: `${x.paymentTypeName}`,
                data: [...x.statisticsData.map(y => ({
                    x: `${getMonthName(y.month - 1)}${isInOneYear ? '' : ' ' + y.year}`,
                    y: y.amount
                }))]
            }));
        } else {
            this.chart.series = [{
                name: 'Empty',
                data: []
            }];
        }
    }
}