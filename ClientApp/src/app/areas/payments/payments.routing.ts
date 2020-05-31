import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AddPaymentComponent } from './components/addPayment/addPayment.component';
import { AddPaymentTypeComponent } from './components/addPaymentType/addPaymentType.component';
import { PaymentListComponent } from './components/paymentList/paymentList.component';
import { PaymentTypesComponent } from './components/paymentTypes/paymentTypes.component';
import { UpdatePaymentComponent } from './components/updatePayment/updatePayment.component';
import { UpdatePaymentTypeComponent } from './components/updatePaymentType/updatePaymentType.component';

const routes: Routes = [
    {
        path: '',
        component: PaymentListComponent
    },
    {
        path: 'add',
        component: AddPaymentComponent
    },
    {
        path: 'update',
        component: UpdatePaymentComponent
    },
    {
        path: 'types',
        component: PaymentTypesComponent
    },
    {
        path: 'addType',
        component: AddPaymentTypeComponent
    },
    {
        path: 'updateType',
        component: UpdatePaymentTypeComponent
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
    declarations: [],
})
export class PaymentsRoutingModule { }