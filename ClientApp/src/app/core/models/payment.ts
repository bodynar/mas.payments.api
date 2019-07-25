interface Payment {
    id: number;
    amount: number;
    description?: string;
    paymentType: string;
    paymentTypeId: number;
    date?: Date;
}

export { Payment };