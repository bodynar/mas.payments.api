import { MenuItem } from 'models/menuItem';

const siteMenu: Array<MenuItem> = [
    {
        name: 'Payments',
        link: 'app/payments',
        isEnabled: true
    },
    {
        name: 'Measurements',
        link: 'app/measurements',
        isEnabled: true
    },
    {
        name: 'Statistics',
        link: 'app/stats',
        description: 'Payments statistics and predictions for next payments',
        isEnabled: true
    }
];

export { siteMenu };