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
        name: 'Application info',
        link: '',
        isEnabled: true,
    },
    {
        name: 'Notifications',
        link: 'notifications',
        isEnabled: true,
    },
    {
        name: 'Settings',
        link: 'settings',
        isEnabled: true,
    },
    {
        name: 'Email test',
        link: 'mailTest',
        isEnabled: true,
    },
    //{
    //    name: 'Mail message logs',
    //    link: 'mailLogs',
    //    isEnabled: true
    //}
];

export {
    mainSiteMenu as siteMenu,
    userSideMenu
};