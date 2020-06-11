import { Observable } from 'rxjs';

import MeasurementStatisticsFilter from 'models/request/stats/measurementStatisticsFilter';
import PaymentStatisticsFilter from 'models/request/stats/paymentStatisticsFilter';
import QueryExecutionResult from 'models/response/queryExecutionResult';

abstract class IStatisticsService {
    abstract getPaymentStatistics(filter: PaymentStatisticsFilter): Observable<QueryExecutionResult<GetPaymentsStatisticsResponse>>;
}

export { IStatisticsService };