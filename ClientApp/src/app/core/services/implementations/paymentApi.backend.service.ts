import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { IPaymentApiBackendService } from '../contracts/IPaymentApi.backend';

import { AddPaymentRequest } from 'models/addPaymentRequest';
import { AddPaymentTypeRequest } from 'models/addPaymentTypeRequest';
import { PaymentResponse } from 'models/paymentResponse';
import { PaymentTypeResponse } from 'models/paymentTypeResponse';

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

    public getPayments(): Observable<Array<PaymentResponse>> {
        return this.http
            .get(`${this.apiPrefix}/getPayments`)
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