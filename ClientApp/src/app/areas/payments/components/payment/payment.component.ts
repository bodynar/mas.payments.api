import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { PaymentResponse } from 'models/response/paymentResponse';

import { months } from 'src/static/months';

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

    public date: Date;

    constructor(
    ) {
    }

    public ngOnInit(): void {
        this.date = new Date(this.payment.date);
    }

    public formatDate(rawDate: string): string {
        const month: number =
            new Date(rawDate).getMonth();

        return `${months[month].name}`;
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