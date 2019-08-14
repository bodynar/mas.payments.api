import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { PaymentsComponent } from './component/payments.component';
import { AddPaymentComponent } from './components/addPayment/addPayment.component';
import { AddPaymentTypeComponent } from './components/addPaymentType/addPaymentType.component';
import { PaymentComponent } from './components/payment/payment.component';
import { PaymentListComponent } from './components/paymentList/paymentList.component';
import { PaymentTypesComponent } from './components/paymentTypes/paymentTypes.component';
import { UpdatePaymentComponent } from './components/updatePayment/updatePayment.component';

@NgModule({
    imports: [
        RouterModule,
        FormsModule,
        CommonModule,
    ],
    exports: [],
    declarations: [
        PaymentsComponent, AddPaymentComponent, AddPaymentTypeComponent,
        PaymentListComponent, PaymentComponent, PaymentTypesComponent,
        UpdatePaymentComponent
    ],
    providers: [],
})
class PaymentsModule { }

export { PaymentsModule };