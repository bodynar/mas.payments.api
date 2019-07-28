import { Component, Input } from '@angular/core';

import { PaymentResponse } from 'models/response/PaymentResponse';

@Component({
    selector: 'app-payment-item',
    templateUrl: 'payment.template.pug',
    styleUrls: ['payment.style.styl']
})
class PaymentComponent {
    @Input()
    public Payment: PaymentResponse;

    constructor(
    ) { }
}

export { PaymentComponent };