interface MeasurementResponse {
    id: number;
    measurement: number;
    comment?: string;
    date: Date;
    meterMeasurementTypeId: number;
    measurementTypeName: string;
}

export { MeasurementResponse };