import MonthYear from 'models/monthYearDate';

export default interface PaymentStatisticsFilter {
    from?: MonthYear;
    to?: MonthYear;
    paymentTypeId: number;
}