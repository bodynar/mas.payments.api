import { Component, OnDestroy } from '@angular/core';

import { Subject } from 'rxjs';

@Component({
    templateUrl: 'forgotPassword.template.pug',
    styleUrls: ['forgotPassword.style.styl']
})
class ForgotPasswordComponent implements OnDestroy {

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
    ) {
    }
    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }
}

export { ForgotPasswordComponent };