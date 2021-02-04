import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { IStatisticsApiBackendService } from 'services/backend/IStatisticsApi.backend';
import { IStatisticsService } from 'services/IStatisticsService';

import MeasurementStatisticsFilter from 'models/request/stats/measurementStatisticsFilter';
import PaymentStatisticsFilter from 'models/request/stats/paymentStatisticsFilter';
import QueryExecutionResult from 'models/response/queryExecutionResult';
import { GetMeasurementStatisticsResponse } from 'models/response/stats/measurementStatsResponse';
import { GetPaymentsStatisticsResponse } from 'models/response/stats/paymentStatsResponse';

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

    public getMeasurementStatistics(filter: MeasurementStatisticsFilter)
        : Observable<QueryExecutionResult<GetMeasurementStatisticsResponse>> {

        return this.statsApiBackend
            .getMeasurementStatistics(filter)
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