import { Observable } from 'rxjs';

import { PaymentsFilter } from 'models/paymentsFilter';
import { AddPaymentRequest } from 'models/request/addPaymentRequest';
import { AddPaymentTypeRequest } from 'models/request/addPaymentTypeRequest';
import { PaymentResponse } from 'models/response/paymentResponse';
import { PaymentTypeResponse } from 'models/response/paymentTypeResponse';

abstract class IPaymentApiBackendService {
    abstract addPaymentType(paymentTypeData: AddPaymentTypeRequest): Observable<any>;

    abstract addPayment(paymentData: AddPaymentRequest): Observable<any>;

    abstract getPaymentTypes(): Observable<Array<PaymentTypeResponse>>;

    abstract getPayments(filter?: PaymentsFilter): Observable<Array<PaymentResponse>>;
}

export { IPaymentApiBackendService };