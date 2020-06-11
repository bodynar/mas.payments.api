import { MenuItem } from 'models/menuItem';

const mainSiteMenu: Array<MenuItem> = [
    {
        name: 'Payments',
        link: 'payments',
        isEnabled: true,
    },
    {
        name: 'Measurements',
        link: 'measurements',
        isEnabled: true,
    },
    {
        name: 'Statistics',
        link: 'stats',
        description: 'Visual statistics in charts',
        isEnabled: true,
    }
];

const userSideMenu: Array<MenuItem> = [
    {
        name: 'User card',
        link: '',
        isEnabled: true,
    },
    {
        name: 'User settings',
        link: 'settings',
        isEnabled: true,
    },
    {
        name: 'Test mail message',
        link: 'mailTest',
        isEnabled: true,
    }
];

export {
    mainSiteMenu as siteMenu,
    userSideMenu
};