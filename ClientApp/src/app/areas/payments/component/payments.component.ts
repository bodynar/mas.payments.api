import { Component } from '@angular/core';

import { IRouterService } from 'services/IRouterService';

@Component({
    templateUrl: 'payments.template.pug',
    styleUrls: ['payments.style.styl']
})
class PaymentsComponent {
    constructor(
        private routerService: IRouterService
    ) {
    }

    public onModuleNameClick(): void {
        this.routerService.navigate(['app', 'payments']);
    }
}

export { PaymentsComponent };