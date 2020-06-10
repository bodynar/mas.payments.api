import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ApexAxisChartSeries, ApexTitleSubtitle, ApexXAxis } from 'ng-apexcharts';

import { Subject } from 'rxjs';
import { filter, switchMap, takeUntil } from 'rxjs/operators';

import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';
import { IStatisticsService } from 'services/IStatisticsService';

import { yearsRange } from 'src/common/years';
import { getMonthName, months } from 'src/static/months';

import { GetPaymentsStatisticsResponse } from 'models/response/payments/paymentStatsResponse';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';
import StatisticsFilter from 'models/statisticsFilter';

@Component({
    templateUrl: 'stats.template.pug',
    styleUrls: ['stats.style.styl']
})
class StatsComponent implements OnInit, OnDestroy {
    public paymentsChart: {
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
            title: { text: 'Payments statistics' }
        };

    public paymentTypes$: Subject<Array<PaymentTypeResponse>> =
        new Subject();

    public statisticsFilter: StatisticsFilter =
        new StatisticsFilter();

    public years: Array<{ name: string, id?: number }>;

    private whenSubmitForm$: Subject<NgForm> =
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

    public onPaymentsStatsRecieved(paymentStats: GetPaymentsStatisticsResponse): void {
        const paymentTypeName: string =
            this.paymentTypes.filter(x => x.id === paymentStats.paymentTypeId).pop().name;

        this.paymentsChart.series = [{
            name: `${paymentTypeName} for ${paymentStats.year}`,
            data: [...paymentStats.statisticsData.map(x => ({ x: getMonthName(x.month), y: x.amount }))]
        }];
    }
}

export { StatsComponent };