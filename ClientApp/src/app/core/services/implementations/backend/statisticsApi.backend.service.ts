import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { isNullOrUndefined } from 'common/utils/common';
import { boxServerQueryResponse } from 'common/utils/api';

import { IStatisticsApiBackendService } from 'services/backend/IStatisticsApi.backend';

import QueryExecutionResult from 'models/response/queryExecutionResult';

import MeasurementStatisticsFilter from 'models/request/stats/measurementStatisticsFilter';
import PaymentStatisticsFilter from 'models/request/stats/paymentStatisticsFilter';
import {
    GetMeasurementStatisticsDataItem, GetMeasurementStatisticsResponse, MeasurementTypeStatisticsItem,
    GetPaymentsStatisticsDataItem, GetPaymentsStatisticsResponse, PaymentTypeStatisticsItem
} from 'models/response/stats';

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

        if (!isNullOrUndefined(filter.from)) {
            params = params.set('from', `${new Date(filter.from.year, filter.from.month).toDateString()}`);
        }
        if (!isNullOrUndefined(filter.to)) {
            params = params.set('to', `${new Date(filter.to.year, filter.to.month).toDateString()}`);
        }
        if (!isNullOrUndefined(filter.paymentTypeId) && filter.paymentTypeId !== 0) {
            params = params.set('paymentTypeId', `${filter.paymentTypeId}`);
        }

        return this.http
            .get(`${this.apiPrefix}/getPaymentsStatistics`, { headers: this.headers, params })
            .pipe(
                map((response: any) =>
                    ({
                        from: isNullOrUndefined(response['from']) ? undefined : new Date(response['from']),
                        to: isNullOrUndefined(response['to']) ? undefined : new Date(response['to']),
                        typeStatistics: (response['typeStatistics'] || []).map(typeItem => ({
                            paymentTypeId: typeItem['paymentTypeId'],
                            paymentTypeName: typeItem['paymentTypeName'],
                            statisticsData: (typeItem['statisticsData'] || []).map(statsItem => ({
                                amount: statsItem['amount'],
                                month: statsItem['month'],
                                year: statsItem['year']
                            }) as GetPaymentsStatisticsDataItem)
                        }) as PaymentTypeStatisticsItem)
                    }) as GetPaymentsStatisticsResponse),
                catchError(error => of(error)),
                map(x => boxServerQueryResponse<GetPaymentsStatisticsResponse>(x))
            );
    }

    public getMeasurementStatistics(filter: MeasurementStatisticsFilter)
        : Observable<QueryExecutionResult<GetMeasurementStatisticsResponse>> {
        let params: HttpParams =
            new HttpParams();

        if (!isNullOrUndefined(filter.from)) {
            params = params.set('from', `${new Date(filter.from.year, filter.from.month).toDateString()}`);
        }
        if (!isNullOrUndefined(filter.to)) {
            params = params.set('to', `${new Date(filter.to.year, filter.to.month).toDateString()}`);
        }
        if (!isNullOrUndefined(filter.measurementTypeId) && filter.measurementTypeId !== 0) {
            params = params.set('measurementTypeId', `${filter.measurementTypeId}`);
        }

        return this.http
            .get(`${this.apiPrefix}/getMeasurementStatistics`, { headers: this.headers, params })
            .pipe(
                map((response: any) =>
                    ({
                        from: isNullOrUndefined(response['from']) ? undefined : new Date(response['from']),
                        to: isNullOrUndefined(response['to']) ? undefined : new Date(response['to']),
                        typeStatistics: (response['typeStatistics'] || []).map(typeItem => ({
                            measurementTypeId: typeItem['measurementTypeId'],
                            measurementTypeName: typeItem['measurementTypeName'],
                            statisticsData: (typeItem['statisticsData'] || []).map(dataItem => ({
                                month: dataItem['month'],
                                year: dataItem['year'],
                                diff: dataItem['measurementDiff'] || null,
                            }) as GetMeasurementStatisticsDataItem)
                        }) as MeasurementTypeStatisticsItem)
                    }) as GetMeasurementStatisticsResponse),
                catchError(error => of(error)),
                map(x => boxServerQueryResponse<GetMeasurementStatisticsResponse>(x))
            );
    }
}

export { StatisticsApiBackendService };