import MonthYear from 'models/monthYearDate';

export interface AddMeasurementRequest {
    date: MonthYear;
    measurements: Array<AddMeasurementGroupRequestModel>;
}

export interface AddMeasurementGroupRequestModel {
    id: string;
    measurementTypeId: number;
    measurement: number;
    comment?: string;
}