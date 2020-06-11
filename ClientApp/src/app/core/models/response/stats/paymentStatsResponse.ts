export interface GetPaymentsStatisticsDataItem {
    month: number;
    year: number;
    amount: number;
}

export interface GetPaymentsStatisticsResponse {
    year: number;
    paymentTypeId: number;
    statisticsData: Array<GetPaymentsStatisticsDataItem>;
}