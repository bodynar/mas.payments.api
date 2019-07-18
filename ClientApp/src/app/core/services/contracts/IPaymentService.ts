import { Observable } from 'rxjs';

import { AddPaymentRequest } from 'models/AddPaymentRequest';
import { AddPaymentTypeRequest } from 'models/addPaymentTypeRequest';
import { PaymentResponse } from 'models/PaymentResponse';
import { PaymentTypeResponse } from 'models/paymentTypeResponse';

abstract class IPaymentService {
    abstract addPaymentType(paymentTypeData: AddPaymentTypeRequest): void;

    abstract addPayment(paymentData: AddPaymentRequest): void;

    abstract getPaymentTypes(): Observable<Array<PaymentTypeResponse>>;

    abstract getPayments(): Observable<Array<PaymentResponse>>;
}

export { IPaymentService };