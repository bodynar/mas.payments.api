import BaseStatsFilter from './baseStatsFilter';

export default interface MeasurementStatisticsFilter extends BaseStatsFilter {
    measurementTypeId: number;
}