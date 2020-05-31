export default interface PaymentResponse {
    id: number;
    amount: number;
    month: string;
    year: number;
    description?: string;
    paymentTypeName: string;
    paymentTypeId: number;
}