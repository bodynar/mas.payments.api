import { Observable } from 'rxjs';

import { PaymentStatsResponse } from 'models/response/paymentStatsResponse';
import { StatisticsFilter } from 'models/statisticsFilter';


abstract class IStatisticsService {
    abstract getPaymentStatistics(filter?: StatisticsFilter): Observable<Array<PaymentStatsResponse>>;
}

export { IStatisticsService };