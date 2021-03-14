export interface PaymentFilter {
    month?: number;
    year?: number;
    paymentTypeId?: number;

    amount?: {
        min?: number;
        max?: number;
        exact?: number;
    };
}

export default class PaymentsFilter implements PaymentFilter {
    month?: number;
    year?: number;
    paymentTypeId?: number;

    amount?: {
        min?: number;
        max?: number;
        exact?: number;
    };
}