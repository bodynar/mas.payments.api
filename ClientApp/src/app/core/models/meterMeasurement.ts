interface MeterMeasurement {
    id: number;
    measurementType: string;
    measurementTypeId: number;
    date: Date;
    measurement: number;
    comment?: string;
}

export { MeterMeasurement };