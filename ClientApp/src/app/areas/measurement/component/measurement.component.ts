import { Component } from '@angular/core';

import { IRouterService } from 'services/IRouterService';

@Component({
    templateUrl: 'measurement.template.pug',
    styleUrls: ['measurement.style.styl']
})
class MeasurementsComponent {
    constructor(
        private routerService: IRouterService
    ) {
    }

    public onModuleNameClick(): void {
        this.routerService.navigate(['app', 'measurements']);
    }
}

export { MeasurementsComponent };