export interface GetMeasurementStatisticsDataItem {
    month: number;
    year: number;
    diff?: number;
}

export interface MeasurementTypeStatisticsItem {
    measurementTypeId: number;
    measurementTypeName: string;
    statisticsData: Array<GetMeasurementStatisticsDataItem>;
}

export interface GetMeasurementStatisticsResponse {
    year: number;
    typeStatistics: Array<MeasurementTypeStatisticsItem>;
}