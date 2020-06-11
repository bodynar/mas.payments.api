import { Component, OnDestroy, OnInit } from '@angular/core';

import { ApexAxisChartSeries, ApexTitleSubtitle, ApexXAxis } from 'ng-apexcharts';

import { Subject } from 'rxjs';
import { filter, switchMap, takeUntil } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IMeasurementService } from 'services/IMeasurementService';
import { INotificationService } from 'services/INotificationService';
import { IStatisticsService } from 'services/IStatisticsService';

import { yearsRange } from 'src/common/years';
import { getMonthName, months } from 'src/static/months';

import MeasurementStatisticsFilter from 'models/request/stats/measurementStatisticsFilter';
import MeasurementTypeResponse from 'models/response/measurements/measurementTypeResponse';
import { GetMeasurementStatisticsResponse } from 'models/response/stats/measurementStatsResponse';

@Component({
    selector: 'app-stats-measurement',
    templateUrl: 'measurementStats.template.pug'
})
export class MeasurementStatsComponent implements OnInit, OnDestroy {
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

    public statisticsFilter: MeasurementStatisticsFilter =
        new MeasurementStatisticsFilter();

    public years: Array<{ name: string, id?: number }>;

    private whenSubmitForm$: Subject<null> =
        new Subject();

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    private measurementTypes: Array<MeasurementTypeResponse>
        = [];

    constructor(
        private measurementService: IMeasurementService,
        private statisticsService: IStatisticsService,
        private notificationService: INotificationService,
    ) {
        this.years = yearsRange(2019, new Date().getFullYear() + 5);
        this.statisticsFilter.year = this.years[0].id;

        this.whenSubmitForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                switchMap(_ => this.statisticsService.getMeasurementStatistics(this.statisticsFilter)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) => this.onStatsRecieved(result));
    }

    public ngOnInit(): void {
        this.measurementService
            .getMeasurementTypes()
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
                this.measurementTypes = result;
                this.measurementTypes$.next(this.measurementTypes);

                this.statisticsFilter.measurementTypeId = result.slice(0, 1).pop().id;

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

    public onStatsRecieved(stats: GetMeasurementStatisticsResponse): void {
        const paymentTypeName: string =
            this.measurementTypes.filter(x => x.id === stats.measurementTypeId).pop().name;

        const hasAnyData: boolean =
            stats.statisticsData.some(x => !isNullOrUndefined(x.diff));

        if (hasAnyData) {
            this.chart.series = [{
                name: `${paymentTypeName} for ${stats.year}`,
                data: [...stats.statisticsData.map(x => ({ x: getMonthName(x.month), y: x.diff }))]
            }];
        } else {
            this.chart.series = [{
                name: `${paymentTypeName} for ${stats.year}`,
                data: []
            }];
        }

    }
}