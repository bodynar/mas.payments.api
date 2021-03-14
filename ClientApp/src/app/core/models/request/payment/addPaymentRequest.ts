export interface AddPaymentRequest {
    amount?: number;
    year?: number;
    month?: string;
    description?: string;
    paymentTypeId?: number;
}