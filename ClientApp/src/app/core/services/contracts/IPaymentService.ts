import { Observable } from 'rxjs';

import CommandExecutionResult from 'models/response/commandExecutionResult';
import QueryExecutionResult from 'models/response/queryExecutionResult';

import { PaymentFilter, AddPaymentRequest, AddPaymentTypeRequest } from 'models/request/payment';

import PaymentResponse from 'models/response/payments/paymentResponse';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';

abstract class IPaymentService {
    abstract getPayments(filter?: PaymentFilter): Observable<QueryExecutionResult<Array<PaymentResponse>>>;

    abstract addPayment(paymentData: AddPaymentRequest): Observable<CommandExecutionResult>;

    abstract getPayment(id: number): Observable<QueryExecutionResult<PaymentResponse>>;

    abstract updatePayment(id: number, paymentData: AddPaymentRequest): Observable<CommandExecutionResult>;

    abstract deletePayment(paymentId: number): Observable<CommandExecutionResult>;


    abstract getPaymentTypes(): Observable<QueryExecutionResult<Array<PaymentTypeResponse>>>;

    abstract addPaymentType(paymentTypeData: AddPaymentTypeRequest): Observable<CommandExecutionResult>;

    abstract getPaymentType(id: number): Observable<QueryExecutionResult<PaymentTypeResponse>>;

    abstract updatePaymentType(id: number, paymentTypeData: AddPaymentTypeRequest): Observable<CommandExecutionResult>;

    abstract deletePaymentType(paymentTypeId: number): Observable<CommandExecutionResult>;
}

export { IPaymentService };