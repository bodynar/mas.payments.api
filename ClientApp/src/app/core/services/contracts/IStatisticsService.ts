import { Observable } from 'rxjs';

import { GetPaymentStatsResponse } from 'models/response/payments/paymentStatsResponse';
import { StatisticsFilter } from 'models/statisticsFilter';


abstract class IStatisticsService {
    abstract getPaymentStatistics(filter?: StatisticsFilter): Observable<GetPaymentStatsResponse>;
}

export { IStatisticsService };