import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { PaymentsRoutingModule } from './payments.routing';

import { AppCommonModule } from 'common/common.module';

import { PaymentsComponent } from './component/payments.component';

import { AddPaymentComponent } from './components/addPayment/addPayment.component';
import { PaymentComponent } from './components/payment/payment.component';
import { PaymentListComponent } from './components/paymentList/paymentList.component';
import { UpdatePaymentComponent } from './components/updatePayment/updatePayment.component';

import { AddPaymentTypeComponent } from './components/addPaymentType/addPaymentType.component';
import { PaymentTypesComponent } from './components/paymentTypes/paymentTypes.component';
import { UpdatePaymentTypeComponent } from './components/updatePaymentType/updatePaymentType.component';

@NgModule({
    imports: [
        RouterModule,
        FormsModule,
        CommonModule,
        AppCommonModule,
        PaymentsRoutingModule,
    ],
    exports: [],
    declarations: [
        PaymentsComponent, AddPaymentComponent, PaymentListComponent, PaymentComponent, UpdatePaymentComponent,
        AddPaymentTypeComponent, PaymentTypesComponent, UpdatePaymentTypeComponent
    ],
    providers: [],
})
class PaymentsModule { }

export { PaymentsModule };