import { Observable } from 'rxjs';

import { AddPaymentRequest } from 'models/addPaymentRequest';
import { AddPaymentTypeRequest } from 'models/addPaymentTypeRequest';
import { PaymentResponse } from 'models/paymentResponse';
import { PaymentTypeResponse } from 'models/paymentTypeResponse';

abstract class IPaymentApiBackendService {
    abstract addPaymentType(paymentTypeData: AddPaymentTypeRequest): Observable<any>;

    abstract addPayment(paymentData: AddPaymentRequest): Observable<any>;

    abstract getPaymentTypes(): Observable<Array<PaymentTypeResponse>>;

    abstract getPayments(): Observable<Array<PaymentResponse>>;
}

export { IPaymentApiBackendService };