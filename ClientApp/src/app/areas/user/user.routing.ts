import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { TestMailMessageComponent } from './components/testMailMessage/testMailMessage.component';
import { UserCardComponent } from './components/userCard/userCard.component';

const routes: Routes = [
    {
        path: '',
        component: UserCardComponent
    },
    {
        path: 'mailTest',
        component: TestMailMessageComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
    declarations: [],
})
export class UserRoutingModule { }