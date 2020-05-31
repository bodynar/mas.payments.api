export default class PaymentsFilter {
    month?: number;
    year?: number;
    paymentTypeId?: number;

    amount?: {
        min?: number;
        max?: number;
        exact?: number;
    };
}