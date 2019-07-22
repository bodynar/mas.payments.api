import { Component } from '@angular/core';

import { INotificationService } from 'services/INotificationService';

@Component({
    templateUrl: 'payments.template.pug',
    styleUrls: ['payments.style.styl']
})
class PaymentsComponent {
    constructor(
        private notificationService: INotificationService
    ) {
        setTimeout(() =>
            this.notificationService.error('You don\'t have an permission to execute operation.'),
            1 * 1000
        );

        setTimeout(() =>
            this.notificationService.warning('Unauthorized persons will be executed.'),
            1 * 3000
        );

        setTimeout(() =>
            this.notificationService.success('Your application have been accepted.'),
            1 * 5000
        );

        setTimeout(() =>
            this.notificationService.info('Remote databases has been restored.'),
            1 * 7000
        );
    }
}

export { PaymentsComponent };