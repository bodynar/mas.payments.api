interface AddPaymentRequest {
    amount?: number;
    date?: Date;
    description?: string;
    paymentTypeId?: number;
}

export { AddPaymentRequest };