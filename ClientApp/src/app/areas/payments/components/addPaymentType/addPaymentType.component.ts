import { Component, OnDestroy } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ReplaySubject, Subject } from 'rxjs';
import { filter, switchMap, takeUntil } from 'rxjs/operators';

import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';
import { IRouterService } from 'services/IRouterService';

import { AddPaymentTypeRequest } from 'models/request/addPaymentTypeRequest';

@Component({
    templateUrl: 'addPaymentType.template.pug'
})
class AddPaymentTypeComponent implements OnDestroy {

    public addPaymentTypeRequest: AddPaymentTypeRequest =
        {
            name: ''
        };

    public whenSubmittedForm$: Subject<NgForm> =
        new ReplaySubject(1);

    private whenComponentDestroy$: Subject<null> =
        new Subject();

    constructor(
        private paymentService: IPaymentService,
        private notificationService: INotificationService,
        private routerService: IRouterService,
    ) {
        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid),
                switchMap(_ => this.paymentService.addPaymentType(this.addPaymentTypeRequest)),
                filter(withoutError => {
                    if (!withoutError) {
                        this.notificationService.error('Error due saving data. Please, try again later');
                    } else {
                        this.notificationService.success('Payment type was successfully added.');
                    }

                    return withoutError;
                })
            )
            .subscribe(_ => this.routerService.navigateUp());
    }

    public ngOnDestroy(): void {
        this.whenComponentDestroy$.next(null);
        this.whenComponentDestroy$.complete();
    }

    public onFormSubmit(form: NgForm): void {
        this.whenSubmittedForm$.next(form);
    }
}

export { AddPaymentTypeComponent };