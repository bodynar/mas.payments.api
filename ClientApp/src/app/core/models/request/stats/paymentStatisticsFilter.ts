import BaseStatsFilter from './baseStatsFilter';

export default interface PaymentStatisticsFilter extends BaseStatsFilter {
    paymentTypeId: number;
}