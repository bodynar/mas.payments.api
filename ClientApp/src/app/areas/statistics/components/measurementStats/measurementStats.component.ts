import { Component, ViewChild } from '@angular/core';

import { ApexAxisChartSeries, ApexTitleSubtitle, ApexXAxis } from 'ng-apexcharts';

import { Subject } from 'rxjs';
import { filter, switchMap, takeUntil, switchMapTo, delay, tap } from 'rxjs/operators';

import { isNullOrEmpty, isNullOrUndefined } from 'common/utils/common';

import BaseComponent from 'common/components/BaseComponent';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IStatisticsService } from 'services/IStatisticsService';

import MonthYear from 'models/monthYearDate';
import { getShortMonthName, months } from 'static/months';

import MeasurementStatisticsFilter from 'models/request/stats/measurementStatisticsFilter';
import MeasurementTypeResponse from 'models/response/measurements/measurementTypeResponse';
import { GetMeasurementStatisticsResponse } from 'models/response/stats/measurementStatsResponse';

import { StatsChartComponent } from '../statsChart/statsChart.component';

@Component({
    selector: 'app-stats-measurement',
    templateUrl: 'measurementStats.template.pug',
})
export class MeasurementStatsComponent extends BaseComponent {
    private static chartName: string = 'Measurement statistics';

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
                categories: [...months.map(x => x.name)],
                title: { text: 'Month' }
            },
            title: { text: MeasurementStatsComponent.chartName }
        };

    public measurementTypes: Array<MeasurementTypeResponse>
        = [];

    public chartDataIsLoading: boolean =
        false;

    public statisticsFilter: MeasurementStatisticsFilter =
        { measurementTypeId: 0, from: new MonthYear(), to: new MonthYear() };

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
        if (!this.chartDataIsLoading) {
            this.whenSubmitForm$.next(null);
        }
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

        if (!isNullOrUndefined(this.chartElement)) {
            this.chartElement.setTitle(
                MeasurementStatsComponent.chartName + (
                    isNullOrEmpty(namePostfix)
                        ? '' : namePostfix));
        }

        if (hasAnyData) {
            const isInOneYear: boolean =
                new Set(stats.typeStatistics.map(x => x.statisticsData.map(y => y.year))
                    .flat())
                    .size === 1;

            this.chart.series = stats.typeStatistics.map(x => ({
                name: `${x.measurementTypeName}`,
                data: [...x.statisticsData.map(y => ({
                    x: `${getShortMonthName(y.month - 1)}${isInOneYear ? '' : ' ' + y.year}`,
                    y: y.diff
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
