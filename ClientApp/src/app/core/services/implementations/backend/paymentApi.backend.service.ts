import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { isNullOrUndefined } from 'common/utils/common';

import CommandExecutionResult from 'models/response/commandExecutionResult';
import QueryExecutionResult from 'models/response/queryExecutionResult';

import { boxServerResponse, boxServerQueryResponse } from 'common/utils/api';

import { IPaymentApiBackendService } from '../../contracts/backend/IPaymentApi.backend';

import { PaymentFilter, AddPaymentRequest, AddPaymentTypeRequest } from 'models/request/payment';

import PaymentResponse from 'models/response/payments/paymentResponse';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';

@Injectable()
export class PaymentApiBackendService implements IPaymentApiBackendService {

    private readonly apiPrefix: string =
        '/api/payment';

    constructor(
        private http: HttpClient
    ) {
    }

    // #region payments

    public addPayment(paymentData: AddPaymentRequest): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/addPayment`, paymentData)
            .pipe(
                catchError(error => of(error)),
                map(x => boxServerResponse(x)),
            );
    }

    public getPayments(filter?: PaymentFilter): Observable<QueryExecutionResult<Array<PaymentResponse>>> {
        let params: HttpParams =
            new HttpParams();

        if (!isNullOrUndefined(filter)) {
            if (!isNullOrUndefined(filter.month)) {
                params = params.set('month', `${filter.month + 1}`);
            }
            if (!isNullOrUndefined(filter.year)) {
                params = params.set('year', `${filter.year}`);
            }
            if (!isNullOrUndefined(filter.paymentTypeId)) {
                params = params.set('paymentTypeId', `${filter.paymentTypeId}`);
            }
            if (!isNullOrUndefined(filter.amount)) {
                if (!isNullOrUndefined(filter.amount.exact)) {
                    params = params.set('amount.exact', `${filter.amount.exact}`);
                } else {
                    if (!isNullOrUndefined(filter.amount.min)) {
                        params = params.set('amount.min', `${filter.amount.min}`);
                    }
                    if (!isNullOrUndefined(filter.amount.max)) {
                        params = params.set('amount.max', `${filter.amount.max}`);
                    }
                }
            }
        }

        const headers: HttpHeaders =
            new HttpHeaders({
                'Content-Type': 'application/json'
            });

        return this.http
            .get(`${this.apiPrefix}/getPayments`, { headers, params })
            .pipe(
                map((response: Array<any>) =>
                    response.map(payment => ({
                        id: payment['id'],
                        amount: payment['amount'],
                        month: payment['dateMonth'],
                        year: payment['dateYear'],
                        description: payment['description'],
                        paymentTypeName: payment['paymentTypeName'],
                        paymentTypeId: payment['paymentTypeId'],
                        paymentTypeColor: payment['paymentTypeColor'],
                    }) as PaymentResponse)),
                catchError(error => of(error)),
                map(x => boxServerQueryResponse<Array<PaymentResponse>>(x)),
            );
    }

    public getPayment(id: number): Observable<QueryExecutionResult<PaymentResponse>> {
        return this.http
            .get(`${this.apiPrefix}/getPayment`, {
                params: new HttpParams({
                    fromObject: { id: `${id}` }
                })
            })
            .pipe(
                map((response: any) =>
                    ({
                        id: response['id'],
                        amount: response['amount'],
                        month: response['dateMonth'],
                        year: response['dateYear'],
                        description: response['description'],
                        paymentTypeName: response['paymentTypeName'],
                        paymentTypeId: response['paymentTypeId'],
                    }) as PaymentResponse),
                catchError(error => of(error)),
                map(x => boxServerQueryResponse<PaymentResponse>(x)),
            );
    }

    public updatePayment(id: number, paymentData: AddPaymentRequest): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/updatePayment`, { id, ...paymentData })
            .pipe(
                catchError(error => of(error)),
                map(x => boxServerResponse(x)),
            );
    }

    public deletePayment(paymentId: number): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/deletePayment`, paymentId)
            .pipe(
                catchError(error => of(error)),
                map(x => boxServerResponse(x)),
            );
    }

    // #endregion payments

    // #region payment type

    public addPaymentType(paymentTypeData: AddPaymentTypeRequest): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/addPaymentType`, paymentTypeData)
            .pipe(
                catchError(error => of(error)),
                map(x => boxServerResponse(x)),
            );
    }

    public getPaymentTypes(): Observable<QueryExecutionResult<Array<PaymentTypeResponse>>> {
        return this.http
            .get(`${this.apiPrefix}/getPaymentTypes`)
            .pipe(
                map((response: Array<any>) =>
                    response.map(paymentType => ({
                        id: paymentType['id'],
                        name: paymentType['name'],
                        systemName: response['systemName'],
                        description: paymentType['description'],
                        company: paymentType['company'],
                        color: paymentType['color'],
                        hasRelatedPayments: paymentType['hasRelatedPayments'] || false,
                        hasRelatedMeasurementTypes: paymentType['hasRelatedMeasurementTypes'] || false,
                    }) as PaymentTypeResponse)),
                catchError(error => of(error)),
                map(x => boxServerQueryResponse<Array<PaymentTypeResponse>>(x)),
            );
    }

    public getPaymentType(id: number): Observable<QueryExecutionResult<PaymentTypeResponse>> {
        return this.http
            .get(`${this.apiPrefix}/getPaymentType`, {
                params: new HttpParams({
                    fromObject: { id: `${id}` }
                })
            })
            .pipe(
                map((response: any) =>
                    ({
                        id: response['id'],
                        name: response['name'],
                        company: response['company'],
                        systemName: response['systemName'],
                        description: response['description'],
                        color: response['color'],
                    }) as PaymentTypeResponse),
                catchError(error => of(error)),
                map(x => boxServerQueryResponse<PaymentTypeResponse>(x)),
            );
    }

    public updatePaymentType(id: number, paymentTypeData: AddPaymentTypeRequest): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/updatePaymentType`, { id, ...paymentTypeData })
            .pipe(
                catchError(error => of(error)),
                map(x => boxServerResponse(x)),
            );
    }

    public deletePaymentType(paymentTypeId: number): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/deletePaymentType`, paymentTypeId)
            .pipe(
                catchError(error => of(error)),
                map(x => boxServerResponse(x)),
            );
    }

    // #endregion payment type
}