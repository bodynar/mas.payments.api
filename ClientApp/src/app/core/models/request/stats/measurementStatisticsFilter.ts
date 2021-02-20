import MonthYear from 'models/monthYearDate';

export default interface MeasurementStatisticsFilter {
    from?: MonthYear;
    to?: MonthYear;
    measurementTypeId: number;
}