export type ModalSize = 'small' | 'large' | 'extra-large';

export default interface IModalComponentOptions {
    title: string;
    size: ModalSize;
    body?: {
        content: string;
        isHtml: boolean;
    };
    additionalParameters?: {
        [propertyName: string]: any;
    };
}