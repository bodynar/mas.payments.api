import { Component } from '@angular/core';

import { ApexAxisChartSeries, ApexTitleSubtitle, ApexXAxis } from 'ng-apexcharts';

import { Subject, BehaviorSubject } from 'rxjs';
import { filter, switchMap, takeUntil, switchMapTo, delay, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'common/utils/common';

import BaseComponent from 'common/components/BaseComponent';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IStatisticsService } from 'services/IStatisticsService';

import { yearsRange } from 'common/utils/years';
import { getMonthName, months } from 'static/months';

import MeasurementStatisticsFilter from 'models/request/stats/measurementStatisticsFilter';
import MeasurementTypeResponse from 'models/response/measurements/measurementTypeResponse';
import { GetMeasurementStatisticsResponse } from 'models/response/stats/measurementStatsResponse';

@Component({
    selector: 'app-stats-measurement',
    templateUrl: 'measurementStats.template.pug',
    styleUrls: ['../stats/stats.style.styl'],
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

    public measurementTypes$: Subject<Array<MeasurementTypeResponse>> =
        new Subject();

    public chartDataIsLoading$: BehaviorSubject<boolean> =
        new BehaviorSubject<boolean>(false);

    public statisticsFilter: MeasurementStatisticsFilter =
        new MeasurementStatisticsFilter();

    public years: Array<{ name: string, id?: number }>;

    private whenSubmitForm$: Subject<null> =
        new Subject();

    private measurementTypes: Array<MeasurementTypeResponse>
        = [];

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

                this.years = yearsRange(2019, new Date().getFullYear() + 5);
                this.statisticsFilter.year = this.years[0].id;
                this.statisticsFilter.measurementTypeId = 0;

                this.measurementTypes$.next(this.measurementTypes);
                this.whenSubmitForm$.next(null);
            });

        this.whenSubmitForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                tap(() => this.chartDataIsLoading$.next(true)),
                switchMap(_ => this.statisticsService.getMeasurementStatistics(this.statisticsFilter)),
                delay(1.5 * 1000),
                tap(() => this.chartDataIsLoading$.next(false)),
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

    public onStatsRecieved(stats: GetMeasurementStatisticsResponse): void {
        const typeName: string =
            this.statisticsFilter.measurementTypeId
                ? this.measurementTypes.filter(x => x.id === this.statisticsFilter.measurementTypeId).pop().name
                : 'All';

        const hasAnyData: boolean =
            stats.typeStatistics.some(x => !isNullOrUndefined(x));

        if (hasAnyData) {
            this.chart.series = stats.typeStatistics.map(x => ({
                name: `${x.measurementTypeName} for ${stats.year}`,
                data: [...x.statisticsData.map(y => ({
                    x: getMonthName(y.month),
                    y: y.diff
                }))]
            }));
        } else {
            this.chart.series = [{
                name: `${typeName} for ${stats.year}`,
                data: []
            }];
        }
    }
}