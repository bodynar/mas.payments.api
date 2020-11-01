import { Component } from '@angular/core';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import BaseComponent from './BaseComponent';
import BaseRoutingComponent from './BaseRoutingComponent';

import { IRouterService } from 'services/IRouterService';

import { IModalService } from 'src/app/components/modal/IModalService';
import { ModalSize } from 'src/app/components/modal/types/IModalComponentOptions';

import { ConfirmInModalComponent } from 'src/app/components/modal/components/confirm/confirm.component';
import { TextInModalComponent } from 'src/app/components/modal/components/text/text.component';

const confirmInModal = (modalService: IModalService, confirmTitle: string, confirmText: string): Observable<boolean> => {
    return modalService.show(ConfirmInModalComponent, {
        size: 'small',
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

const displayInModal = (modalService: IModalService, size: ModalSize, title: string, body: string, isHtml: boolean): void => {
    modalService.show(TextInModalComponent, {
        size, title,
        body: {
            content: body,
            isHtml,
        },
    });
};

export type ModalOptions = {
    size: ModalSize;
    title: string;
    body: string;
    isHtml: boolean;
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

    protected showInModal(modalOptions: ModalOptions): void {
        displayInModal(this.modalService, modalOptions.size, modalOptions.title, modalOptions.body, modalOptions.isHtml);
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