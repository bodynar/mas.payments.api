import { Observable } from 'rxjs';

import { PaymentsFilter } from 'models/paymentsFilter';
import { AddPaymentRequest } from 'models/request/addPaymentRequest';
import { AddPaymentTypeRequest } from 'models/request/addPaymentTypeRequest';
import CommandExecutionResult from 'models/response/commandExecutionResult';
import PaymentResponse from 'models/response/payments/paymentResponse';
import PaymentTypeResponse from 'models/response/payments/paymentTypeResponse';

abstract class IPaymentService {
    abstract getPayment(id: number): Observable<PaymentResponse>;

    abstract getPaymentType(id: number): Observable<PaymentTypeResponse>;

    abstract addPaymentType(paymentTypeData: AddPaymentTypeRequest): Observable<CommandExecutionResult>;

    abstract addPayment(paymentData: AddPaymentRequest): Observable<CommandExecutionResult>;

    abstract updatePaymentType(id: number, paymentTypeData: AddPaymentTypeRequest): Observable<CommandExecutionResult>;

    abstract updatePayment(id: number, paymentData: AddPaymentRequest): Observable<CommandExecutionResult>;

    abstract getPaymentTypes(): Observable<Array<PaymentTypeResponse>>;

    abstract getPayments(filter?: PaymentsFilter): Observable<Array<PaymentResponse>>;

    abstract deletePaymentType(paymentTypeId: number): Observable<CommandExecutionResult>;

    abstract deletePayment(paymentId: number): Observable<CommandExecutionResult>;
}

export { IPaymentService };