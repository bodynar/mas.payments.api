import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { UserRoutingModule } from './user.routing';

import { UserComponent } from './component/user.component';
import { TestMailMessageComponent } from './components/testMailMessage/testMailMessage.component';
import { UserCardComponent } from './components/userCard/userCard.component';

@NgModule({
    imports: [
        RouterModule,
        FormsModule,
        CommonModule,
        UserRoutingModule
    ],
    exports: [],
    declarations: [
        UserComponent, UserCardComponent,
        TestMailMessageComponent
    ],
    providers: [],
})
class UserModule { }

export { UserModule };