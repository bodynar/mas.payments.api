import { Component, EventEmitter, Input, Output } from '@angular/core';

import { PaymentResponse } from 'models/response/paymentResponse';

import { getMonthName } from 'src/static/months';

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

    @Output()
    public editClick: EventEmitter<number> =
        new EventEmitter();

    @Output()
    public typeClick: EventEmitter<number> =
        new EventEmitter();

    constructor(
    ) {
    }

    public formatMonth(monthNumber: number): string {
      return getMonthName(monthNumber);
    }

    public getPaymentTypeClass(paymentTypeName: string): string {
        // todo: remove method and update model

        switch (paymentTypeName.toLowerCase()) {
            case 'жкх':
                return 'house';
            case 'электричество':
                return 'electricity';
            case 'интернет':
                return 'internet';
            default:
                return '';
        }
    }
}

export { PaymentComponent };