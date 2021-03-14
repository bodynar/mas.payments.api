import { Injectable } from '@angular/core';

import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

import { isNullOrUndefined } from 'common/utils/common';

import { emptyMonth } from 'static/months';
import { emptyYear } from 'common/utils/years';

import CommandExecutionResult from 'models/response/commandExecutionResult';
import QueryExecutionResult from 'models/response/queryExecutionResult';

import { IPaymentApiBackendService } from 'services/backend/IPaymentApi.backend';
import { IPaymentService } from 'services/IPaymentService';

import { PaymentFilter, AddPaymentRequest, AddPaymentTypeRequest } from 'models/request/payment';

import PaymentResponse from 'models/response/payments/paymentResponse';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';


@Injectable()
class PaymentService implements IPaymentService {

    constructor(
        private paymentApiBackend: IPaymentApiBackendService,
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

    public getPayments(filter?: PaymentFilter): Observable<QueryExecutionResult<Array<PaymentResponse>>> {
        const requestData: PaymentFilter = { ...filter };

        if (!isNullOrUndefined(filter)) {
            if (filter.month === emptyMonth.id) {
                requestData.month = undefined;
            }
            if (filter.year === emptyYear.id) {
                requestData.year = undefined;
            }
        }

        return this.paymentApiBackend
            .getPayments(requestData)
            .pipe(
                tap(response => {
                    if (!response.success) {
                        // this.loggingService.error(response);
                    }
                }),
            );
    }

    public getPayment(id: number): Observable<QueryExecutionResult<PaymentResponse>> {
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

    public getPaymentTypes(): Observable<QueryExecutionResult<Array<PaymentTypeResponse>>> {
        return this.paymentApiBackend
            .getPaymentTypes()
            .pipe(
                tap(response => {
                    if (!response.success) {
                        // this.loggingService.error(response);
                    }
                }),
                map(response =>
                    response.success
                        ? ({
                            ...response,
                            result: response.result.map(item => ({ ...item, hasColor: !isNullOrUndefined(item.color) }))
                        })
                        : response
                )
            );
    }

    public getPaymentType(id: number): Observable<QueryExecutionResult<PaymentTypeResponse>> {
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