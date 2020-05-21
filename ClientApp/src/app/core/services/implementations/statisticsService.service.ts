import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IStatisticsApiBackendService } from 'services/backend/IStatisticsApi.backend';
import { INotificationService } from 'services/INotificationService';
import { IStatisticsService } from 'services/IStatisticsService';

import { GetPaymentStatsResponse } from 'models/response/payments/paymentStatsResponse';
import { StatisticsFilter } from 'models/statisticsFilter';

@Injectable()
class StatisticsService implements IStatisticsService {

    constructor(
        private statsApiBackend: IStatisticsApiBackendService,
        private notificationService: INotificationService,
        // private loggingService: ILoggingService
    ) { }

    public getPaymentStatistics(filter?: StatisticsFilter): Observable<GetPaymentStatsResponse> {
        return this.statsApiBackend
            .getPaymentStatistics(filter)
            .pipe(
                tap(response => {
                    const hasError: boolean =
                        isNullOrUndefined(response.items) || !(response.items instanceof Array);

                    if (hasError) {
                        this.notificationService.error('Error due getting statistincs data');
                        // this.loggingService.error(response);
                    }
                }),
            );
    }
}

export { StatisticsService };