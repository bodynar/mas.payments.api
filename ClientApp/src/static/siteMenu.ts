import { MenuItem } from 'models/menuItem';

const mainSiteMenu: Array<MenuItem> = [
    {
        name: 'Payments',
        link: 'payments',
        isEnabled: true
    },
    {
        name: 'Measurements',
        link: 'measurements',
        isEnabled: true
    },
    {
        name: 'Statistics',
        link: 'stats',
        description: 'Payments statistics and predictions for next payments',
        isEnabled: false
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