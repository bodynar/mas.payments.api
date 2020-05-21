import { Observable } from 'rxjs';

import { GetPaymentStatsResponse } from 'models/response/payments/paymentStatsResponse';
import { StatisticsFilter } from 'models/statisticsFilter';

abstract class IStatisticsApiBackendService {
    abstract getPaymentStatistics(filter?: StatisticsFilter): Observable<GetPaymentStatsResponse>;
}

export { IStatisticsApiBackendService };