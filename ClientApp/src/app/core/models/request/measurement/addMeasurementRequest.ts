export interface AddMeasurementRequest {
    date: Date;
    measurements: Array<AddMeasurementGroupRequestModel>;
}

export interface AddMeasurementGroupRequestModel {
    id: string;
    measurementTypeId: number;
    measurement: number;
    comment?: string;
}