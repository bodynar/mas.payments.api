import { IModalService } from '../IModalService';

export default interface IModalComponent {
    title: string;
    modalService: IModalService;
    modalBody?: string;
}