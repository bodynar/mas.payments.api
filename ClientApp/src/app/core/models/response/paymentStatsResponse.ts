interface PaymentStatsMeasurementsResponse {
    name: string;
    measurement: number;
}

interface PaymentStatsPayment {
    amount: number;
    date?: Date;
    measurements: Array<PaymentStatsMeasurementsResponse>;
}

interface PaymentStatsResponse {
    paymentTypeName: string;
    paymentTypeId: number;
    payments: Array<PaymentStatsPayment>;
}

interface GetPaymentStatsResponse {
    dates?: Array<Date>;
    items: Array<PaymentStatsResponse>;
}

export {
    PaymentStatsResponse,
    PaymentStatsMeasurementsResponse,
    GetPaymentStatsResponse
};