interface MeasurementResponse {
    id: number;
    measurement: number;
    comment?: string;
    month: string;
    year: number;
    meterMeasurementTypeId: number;
    measurementTypeName: string;
}

export { MeasurementResponse };