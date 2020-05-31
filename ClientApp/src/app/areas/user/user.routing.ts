import { Routes } from '@angular/router';

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
    }
];