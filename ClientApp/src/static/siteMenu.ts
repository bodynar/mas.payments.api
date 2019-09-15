import { MenuItem } from 'models/menuItem';

const mainSiteMenu: Array<MenuItem> = [
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

const userSideMenu: Array<MenuItem> = [
    {
        name: 'User card',
        link: '',
        isEnabled: true
    },
    {
        name: 'User settings',
        link: 'settings',
        isEnabled: false
    },
    {
        name: 'Test mail message',
        link: 'mailTest',
        isEnabled: true
    }
];

export {
    mainSiteMenu as siteMenu,
    userSideMenu
};