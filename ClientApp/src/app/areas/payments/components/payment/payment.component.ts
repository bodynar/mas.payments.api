import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { getMonthName } from 'static/months';

import PaymentResponse from 'models/response/payments/paymentResponse';

@Component({
    selector: 'app-payment-item',
    templateUrl: 'payment.template.pug',
    styleUrls: ['payment.style.styl']
})
class PaymentComponent implements OnInit {
    @Input()
    public payment: PaymentResponse;

    @Output()
    public deleteClick: EventEmitter<number> =
        new EventEmitter();

    @Output()
    public editClick: EventEmitter<number> =
        new EventEmitter();

    @Output()
    public typeClick: EventEmitter<number> =
        new EventEmitter();

    public formattedMonth: string;

    constructor(
    ) {
    }

    public ngOnInit(): void {
        this.formattedMonth = getMonthName(+this.payment.month);
    }
}

export { PaymentComponent };