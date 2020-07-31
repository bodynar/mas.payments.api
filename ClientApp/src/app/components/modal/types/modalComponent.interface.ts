import { IModalService } from '../IModalService';

export interface IModalBodyOptions {
    content: string;
    isHtml: boolean;
}

export default interface IModalComponent {
    title: string;
    modalService: IModalService;
    body?: IModalBodyOptions;
}