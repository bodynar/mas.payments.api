export default interface IModalComponentOptions {
    title: string;
    size: 'small' | 'large';
    modalBody?: string;
    additionalParameters?: {
        [propertyName: string]: any;
    };
}