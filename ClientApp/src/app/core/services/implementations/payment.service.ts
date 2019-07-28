import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IPaymentApiBackendService } from 'services/backend/IPaymentApi.backend';
import { IPaymentService } from 'services/IPaymentService';

import { PaymentsFilter } from 'models/paymentsFilter';
import { AddPaymentRequest } from 'models/request/addPaymentRequest';
import { AddPaymentTypeRequest } from 'models/request/addPaymentTypeRequest';
import { PaymentResponse } from 'models/response/paymentResponse';
import { PaymentTypeResponse } from 'models/response/paymentTypeResponse';

@Injectable()
class PaymentService implements IPaymentService {

    constructor(
        private paymentApiBackend: IPaymentApiBackendService,
        // private notificationService: INotificationService,
        // private loggingService: ILoggingService
    ) { }

    public addPaymentType(paymentTypeData: AddPaymentTypeRequest): Observable<boolean> {
        // data validation

        return this.paymentApiBackend
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

    public addPayment(paymentData: AddPaymentRequest): Observable<boolean> {
        // data validation

        return this.paymentApiBackend
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

    public getPayments(filter?: PaymentsFilter): Observable<Array<PaymentResponse>> {
        return this.paymentApiBackend
            .getPayments(filter)
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