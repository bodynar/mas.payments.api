import { Component } from '@angular/core';

import { IRouterService } from 'services/IRouterService';

import BaseComponent from './BaseComponent';

@Component({
    template: ''
})
export default abstract class BaseRoutingComponent extends BaseComponent {
    constructor(
        protected routerService: IRouterService
    ) {
        super();
    }

    public onBackClick(): void {
        this.routerService.navigateBack();
    }
}