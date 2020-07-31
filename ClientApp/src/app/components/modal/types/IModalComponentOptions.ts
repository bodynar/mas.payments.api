export default interface IModalComponentOptions {
    title: string;
    size: 'small' | 'medium' | 'large';
    modalBody?: string;
    additionalParameters?: {
        [propertyName: string]: any;
    };
}