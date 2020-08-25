import { OnDestroy, Component } from '@angular/core';

import { Subject } from 'rxjs';

import { IRouterService } from 'services/IRouterService';

@Component({
    template: ''
})
export default abstract class BaseComponent implements OnDestroy {
    protected whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        protected routerService: IRouterService
    ) {
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public onBackClick(): void {
        this.routerService.navigateBack();
    }
}