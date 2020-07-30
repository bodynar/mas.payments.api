import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IPaymentApiBackendService } from '../../contracts/backend/IPaymentApi.backend';

import PaymentsFilter from 'models/paymentsFilter';
import { AddPaymentRequest } from 'models/request/payment/addPaymentRequest';
import { AddPaymentTypeRequest } from 'models/request/payment/addPaymentTypeRequest';

import CommandExecutionResult from 'models/response/commandExecutionResult';
import PaymentResponse from 'models/response/payments/paymentResponse';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';
import QueryExecutionResult from 'models/response/queryExecutionResult';

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
                catchError(error => of(error.error)),
                map(x => x
                    ? (({
                        success: false,
                        error: x['Message'],
                    }) as CommandExecutionResult)
                    : ({ success: true })
                ),
            );
    }

    public getPayments(filter?: PaymentsFilter): Observable<QueryExecutionResult<Array<PaymentResponse>>> {
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
                        paymentTypeId: payment['paymentTypeId']
                    }) as PaymentResponse)),
                catchError(error => of(error.error)),
                map(x => isNullOrUndefined(x.Success)
                    ? ({
                        success: true,
                        result: x
                    })
                    : ({
                        success: false,
                        error: x['Message'],
                        result: [],
                    })
                ),
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

    public updatePayment(id: number, paymentData: AddPaymentRequest): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/updatePayment`, { id, ...paymentData })
            .pipe(
                catchError(error => of(error.error)),
                map(x => x
                    ? (({
                        success: false,
                        error: x['Message'],
                    }) as CommandExecutionResult)
                    : ({ success: true })
                ),
            );
    }

    public deletePayment(paymentId: number): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/deletePayment`, paymentId)
            .pipe(
                catchError(error => of(error.error)),
                map(x => x
                    ? (({
                        success: false,
                        error: x['Message'],
                    }) as CommandExecutionResult)
                    : ({ success: true })
                ),
            );
    }

    // #endregion payments

    // #region payment type

    public addPaymentType(paymentTypeData: AddPaymentTypeRequest): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/addPaymentType`, paymentTypeData)
            .pipe(
                catchError(error => of(error.error)),
                map(x => x
                    ? (({
                        success: false,
                        error: x['Message'],
                    }) as CommandExecutionResult)
                    : ({ success: true })
                ),
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
                        hasRelatedPayments: paymentType['hasRelatedPayments'] || false,
                        hasRelatedMeasurementTypes: paymentType['hasRelatedMeasurementTypes'] || false,
                    }) as PaymentTypeResponse)),
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
                    }) as PaymentTypeResponse),
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

    public updatePaymentType(id: number, paymentTypeData: AddPaymentTypeRequest): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/updatePaymentType`, { id, ...paymentTypeData })
            .pipe(
                catchError(error => of(error.error)),
                map(x => x
                    ? (({
                        success: false,
                        error: x['Message'],
                    }) as CommandExecutionResult)
                    : ({ success: true })
                ),
            );
    }

    public deletePaymentType(paymentTypeId: number): Observable<CommandExecutionResult> {
        return this.http
            .post(`${this.apiPrefix}/deletePaymentType`, paymentTypeId)
            .pipe(
                catchError(error => of(error.error)),
                map(x => x
                    ? (({
                        success: false,
                        error: x['Message'],
                    }) as CommandExecutionResult)
                    : ({ success: true })
                ),
            );
    }

    // #endregion payment type
}