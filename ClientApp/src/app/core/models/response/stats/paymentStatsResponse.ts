export interface GetPaymentsStatisticsDataItem {
    month: number;
    year: number;
    amount: number;
}

export interface TypeStatisticsItem {
    paymentTypeId: number;
    paymentTypeName: string;
    statisticsData: Array<GetPaymentsStatisticsDataItem>;
}

export interface GetPaymentsStatisticsResponse {
    year: number;
    paymentTypeId: number;
    typeStatistics: Array<TypeStatisticsItem>;
}