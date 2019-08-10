import { Component, EventEmitter, Input, Output } from '@angular/core';

import { PaymentResponse } from 'models/response/paymentResponse';

import { months } from 'src/static/months';


@Component({
    selector: 'app-payment-item',
    templateUrl: 'payment.template.pug',
    styleUrls: ['payment.style.styl']
})
class PaymentComponent {
    @Input()
    public payment: PaymentResponse;

    @Output()
    public deleteClick: EventEmitter<number> =
        new EventEmitter();

    constructor(
    ) {
    }

    public formatDate(rawDate: string): string {
        const date: Date =
            new Date(rawDate);

        const month: number =
            date.getMonth();

        return `[${date.getFullYear()}] ${months[month].name}`;
    }
}

export { PaymentComponent };