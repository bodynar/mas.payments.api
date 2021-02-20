import MonthYear from 'models/monthYearDate';

export default interface UpdateMeasurementRequest {
    date: MonthYear;
    measurement: number;
    meterMeasurementTypeId: number;
    comment?: string;
}