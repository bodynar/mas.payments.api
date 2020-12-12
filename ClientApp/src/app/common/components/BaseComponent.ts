import { OnDestroy, Component, OnInit } from '@angular/core';

import { Subject } from 'rxjs';

@Component({
    template: ''
})
export default abstract class BaseComponent implements OnDestroy, OnInit {
    protected whenComponentDestroy$: Subject<null> =
        new Subject();

    protected whenComponentInit$: Subject<null> =
        new Subject();

    constructor(
    ) {
    }

    public ngOnInit(): void {
        this.whenComponentInit$.next(null);
        this.whenComponentInit$.complete();
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }
}