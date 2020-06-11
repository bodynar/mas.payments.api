import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { IStatisticsApiBackendService } from 'services/backend/IStatisticsApi.backend';
import { IStatisticsService } from 'services/IStatisticsService';

import MeasurementStatisticsFilter from 'models/request/stats/measurementStatisticsFilter';
import PaymentStatisticsFilter from 'models/request/stats/paymentStatisticsFilter';
import QueryExecutionResult from 'models/response/queryExecutionResult';

@Injectable()
class StatisticsService implements IStatisticsService {

    constructor(
        private statsApiBackend: IStatisticsApiBackendService,
        // private loggingService: ILoggingService
    ) { }

    public getPaymentStatistics(filter: PaymentStatisticsFilter): Observable<QueryExecutionResult<GetPaymentsStatisticsResponse>> {
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