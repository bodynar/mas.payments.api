import { MenuItem } from 'models/menuItem';

const siteMenu: Array<MenuItem> = [
    {
        name: 'Payments',
        link: 'payments',
        isEnabled: true
    },
    {
        name: 'Measurements',
        link: 'measures',
        isEnabled: true
    },
    {
        name: 'Statistics',
        link: 'stats',
        description: 'Payments statistics and predictions for next payments',
        isEnabled: false
    }
];

export { siteMenu };