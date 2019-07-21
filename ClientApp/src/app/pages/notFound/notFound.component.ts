import { Component } from '@angular/core';

import { IRouterService } from 'services/IRouterService';

@Component({
    templateUrl: 'notFound.template.pug',
    styleUrls: ['notFound.style.styl']
})
class NotFoundComponent {
    public route: string;

    constructor(
        private routerService: IRouterService
    ) {
        this.route = `/${this.routerService.getCurrentRoute(true)}`;
    }
}

export { NotFoundComponent };