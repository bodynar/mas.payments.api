interface PaymentsFilter {
    month?: number;
    paymentTypeId?: number;

    amount?: {
        min?: number;
        max?: number;
        exact?: number;
    };
}


export { PaymentsFilter };