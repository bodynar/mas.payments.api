import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';

import { IPaymentApiBackendService } from 'services/IPaymentApi.backend';
import { PaymentApiBackendService } from './services/implementations/paymentApi.backend.service';

@NgModule({
    imports: [
        HttpClientModule
    ],
    exports: [],
    declarations: [],
    providers: [
        { provide: IPaymentApiBackendService, useClass: PaymentApiBackendService }
    ],
})
export class CoreModule { }