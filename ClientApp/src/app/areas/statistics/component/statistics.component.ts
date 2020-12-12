import { Component } from '@angular/core';

import { IRouterService } from 'services/IRouterService';

@Component({
    templateUrl: 'statistics.template.pug'
})
export class StatisticsComponent {
    constructor(
        private routerService: IRouterService
    ) {
    }

    public onModuleNameClick(): void {
        this.routerService.navigate(['app', 'stats']);
    }
}