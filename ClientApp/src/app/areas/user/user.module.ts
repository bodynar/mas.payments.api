import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { UserRoutingModule } from './user.routing';

import { AppCommonModule } from 'common/common.module';

import { UserComponent } from './component/user.component';
import { MailMessageLogsComponent } from './components/mailMessageLogs/mailMessageLogs.component';
import { UserNotificationsComponent } from './components/notifications/userNotifications.component';
import { UserSettingComponent } from './components/settings/userSetting.component';
import { TestMailMessageComponent } from './components/testMailMessage/testMailMessage.component';
import { UserCardComponent } from './components/userCard/userCard.component';

@NgModule({
    imports: [
        RouterModule,
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        AppCommonModule,
        UserRoutingModule
    ],
    exports: [],
    declarations: [
        UserComponent,
        UserCardComponent,
        UserSettingComponent,
        UserNotificationsComponent,
        TestMailMessageComponent,
        MailMessageLogsComponent
    ],
    providers: [],
})
class UserModule { }

export { UserModule };