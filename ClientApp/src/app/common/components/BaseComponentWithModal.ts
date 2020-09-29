import { Component } from '@angular/core';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import BaseComponent from './BaseComponent';
import BaseRoutingComponent from './BaseRoutingComponent';

import { IRouterService } from 'services/IRouterService';

import { IModalService } from 'src/app/components/modal/IModalService';
import { ConfirmInModalComponent } from 'src/app/components/modal/components/confirm/confirm.component';

const confirmInModal = (modalService: IModalService, confirmTitle: string, confirmText: string): Observable<boolean> => {
    return modalService.show(ConfirmInModalComponent, {
        size: 'medium',
        title: confirmTitle,
        body: {
            content: confirmText,
            isHtml: false,
        },
        additionalParameters: {
            confirmBtnText: 'Yes',
            cancelBtnText: 'No',
        }
    }).pipe(map(response => response as boolean));
};

@Component({ template: '' })
export abstract class BaseComponentWithModalComponent extends BaseComponent {
    constructor(
        protected modalService: IModalService
    ) {
        super();
    }

    protected confirmInModal(confirmTitle: string, confirmText: string): Observable<boolean> {
        return confirmInModal(this.modalService, confirmTitle, confirmText);
    }

    protected confirmDelete(): Observable<boolean> {
        return confirmInModal(this.modalService, 'Confirm delete', 'Are you sure want to delete?');
    }
}

@Component({ template: '' })
export abstract class BaseRoutingComponentWithModalComponent extends BaseRoutingComponent {
    constructor(
        routerService: IRouterService,
        protected modalService: IModalService
    ) {
        super(routerService);
    }

    protected confirmInModal(confirmTitle: string, confirmText: string): Observable<boolean> {
        return confirmInModal(this.modalService, confirmTitle, confirmText);
    }

    protected confirmDelete(): Observable<boolean> {
        return confirmInModal(this.modalService, 'Confirm delete', 'Are you sure want to delete?');
    }
}