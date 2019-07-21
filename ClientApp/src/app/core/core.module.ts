import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';

import { IPaymentApiBackendService } from 'services/backend/IPaymentApi.backend';
import { PaymentApiBackendService } from './services/implementations/backend/paymentApi.backend.service';

import { IPaymentService } from 'services/IPaymentService';
import { PaymentService } from './services/implementations/payment.service';

import { IMeasurementApiBackendService } from 'services/backend/IMeasurementApi.backend';
import { MeasurementApiBackendService } from './services/implementations/backend/measurementApi.backend.service';

import { IMeasurementService } from 'services/IMeasurementService';
import { MeasurementService } from './services/implementations/measurement.service';

import { IRouterService } from 'services/IRouterService';
import { RouterService } from './services/implementations/router.service';

import { INotificationService } from 'services/INotificationService';
import { NotificationService } from './services/implementations/notification.service';

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

        { provide: IRouterService, useClass: RouterService },

        { provide: INotificationService, useClass: NotificationService },
    ],
})
class CoreModule { }

export { CoreModule };