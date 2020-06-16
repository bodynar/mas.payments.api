export default interface PaymentTypeResponse {
    id?: number;
    systemName: string;
    name: string;
    description?: string;
    company?: string;
    hasRelatedPayments?: boolean;
    hasRelatedMeasurementTypes?: boolean;
}