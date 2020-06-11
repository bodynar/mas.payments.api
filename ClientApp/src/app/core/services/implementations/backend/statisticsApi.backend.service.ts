import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IStatisticsApiBackendService } from 'services/backend/IStatisticsApi.backend';

import MeasurementStatisticsFilter from 'models/request/stats/measurementStatisticsFilter';
import PaymentStatisticsFilter from 'models/request/stats/paymentStatisticsFilter';
import QueryExecutionResult from 'models/response/queryExecutionResult';

@Injectable()
class StatisticsApiBackendService implements IStatisticsApiBackendService {

    private readonly apiPrefix: string =
        '/api/stats';

    constructor(
        private http: HttpClient
    ) {
    }

    public getPaymentStatistics(filter: PaymentStatisticsFilter): Observable<QueryExecutionResult<GetPaymentsStatisticsResponse>> {
        let params: HttpParams =
            new HttpParams();

        if (!isNullOrUndefined(filter.year)) {
            params = params.set('year', `${filter.year}`);
        }
        if (!isNullOrUndefined(filter.paymentTypeId)) {
            params = params.set('paymentTypeId', `${filter.paymentTypeId}`);
        }

        const headers: HttpHeaders =
            new HttpHeaders({
                'Content-Type': 'application/json'
            });

        return this.http
            .get(`${this.apiPrefix}/getPaymentsStatistics`, { headers, params })
            .pipe(
                map((response: any) =>
                    ({
                        year: response['year'],
                        paymentTypeId: response['paymentTypeId'],
                        statisticsData: (response['statisticsData'] || []).map(dataItem => ({
                            month: dataItem['month'],
                            year: dataItem['year'],
                            amount: dataItem['amount'],
                        }) as GetPaymentsStatisticsDataItem)
                    }) as GetPaymentsStatisticsResponse),
                catchError(error => of(error.error)),
                map(x => isNullOrUndefined(x.Success)
                    ? ({
                        success: true,
                        result: x
                    })
                    : ({
                        success: false,
                        error: x['Message'],
                    })
                ),
            );
    }
}

export { StatisticsApiBackendService };