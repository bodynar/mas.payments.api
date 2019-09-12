import { Component } from '@angular/core';

import { IRouterService } from 'services/IRouterService';

@Component({
    templateUrl: 'user.template.pug'
})
class UserComponent {
    constructor(
        private routerService: IRouterService
    ) {
    }

    public onModuleNameClick(): void {
        this.routerService.navigate(['app', 'user']);
    }
}

export { UserComponent };