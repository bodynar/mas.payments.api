import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IPaymentApiBackendService } from '../../contracts/backend/IPaymentApi.backend';

import { PaymentsFilter } from 'models/paymentsFilter';
import { AddPaymentRequest } from 'models/request/addPaymentRequest';
import { AddPaymentTypeRequest } from 'models/request/addPaymentTypeRequest';
import { PaymentResponse } from 'models/response/paymentResponse';
import { PaymentTypeResponse } from 'models/response/paymentTypeResponse';

@Injectable()
class PaymentApiBackendService implements IPaymentApiBackendService {

    private readonly apiPrefix: string =
        '/api/payment';

    constructor(
        private http: HttpClient
    ) {
    }

    public addPaymentType(paymentTypeData: AddPaymentTypeRequest): Observable<any> {
        return this.http
            .post(`${this.apiPrefix}/addPaymentType`, paymentTypeData)
            .pipe(catchError(error => of(error)));
    }

    public addPayment(paymentData: AddPaymentRequest): Observable<any> {
        return this.http
            .post(`${this.apiPrefix}/addPayment`, paymentData)
            .pipe(catchError(error => of(error)));
    }

    public getPaymentTypes(): Observable<Array<PaymentTypeResponse>> {
        return this.http
            .get(`${this.apiPrefix}/getPaymentTypes`)
            .pipe(
                map((response: Array<any>) =>
                    response.map(paymentType => ({
                        id: paymentType['id'],
                        name: paymentType['name'],
                        description: paymentType['description'],
                        company: paymentType['company']
                    }) as PaymentTypeResponse)),
                catchError(error => of(error))
            );
    }

    public getPayments(filter?: PaymentsFilter): Observable<Array<PaymentResponse>> {
        let params: HttpParams =
            new HttpParams();

        if (!isNullOrUndefined(filter)) {
            if (!isNullOrUndefined(filter.month)) {
                params = params.set('month', `${filter.month}`);
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
                        date: payment['date'],
                        description: payment['description'],
                        paymentType: payment['paymentType']
                    }) as PaymentResponse)),
                catchError(error => of(error))
            );
    }
}

export { PaymentApiBackendService };