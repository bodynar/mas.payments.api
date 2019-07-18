import { NgModule } from '@angular/core';

import { PaymentsComponent } from './component/payments.component';
import { AddPaymentComponent } from './components/addPayment/addPayment.component';
import { AddPaymentTypeComponent } from './components/addPaymentType/addPaymentType.component';

@NgModule({
    imports: [],
    exports: [],
    declarations: [
        PaymentsComponent, AddPaymentComponent, AddPaymentTypeComponent
    ],
    providers: [],
})
class PaymentsModule { }

export { PaymentsModule };