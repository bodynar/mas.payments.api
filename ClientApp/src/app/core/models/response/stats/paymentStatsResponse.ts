export interface GetPaymentsStatisticsDataItem {
    month: number;
    year: number;
    amount: number;
}

export interface PaymentTypeStatisticsItem {
    paymentTypeId: number;
    paymentTypeName: string;
    statisticsData: Array<GetPaymentsStatisticsDataItem>;
}

export interface GetPaymentsStatisticsResponse {
    year: number;
    typeStatistics: Array<PaymentTypeStatisticsItem>;
}