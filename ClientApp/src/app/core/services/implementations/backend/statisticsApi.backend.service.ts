import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IStatisticsApiBackendService } from 'services/backend/IStatisticsApi.backend';

import { PaymentStatsResponse } from 'models/response/paymentStatsResponse';
import { StatisticsFilter } from 'models/statisticsFilter';


@Injectable()
class StatisticsApiBackendService implements IStatisticsApiBackendService {

    private readonly apiPrefix: string =
        '/api/stats';

    constructor(
        private http: HttpClient
    ) {
    }

    public getPaymentStatistics(filter?: StatisticsFilter): Observable<Array<PaymentStatsResponse>> {
        let params: HttpParams =
            new HttpParams().set('includeMeasurements', `${filter.includeMeasurements}`);

        if (!isNullOrUndefined(filter)) {
            if (!isNullOrUndefined(filter.year)) {
                params = params.set('year', `${filter.year}`);
            }
            if (!isNullOrUndefined(filter.from)) {
                params = params.set('from', `${filter.from}`);
            }
            if (!isNullOrUndefined(filter.to)) {
                params = params.set('to', `${filter.to}`);
            }
        }

        const headers: HttpHeaders =
            new HttpHeaders({
                'Content-Type': 'application/json'
            });
        return this.http
            .get(`${this.apiPrefix}/getStatistics`, { headers, params })
            .pipe(
                map((response: Array<any>) =>
                    response.map(payment => ({
                        paymentTypeName: payment['paymentTypeName'],
                        paymentTypeId: payment['paymentTypeId'],
                        payments: payment['payments']
                    }) as PaymentStatsResponse)),
                catchError(error => of(error))
            );
    }
}

export { StatisticsApiBackendService };