import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { UserComponent } from './component/user.component';
import { UserSettingComponent } from './components/settings/userSetting.component';
import { TestMailMessageComponent } from './components/testMailMessage/testMailMessage.component';
import { UserCardComponent } from './components/userCard/userCard.component';

@NgModule({
    imports: [
        RouterModule,
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
    ],
    exports: [],
    declarations: [
        UserComponent,
        UserCardComponent,
        UserSettingComponent,
        TestMailMessageComponent
    ],
    providers: [],
})
class UserModule { }

export { UserModule };