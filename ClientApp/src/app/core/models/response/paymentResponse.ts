interface PaymentResponse {
    id: number;
    amount: number;
    date?: Date;
    description?: string;
    paymentTypeName: string;
    paymentTypeId: number;
}

export { PaymentResponse };