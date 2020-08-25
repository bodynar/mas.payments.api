import { Component } from '@angular/core';

import { IRouterService } from 'services/IRouterService';

@Component({
    templateUrl: 'measurement.template.pug'
})
export class MeasurementsComponent {
    constructor(
        private routerService: IRouterService
    ) {
    }

    public onModuleNameClick(): void {
        this.routerService.navigate(['app', 'measurements']);
    }
}