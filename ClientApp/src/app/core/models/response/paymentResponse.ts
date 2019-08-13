interface PaymentResponse {
    id: number;
    amount: number;
    date?: Date;
    description?: string;
    paymentType: string;
    paymentTypeId: number;
}

export { PaymentResponse };