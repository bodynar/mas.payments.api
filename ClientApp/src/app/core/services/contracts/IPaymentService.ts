import { Observable } from 'rxjs';

import { AddPaymentRequest } from 'models/addPaymentRequest';
import { AddPaymentTypeRequest } from 'models/addPaymentTypeRequest';
import { PaymentResponse } from 'models/paymentResponse';
import { PaymentTypeResponse } from 'models/paymentTypeResponse';

abstract class IPaymentService {
    abstract addPaymentType(paymentTypeData: AddPaymentTypeRequest): void;

    abstract addPayment(paymentData: AddPaymentRequest): void;

    abstract getPaymentTypes(): Observable<Array<PaymentTypeResponse>>;

    abstract getPayments(): Observable<Array<PaymentResponse>>;
}

export { IPaymentService };