import { Observable } from 'rxjs';

import { GetPaymentStatsResponse } from 'models/response/paymentStatsResponse';
import { StatisticsFilter } from 'models/statisticsFilter';


abstract class IStatisticsService {
    abstract getPaymentStatistics(filter?: StatisticsFilter): Observable<GetPaymentStatsResponse>;
}

export { IStatisticsService };