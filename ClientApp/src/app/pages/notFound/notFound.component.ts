import { Component } from '@angular/core';

import { IRouterService } from 'services/IRouterService';

@Component({
    templateUrl: 'notFound.template.pug',
})
class NotFoundComponent {
    constructor(
        private routerService: IRouterService
    ) {
    }

    public onHomeClick(): void {
        this.routerService.navigate(['app']);
    }
}

export { NotFoundComponent };