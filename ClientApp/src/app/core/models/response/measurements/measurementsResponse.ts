interface MeasurementsResponseMeasurement {
    id: number;
    measurement: number;
    comment?: string;
    meterMeasurementTypeId: number;
    measurementTypeName: string;
    isSent: boolean;
}

export default interface MeasurementsResponse {
    month: string;
    year: number;

    measurements: Array<MeasurementsResponseMeasurement>;
}