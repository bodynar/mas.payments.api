import { Observable } from 'rxjs';

import { PaymentsFilter } from 'models/paymentsFilter';
import { AddPaymentRequest } from 'models/request/addPaymentRequest';
import { AddPaymentTypeRequest } from 'models/request/addPaymentTypeRequest';
import { PaymentResponse } from 'models/response/paymentResponse';
import { PaymentTypeResponse } from 'models/response/paymentTypeResponse';

abstract class IPaymentService {
    abstract addPaymentType(paymentTypeData: AddPaymentTypeRequest): void;

    abstract addPayment(paymentData: AddPaymentRequest): void;

    abstract getPaymentTypes(): Observable<Array<PaymentTypeResponse>>;

    abstract getPayments(filter?: PaymentsFilter): Observable<Array<PaymentResponse>>;
}

export { IPaymentService };