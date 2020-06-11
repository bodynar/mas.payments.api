import { Observable } from 'rxjs';

import PaymentsFilter from 'models/paymentsFilter';
import { AddPaymentRequest } from 'models/request/payment/addPaymentRequest';
import { AddPaymentTypeRequest } from 'models/request/payment/addPaymentTypeRequest';
import CommandExecutionResult from 'models/response/commandExecutionResult';
import PaymentResponse from 'models/response/payments/paymentResponse';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';
import QueryExecutionResult from 'models/response/queryExecutionResult';

abstract class IPaymentService {
    abstract getPayment(id: number): Observable<QueryExecutionResult<PaymentResponse>>;

    abstract getPaymentType(id: number): Observable<QueryExecutionResult<PaymentTypeResponse>>;

    abstract addPaymentType(paymentTypeData: AddPaymentTypeRequest): Observable<CommandExecutionResult>;

    abstract addPayment(paymentData: AddPaymentRequest): Observable<CommandExecutionResult>;

    abstract updatePaymentType(id: number, paymentTypeData: AddPaymentTypeRequest): Observable<CommandExecutionResult>;

    abstract updatePayment(id: number, paymentData: AddPaymentRequest): Observable<CommandExecutionResult>;

    abstract getPaymentTypes(): Observable<QueryExecutionResult<Array<PaymentTypeResponse>>>;

    abstract getPayments(filter?: PaymentsFilter): Observable<QueryExecutionResult<Array<PaymentResponse>>>;

    abstract deletePaymentType(paymentTypeId: number): Observable<CommandExecutionResult>;

    abstract deletePayment(paymentId: number): Observable<CommandExecutionResult>;
}

export { IPaymentService };