export default interface MeasurementTypeResponse {
    id?: number;
    name: string;
    systemName: string;
    description?: string;
    paymentTypeId?: number;
    paymentTypeName?: string;
    paymentTypeColor?: string;
    color?: string;
    hasColor?: boolean;
    hasRelatedMeasurements?: boolean;
}