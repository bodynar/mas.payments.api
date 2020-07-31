import { Type } from '@angular/core';

import { Observable } from 'rxjs';

import IModalComponentOptions from './types/IModalComponentOptions';
import IModalComponent from './types/modalComponent.interface';

export abstract class IModalService {
    abstract show(modalComponent: Type<IModalComponent>, modalOptions: IModalComponentOptions): Observable<any>;
}