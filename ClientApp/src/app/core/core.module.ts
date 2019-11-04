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

import { IStatisticsApiBackendService } from 'services/backend/IStatisticsApi.backend';
import { IStatisticsService } from 'services/IStatisticsService';
import { StatisticsApiBackendService } from './services/implementations/backend/statisticsApi.backend.service';
import { StatisticsService } from './services/implementations/statisticsService.service';

import { IUserApiBackendService } from 'services/backend/IUserApi.backend';
import { IUserService } from 'services/IUserService';
import { UserApiBackendService } from './services/implementations/backend/userApi.backend.service';
import { UserService } from './services/implementations/user.service';

import { IAuthService } from 'services/IAuthService';
import { AuthService } from './services/implementations/auth.service';

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

        { provide: IStatisticsApiBackendService, useClass: StatisticsApiBackendService },
        { provide: IStatisticsService, useClass: StatisticsService },

        { provide: IUserApiBackendService, useClass: UserApiBackendService },
        { provide: IUserService, useClass: UserService },

        { provide: IAuthService, useClass: AuthService },
    ],
})
class CoreModule { }

export { CoreModule };