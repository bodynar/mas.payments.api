import { Injectable, Type } from '@angular/core';

import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

import { from, Observable } from 'rxjs';

import { isNullOrUndefined } from 'util';

import { IModalService } from './IModalService';

import IModalComponentOptions from './types/IModalComponentOptions';
import IModalComponent from './types/modalComponent.interface';

@Injectable({ providedIn: 'root' })
export class ModalService implements IModalService {

    public modalRef: NgbModalRef;

    public modalComponent: IModalComponent;

    constructor(
        private modalService: NgbModal
    ) { }

    public show(modalComponent: Type<IModalComponent>, modalOptions: IModalComponentOptions): Observable<any> {
        this.modalRef = this.modalService.open(modalComponent, {
            size: modalOptions.size === 'large' ? 'lg' : 'sm',
        });

        this.modalComponent = this.modalRef.componentInstance;

        if (isNullOrUndefined(this.modalComponent)) {
            throw Error('Modal component is\'nt initialized.');
        }

        this.modalComponent.title = modalOptions.title;
        this.modalComponent.modalBody = modalOptions.modalBody;

        if (!isNullOrUndefined(modalOptions.additionalParameters)) {
            for (const key in modalOptions.additionalParameters) {
                if (Object.prototype.hasOwnProperty.call(modalOptions.additionalParameters, key)
                    && Object.prototype.hasOwnProperty.call(this.modalComponent, key)) {
                    const value = modalOptions.additionalParameters[key];
                    this.modalComponent[key] = value;
                }
            }
        }

        return from(this.modalRef.result);
    }
}