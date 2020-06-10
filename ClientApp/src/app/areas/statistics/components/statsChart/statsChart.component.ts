import { Component, Input } from '@angular/core';

import { ApexAxisChartSeries, ApexChart, ApexTitleSubtitle, ApexXAxis } from 'ng-apexcharts';

@Component({
    selector: 'app-stats-chart',
    templateUrl: 'statsChart.template.html',
})
export class StatsChartComponent {
    @Input('series')
    public series: Array<ApexAxisChartSeries>;

    @Input('xaxis')
    public xaxis: Array<ApexXAxis>;

    @Input('title')
    public title: Array<ApexTitleSubtitle>;

    public chart: ApexChart =
        {
            type: 'line',
            height: 350,
            animations: {
                enabled: true,
                easing: 'easeinout',
                speed: 800,
                animateGradually: {
                    enabled: true,
                    delay: 150
                },
                dynamicAnimation: {
                    enabled: true,
                    speed: 350
                }
            },
            toolbar: {
                show: false,
            },
            zoom: {
                enabled: false,
            }
        };

    constructor(
    ) {
    }
}