import { Observable } from 'rxjs';

import { GetPaymentStatsResponse } from 'models/response/paymentStatsResponse';
import { StatisticsFilter } from 'models/statisticsFilter';

abstract class IStatisticsApiBackendService {
    abstract getPaymentStatistics(filter?: StatisticsFilter): Observable<GetPaymentStatsResponse>;
}

export { IStatisticsApiBackendService };