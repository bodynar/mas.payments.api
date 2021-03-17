import { AddMeasurementGroupRequestModel, AddMeasurementRequest } from 'models/request/measurement';

export interface MeasurementModel extends AddMeasurementGroupRequestModel {
    isValid?: boolean;
}

export interface AddMeasurementsModel extends AddMeasurementRequest {
    measurements: Array<MeasurementModel>;
}