import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';

import { IPaymentApiBackendService } from 'services/IPaymentApi.backend';
import { PaymentApiBackendService } from './services/implementations/paymentApi.backend.service';

import { IPaymentService } from 'services/IPaymentService';
import { PaymentService } from './services/implementations/payment.service';

@NgModule({
    imports: [
        HttpClientModule
    ],
    exports: [],
    declarations: [],
    providers: [
        { provide: IPaymentApiBackendService, useClass: PaymentApiBackendService },
        { provide: IPaymentService, useClass: PaymentService }
    ],
})
export class CoreModule { }