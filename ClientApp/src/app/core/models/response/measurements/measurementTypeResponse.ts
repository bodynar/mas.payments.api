export default interface MeasurementTypeResponse {
    id?: number;
    name: string;
    systemName: string;
    description?: string;
    paymentTypeId?: number;
    paymentTypeName?: string;
    color?: string;
    hasColor?: boolean;
    hasRelatedMeasurements?: boolean;
}