import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';

import { IPaymentApiBackendService } from 'services/IPaymentApi.backend';
import { PaymentApiBackendService } from './services/implementations/paymentApi.backend.service';

import { IPaymentService } from 'services/IPaymentService';
import { PaymentService } from './services/implementations/payment.service';

import { IMeasurementApiBackendService } from 'services/IMeasurementApi.backend';
import { MeasurementApiBackendService } from './services/implementations/measurementApi.backend.service';

import { IMeasurementService } from 'services/IMeasurementService';
import { MeasurementService } from './services/implementations/measurement.service';

@NgModule({
    imports: [
        HttpClientModule
    ],
    exports: [],
    declarations: [],
    providers: [
        { provide: IPaymentApiBackendService, useClass: PaymentApiBackendService },
        { provide: IPaymentService, useClass: PaymentService },

        { provide: IMeasurementApiBackendService, useClass: MeasurementApiBackendService },
        { provide: IMeasurementService, useClass: MeasurementService },
    ],
})
class CoreModule { }

export { CoreModule };