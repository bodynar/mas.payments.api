export default interface UpdateMeasurementRequest {
    date: Date;
    measurement: number;
    meterMeasurementTypeId: number;
    comment?: string;
}