import { Component, OnDestroy } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ReplaySubject, Subject } from 'rxjs';
import { switchMap, takeUntil } from 'rxjs/operators';

import { IStatisticsService } from 'services/IStatisticsService';

import { PaymentStatsResponse } from 'models/response/paymentStatsResponse';
import { StatisticsFilter } from 'models/statisticsFilter';

@Component({
    templateUrl: 'stats.template.pug',
    styleUrls: ['stats.style.styl']
})
class StatsComponent implements OnDestroy {

    public statisticsFilter: StatisticsFilter = {
        includeMeasurements: true,
    };

    public stats$: Subject<Array<PaymentStatsResponse>> =
        new ReplaySubject(1);

    private whenSubmitForm$: Subject<NgForm> =
        new Subject();

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private statisticsService: IStatisticsService
    ) {
        this.whenSubmitForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                switchMap(_ => this.statisticsService.getPaymentStatistics(this.statisticsFilter)),
            )
            .subscribe(stats => this.stats$.next(stats.items));
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public onFormSubmit(): void {
        this.whenSubmitForm$.next();
    }
}

export { StatsComponent };