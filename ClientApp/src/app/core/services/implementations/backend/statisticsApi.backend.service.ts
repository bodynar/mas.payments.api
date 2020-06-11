import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IStatisticsApiBackendService } from 'services/backend/IStatisticsApi.backend';

import MeasurementStatisticsFilter from 'models/request/stats/measurementStatisticsFilter';
import PaymentStatisticsFilter from 'models/request/stats/paymentStatisticsFilter';
import QueryExecutionResult from 'models/response/queryExecutionResult';
import { GetMeasurementStatisticsDataItem, GetMeasurementStatisticsResponse } from 'models/response/stats/measurementStatsResponse';
import { GetPaymentsStatisticsDataItem, GetPaymentsStatisticsResponse } from 'models/response/stats/paymentStatsResponse';

@Injectable()
class StatisticsApiBackendService implements IStatisticsApiBackendService {

    private readonly apiPrefix: string =
        '/api/stats';

    private readonly headers: HttpHeaders =
        new HttpHeaders({
            'Content-Type': 'application/json'
        });

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

        return this.http
            .get(`${this.apiPrefix}/getPaymentsStatistics`, { headers: this.headers, params })
            .pipe(
                map((response: any) =>
                    ({
                        year: response['year'],
                        paymentTypeId: response['paymentTypeId'],
                        statisticsData: (response['statisticsData'] || []).map(dataItem => ({
                            month: dataItem['month'],
                            year: dataItem['year'],
                            amount: dataItem['amount'] || null,
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

    public getMeasurementStatistics(filter: MeasurementStatisticsFilter)
        : Observable<QueryExecutionResult<GetMeasurementStatisticsResponse>> {
        let params: HttpParams =
            new HttpParams();

        if (!isNullOrUndefined(filter.year)) {
            params = params.set('year', `${filter.year}`);
        }
        if (!isNullOrUndefined(filter.measurementTypeId)) {
            params = params.set('measurementTypeId', `${filter.measurementTypeId}`);
        }

        return this.http
            .get(`${this.apiPrefix}/getMeasurementStatistics`, { headers: this.headers, params })
            .pipe(
                map((response: any) =>
                    ({
                        year: response['year'],
                        measurementTypeId: response['measurementTypeId'],
                        statisticsData: (response['statisticsData'] || []).map(dataItem => ({
                            month: dataItem['month'],
                            year: dataItem['year'],
                            diff: dataItem['measurementDiff'] || null,
                        }) as GetMeasurementStatisticsDataItem)
                    }) as GetMeasurementStatisticsResponse),
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