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

export {
    PaymentStatsResponse,
    PaymentStatsMeasurementsResponse
};