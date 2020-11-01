import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { BehaviorSubject, ReplaySubject, Subject } from 'rxjs';
import { delay, filter, map, switchMap, takeUntil, tap } from 'rxjs/operators';

import BaseRoutingComponent from 'common/components/BaseRoutingComponent';

import { isNullOrUndefined } from 'common/utils/common';

import { INotificationService } from 'services/INotificationService';
import { IPaymentService } from 'services/IPaymentService';
import { IRouterService } from 'services/IRouterService';

import { AddPaymentTypeRequest } from 'models/request/payment/addPaymentTypeRequest';

@Component({
    templateUrl: 'updatePaymentType.template.pug'
})
export class UpdatePaymentTypeComponent extends BaseRoutingComponent {

    public paymentTypeRequest: AddPaymentTypeRequest =
        {};

    public isLoading$: Subject<boolean> =
        new BehaviorSubject(false);

    public whenSubmittedForm$: Subject<NgForm> =
        new ReplaySubject(1);

    private paymentTypeId: number;

    constructor(
        private activatedRoute: ActivatedRoute,
        private paymentService: IPaymentService,
        private notificationService: INotificationService,
        routerService: IRouterService,
    ) {
        super(routerService);
        this.activatedRoute
            .queryParams
            .pipe(
                filter(params => !isNullOrUndefined(params['id']) && params['id'] !== 0),
                map(params => params['id']),
                tap(id => this.paymentTypeId = id),
                switchMap(id => this.paymentService.getPaymentType(id)),
                filter(response => {
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    }
                    return response.success;
                }),
            )
            .subscribe(({ result }) => this.paymentTypeRequest = result);

        this.whenSubmittedForm$
            .pipe(
                takeUntil(this.whenComponentDestroy$),
                filter(({ valid }) => valid),
                switchMap(_ => {
                    this.isLoading$.next(true);
                    return this.paymentService.updatePaymentType(this.paymentTypeId, this.paymentTypeRequest);
                }),
                delay(1.5 * 1000),
                filter(response => {
                    this.isLoading$.next(false);
                    if (!response.success) {
                        this.notificationService.error(response.error);
                    } else {
                        this.notificationService.success('Payment type was successfully updated.');
                    }

                    return response.success;
                })
            )
            .subscribe(_ => this.routerService.navigateArea(['types']));
    }

    public onFormSubmit(form: NgForm): void {
        this.whenSubmittedForm$.next(form);
    }
}