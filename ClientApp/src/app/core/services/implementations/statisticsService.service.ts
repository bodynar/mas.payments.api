import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { IStatisticsApiBackendService } from 'services/backend/IStatisticsApi.backend';
import { IStatisticsService } from 'services/IStatisticsService';

import { GetPaymentsStatisticsResponse } from 'models/response/payments/paymentStatsResponse';
import QueryExecutionResult from 'models/response/queryExecutionResult';
import StatisticsFilter from 'models/statisticsFilter';

@Injectable()
class StatisticsService implements IStatisticsService {

    constructor(
        private statsApiBackend: IStatisticsApiBackendService,
        // private loggingService: ILoggingService
    ) { }

    public getPaymentStatistics(filter: StatisticsFilter): Observable<QueryExecutionResult<GetPaymentsStatisticsResponse>> {
        return this.statsApiBackend
            .getPaymentStatistics(filter)
            .pipe(
                tap(response => {
                    if (!response.success) {
                        // this.loggingService.error(response);
                    }
                }),
            );
    }
}

export { StatisticsService };