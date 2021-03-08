import MonthYear from 'models/monthYearDate';

export default interface BaseStatsFilter {
    from?: MonthYear;
    to?: MonthYear;
}