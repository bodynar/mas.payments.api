export interface MeasurementsResponseMeasurement {
    id: number;
    measurement: number;
    comment?: string;
    meterMeasurementTypeId: number;
    measurementTypeName: string;
    measurementTypeColor?: string;
    isSent: boolean;
    month: number;
    year: number;
}

export default interface MeasurementsResponse {
    month: number;
    year: number;

    measurements: Array<MeasurementsResponseMeasurement>;
}