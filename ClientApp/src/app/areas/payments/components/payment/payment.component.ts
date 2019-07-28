import { Component, Input } from '@angular/core';

import { PaymentResponse } from 'models/response/paymentResponse';

@Component({
    selector: 'app-payment-item',
    templateUrl: 'payment.template.pug',
    styleUrls: ['payment.style.styl']
})
class PaymentComponent {
    @Input()
    public payment: PaymentResponse;

    constructor(
    ) { }
}

export { PaymentComponent };