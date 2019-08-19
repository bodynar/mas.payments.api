import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { PaymentsRoutingModule } from './payments.routing';

import { PaymentsComponent } from './component/payments.component';
import { AddPaymentComponent } from './components/addPayment/addPayment.component';
import { AddPaymentTypeComponent } from './components/addPaymentType/addPaymentType.component';
import { PaymentComponent } from './components/payment/payment.component';
import { PaymentListComponent } from './components/paymentList/paymentList.component';
import { PaymentTypesComponent } from './components/paymentTypes/paymentTypes.component';
import { UpdatePaymentComponent } from './components/updatePayment/updatePayment.component';
import { UpdatePaymentTypeComponent } from './components/updatePaymentType/updatePaymentType.component';

@NgModule({
    imports: [
        RouterModule,
        FormsModule,
        CommonModule,
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