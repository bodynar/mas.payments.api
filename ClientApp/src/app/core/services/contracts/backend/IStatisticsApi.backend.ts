import { Observable } from 'rxjs';

import MeasurementStatisticsFilter from 'models/request/stats/measurementStatisticsFilter';
import PaymentStatisticsFilter from 'models/request/stats/paymentStatisticsFilter';
import QueryExecutionResult from 'models/response/queryExecutionResult';
import { GetMeasurementStatisticsResponse } from 'models/response/stats/measurementStatsResponse';
import { GetPaymentsStatisticsResponse } from 'models/response/stats/paymentStatsResponse';

abstract class IStatisticsApiBackendService {
    abstract getPaymentStatistics(filter: PaymentStatisticsFilter): Observable<QueryExecutionResult<GetPaymentsStatisticsResponse>>;

    abstract getMeasurementStatistics(filter: MeasurementStatisticsFilter)
        : Observable<QueryExecutionResult<GetMeasurementStatisticsResponse>>;
}

export { IStatisticsApiBackendService };