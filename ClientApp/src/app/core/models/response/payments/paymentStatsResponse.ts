interface PaymentStatsMeasurementResponse {
    name: string;
    systemName: string;
    measurement: number;
}

interface PaymentStatsMeasurementsResponse {
    date: Date;
    measurements: Array<PaymentStatsMeasurementResponse>;
}

interface PaymentStatsPayment {
    id: number;
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
    PaymentStatsMeasurementResponse,
    GetPaymentStatsResponse
};