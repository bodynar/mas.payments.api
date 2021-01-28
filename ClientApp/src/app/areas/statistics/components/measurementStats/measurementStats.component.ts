import { Component } from '@angular/core';

import { ApexAxisChartSeries, ApexTitleSubtitle, ApexXAxis } from 'ng-apexcharts';

import { Subject } from 'rxjs';
import { filter, switchMap, takeUntil, switchMapTo, delay, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'common/utils/common';

import BaseComponent from 'common/components/BaseComponent';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IStatisticsService } from 'services/IStatisticsService';

import { getMonthName, months } from 'static/months';

import MeasurementStatisticsFilter from 'models/request/stats/measurementStatisticsFilter';
import MeasurementTypeResponse from 'models/response/measurements/measurementTypeResponse';
import { GetMeasurementStatisticsResponse } from 'models/response/stats/measurementStatsResponse';
import { MonthSelectorValue } from 'common/components/monthSelector/monthSelector.component';

@Component({
    selector: 'app-stats-measurement',
    templateUrl: 'measurementStats.template.pug',
})
export class MeasurementStatsComponent extends BaseComponent {
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
            title: { text: 'Measurement statistics' }
        };

    public measurementTypes: Array<MeasurementTypeResponse>
        = [];

    public chartDataIsLoading: boolean =
        false;

    public statisticsFilter: MeasurementStatisticsFilter =
        new MeasurementStatisticsFilter();

    private whenSubmitForm$: Subject<null> =
        new Subject();

    constructor(
        private measurementService: IMeasurementService,
        private statisticsService: IStatisticsService,
        private notificationService: INotificationService,
    ) {
        super();

        this.whenComponentInit$
            .pipe(
                switchMapTo(this.measurementService.getMeasurementTypes()),
                takeUntil(this.whenComponentDestroy$),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) => {
                this.measurementTypes = [
                    {
                        name: 'All',
                        id: 0,
                        systemName: 'all'
                    },
                    ...result
                ];
                this.statisticsFilter.measurementTypeId = 0;
                this.whenSubmitForm$.next(null);
            });

        this.whenSubmitForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                tap(() => this.chartDataIsLoading = true),
                switchMap(_ => this.statisticsService.getMeasurementStatistics(this.statisticsFilter)),
                delay(1.5 * 1000),
                tap(() => this.chartDataIsLoading = false),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) => this.onStatsRecieved(result));
    }

    public onFormSubmit(): void {
        this.whenSubmitForm$.next(null);
    }

    public onDateChange(dateType: 'from' | 'to', value: MonthSelectorValue): void {
        this.statisticsFilter[dateType] = new Date(value.year, value.month);
    }

    public onStatsRecieved(stats: GetMeasurementStatisticsResponse): void {
        const hasAnyData: boolean =
            stats.typeStatistics.some(x => !isNullOrUndefined(x));

        const namePostfix: string =
            (!isNullOrUndefined(stats.from)
                ? ` from ${stats.from.toDateString()}` : '')
            + (
                !isNullOrUndefined(stats.to)
                    ? ` to ${stats.to.toDateString()}` : ''
            );

        if (hasAnyData) {
            this.chart.series = stats.typeStatistics.map(x => ({
                name: `${x.measurementTypeName}${namePostfix}`,
                data: [...x.statisticsData.map(y => ({
                    x: getMonthName(y.month),
                    y: y.diff
                }))]
            })
            );
        } else {
            this.chart.series = [{
                name: 'Empty',
                data: []
            }];
        }
    }
}