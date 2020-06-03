import { Observable } from 'rxjs';

import { GetPaymentsStatisticsResponse } from 'models/response/payments/paymentStatsResponse';
import QueryExecutionResult from 'models/response/queryExecutionResult';
import StatisticsFilter from 'models/statisticsFilter';

abstract class IStatisticsService {
    abstract getPaymentStatistics(filter: StatisticsFilter): Observable<QueryExecutionResult<GetPaymentsStatisticsResponse>>;
}

export { IStatisticsService };