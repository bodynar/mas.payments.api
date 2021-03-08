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
import { emptyMonth } from 'static/months';
import { emptyYear } from 'common/utils/years';

@Injectable()
class StatisticsService implements IStatisticsService {

    constructor(
        private statsApiBackend: IStatisticsApiBackendService,
        // private loggingService: ILoggingService
    ) { }

    public getPaymentStatistics(filter: PaymentStatisticsFilter): Observable<QueryExecutionResult<GetPaymentsStatisticsResponse>> {
        const requestData = this.getRequestData(filter);

        return this.statsApiBackend
            .getPaymentStatistics(requestData)
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
        const requestData = this.getRequestData(filter);

        return this.statsApiBackend
            .getMeasurementStatistics(requestData)
            .pipe(
                tap(response => {
                    if (!response.success) {
                        // this.loggingService.error(response);
                    }
                }),
            );
    }

    private getRequestData(filter: MeasurementStatisticsFilter | PaymentStatisticsFilter): any {
        const requestData = { ...filter };

        if (requestData.from.month === emptyMonth.id || requestData.from.year === emptyYear.id) {
            requestData.from = undefined;
        }
        if (requestData.to.month === emptyMonth.id || requestData.to.year === emptyYear.id) {
            requestData.to = undefined;
        }

        return requestData;
    }
}

export { StatisticsService };