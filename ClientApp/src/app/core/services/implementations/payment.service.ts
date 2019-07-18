import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IPaymentApiBackendService } from 'services/IPaymentApi.backend';
import { IPaymentService } from 'services/IPaymentService';

import { AddPaymentRequest } from 'models/AddPaymentRequest';
import { AddPaymentTypeRequest } from 'models/addPaymentTypeRequest';
import { PaymentResponse } from 'models/PaymentResponse';
import { PaymentTypeResponse } from 'models/paymentTypeResponse';

@Injectable()
class PaymentService implements IPaymentService {

    constructor(
        private paymentApiBackend: IPaymentApiBackendService,
        // private notificationService: INotificationService,
        // private loggingService: ILoggingService
    ) { }

    public addPaymentType(paymentTypeData: AddPaymentTypeRequest): void {
        // data validation

        this.paymentApiBackend
            .addPaymentType(paymentTypeData)
            .pipe(
                tap(response => {
                    const hasError: boolean =
                        !isNullOrUndefined(response);

                    if (hasError) {
                        // this.notificationService.error();
                        // this.loggingService.error()
                    }
                }),
                map(response => !isNullOrUndefined(response))
            );
    }

    public addPayment(paymentData: AddPaymentRequest): void {
        // data validation

        this.paymentApiBackend
            .addPayment(paymentData)
            .pipe(
                tap(response => {
                    const hasError: boolean =
                        !isNullOrUndefined(response);

                    if (hasError) {
                        // this.notificationService.error();
                        // this.loggingService.error()
                    }
                }),
                map(response => !isNullOrUndefined(response))
            );
    }

    public getPaymentTypes(): Observable<Array<PaymentTypeResponse>> {
        return this.paymentApiBackend
            .getPaymentTypes()
            .pipe(
                map(response => {
                    const hasError: boolean =
                        isNullOrUndefined(response) || !(response instanceof Array);

                    if (hasError) {
                        // this.notificationService.error();
                        // this.loggingService.error(response);
                    }

                    return hasError ? [] : response;
                }),
            );
    }

    public getPayments(): Observable<Array<PaymentResponse>> {
        return this.paymentApiBackend
            .getPayments()
            .pipe(
                map(response => {
                    const hasError: boolean =
                        isNullOrUndefined(response) || !(response instanceof Array);

                    if (hasError) {
                        // this.notificationService.error();
                        // this.loggingService.error(response);
                    }

                    return hasError ? [] : response;
                }),
            );
    }
}

export { PaymentService };