export default interface IModalComponentOptions {
    title: string;
    size: 'small' | 'medium' | 'large';
    body?: {
        content: string;
        isHtml: boolean;
    };
    additionalParameters?: {
        [propertyName: string]: any;
    };
}