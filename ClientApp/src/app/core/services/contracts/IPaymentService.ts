import { Observable } from 'rxjs';

import { PaymentsFilter } from 'models/paymentsFilter';
import { AddPaymentRequest } from 'models/request/addPaymentRequest';
import { AddPaymentTypeRequest } from 'models/request/addPaymentTypeRequest';
import { PaymentResponse } from 'models/response/paymentResponse';
import { PaymentTypeResponse } from 'models/response/paymentTypeResponse';

abstract class IPaymentService {
    abstract getPayment(id: number): Observable<PaymentResponse>;

    abstract getPaymentType(id: number): Observable<PaymentTypeResponse>;

    abstract addPaymentType(paymentTypeData: AddPaymentTypeRequest): Observable<boolean>;

    abstract addPayment(paymentData: AddPaymentRequest): Observable<boolean>;

    abstract updatePaymentType(id: number, paymentTypeData: AddPaymentTypeRequest): Observable<boolean>;

    abstract updatePayment(id: number, paymentData: AddPaymentRequest): Observable<boolean>;

    abstract getPaymentTypes(): Observable<Array<PaymentTypeResponse>>;

    abstract getPayments(filter?: PaymentsFilter): Observable<Array<PaymentResponse>>;

    abstract deletePaymentType(paymentTypeId: number): Observable<boolean>;

    abstract deletePayment(paymentId: number): Observable<boolean>;
}

export { IPaymentService };