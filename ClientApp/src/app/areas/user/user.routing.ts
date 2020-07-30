import { Routes } from '@angular/router';

import { MailMessageLogsComponent } from './components/mailMessageLogs/mailMessageLogs.component';
import { UserNotificationsComponent } from './components/notifications/userNotifications.component';
import { UserSettingComponent } from './components/settings/userSetting.component';
import { TestMailMessageComponent } from './components/testMailMessage/testMailMessage.component';
import { UserCardComponent } from './components/userCard/userCard.component';

export const routes: Routes = [
    {
        path: '',
        component: UserCardComponent
    },
    {
        path: 'settings',
        component: UserSettingComponent,
    },
    {
        path: 'mailTest',
        component: TestMailMessageComponent
    },
    {
        path: 'notifications',
        component: UserNotificationsComponent
    },
    {
        path: 'mailLogs',
        component: MailMessageLogsComponent
    }
];