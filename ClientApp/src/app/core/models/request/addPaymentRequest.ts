interface AddPaymentRequest {
    amount?: number;
    year?: string;
    month?: string;
    description?: string;
    paymentTypeId?: number;
}

export { AddPaymentRequest };