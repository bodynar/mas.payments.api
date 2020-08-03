export interface GetMeasurementStatisticsDataItem {
    month: number;
    year: number;
    diff?: number;
}

export interface GetMeasurementStatisticsResponse {
    year: number;
    measurementTypeId: number;
    statisticsData: Array<GetMeasurementStatisticsDataItem>;
}