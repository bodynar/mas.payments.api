import MonthYear from 'models/monthYearDate';

export default interface MeasurementResponse {
    id: number;
    measurement: number;
    comment?: string;
    date: MonthYear;
    meterMeasurementTypeId: number;
    measurementTypeName: string;
    isSent: boolean;
}