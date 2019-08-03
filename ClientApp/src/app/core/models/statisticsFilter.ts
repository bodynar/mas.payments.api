interface StatisticsFilter {
    includeMeasurements: boolean;
    year?: number;
    from?: Date;
    to?: Date;
}

export { StatisticsFilter };