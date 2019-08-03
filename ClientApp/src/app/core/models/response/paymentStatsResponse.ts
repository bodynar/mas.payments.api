interface PaymentStatsMeasurementsResponse {
    name: string;
    measurement: number;
}

interface PaymentStatsResponse {
    amount: number;
    paymentType: string;
    date?: Date;
    measurements: Array<PaymentStatsMeasurementsResponse>;
}

export {
    PaymentStatsResponse,
    PaymentStatsMeasurementsResponse
};