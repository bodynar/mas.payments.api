import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'util';

import { IPaymentApiBackendService } from 'services/backend/IPaymentApi.backend';
import { IPaymentService } from 'services/IPaymentService';

import { PaymentsFilter } from 'models/paymentsFilter';
import { AddPaymentRequest } from 'models/request/addPaymentRequest';
import { AddPaymentTypeRequest } from 'models/request/addPaymentTypeRequest';
import CommandExecutionResult from 'models/response/commandExecutionResult';
import PaymentResponse from 'models/response/payments/paymentResponse';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';

@Injectable()
class PaymentService implements IPaymentService {

    constructor(
        private paymentApiBackend: IPaymentApiBackendService,
        // private notificationService: INotificationService,
        // private loggingService: ILoggingService
    ) { }

    // #region payments

    public addPayment(paymentData: AddPaymentRequest): Observable<CommandExecutionResult> {
        // data validation
        const parsedMonth: number =
            parseInt(paymentData.month) + 1;
        const month: number =
          parsedMonth > 12
            ? parsedMonth % 12
            : parsedMonth;

        return this.paymentApiBackend
            .addPayment({
              ...paymentData,
              month: month.toString()
            })
            .pipe(
                tap(withoutError => {
                    if (!withoutError) {
                        // this.loggingService.error()
                    }
                }),
            );
    }

    public getPayments(filter?: PaymentsFilter): Observable<Array<PaymentResponse>> {
        return this.paymentApiBackend
            .getPayments(filter)
            .pipe(
                map(response => {
                    const hasError: CommandExecutionResult =
                        isNullOrUndefined(response) || !(response instanceof Array);

                    if (hasError) {
                        // this.notificationService.error();
                        // this.loggingService.error(response);
                    }

                    return hasError ? [] : response;
                }),
            );
    }

    public getPayment(id: number): Observable<PaymentResponse> {
        return this.paymentApiBackend.getPayment(id);
    }

    public updatePayment(id: number, paymentData: AddPaymentRequest): Observable<CommandExecutionResult> {
        return this.paymentApiBackend
            .updatePayment(id, paymentData)
            .pipe(
                tap(withoutError => {
                    if (!withoutError) {
                        // this.loggingService.error()
                    }
                }),
            );
    }

    public deletePayment(paymentId: number): Observable<CommandExecutionResult> {
        return this.paymentApiBackend
            .deletePayment(paymentId);
    }

    // #endregion payments

    // #region payment types

    public addPaymentType(paymentTypeData: AddPaymentTypeRequest): Observable<CommandExecutionResult> {
        // data validation

        return this.paymentApiBackend
            .addPaymentType(paymentTypeData)
            .pipe(
                tap(withoutError => {
                    if (!withoutError) {
                        // this.loggingService.error()
                    }
                }),
            );
    }

    public getPaymentTypes(): Observable<Array<PaymentTypeResponse>> {
        return this.paymentApiBackend
            .getPaymentTypes()
            .pipe(
                map(response => {
                    const hasError: CommandExecutionResult =
                        isNullOrUndefined(response) || !(response instanceof Array);

                    if (hasError) {
                        // this.notificationService.error();
                        // this.loggingService.error(response);
                    }

                    return hasError ? [] : response;
                }),
            );
    }

    public getPaymentType(id: number): Observable<PaymentTypeResponse> {
        return this.paymentApiBackend.getPaymentType(id);
    }

    public updatePaymentType(id: number, paymentTypeData: AddPaymentTypeRequest): Observable<CommandExecutionResult> {
        return this.paymentApiBackend
            .updatePaymentType(id, paymentTypeData)
            .pipe(
                tap(withoutError => {
                    if (!withoutError) {
                        // this.loggingService.error()
                    }
                }),
            );
    }

    public deletePaymentType(paymentTypeId: number): Observable<CommandExecutionResult> {
        return this.paymentApiBackend
            .deletePaymentType(paymentTypeId);
    }

    // #endregion payment types
}

export { PaymentService };