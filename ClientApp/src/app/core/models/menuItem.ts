interface MenuItem {
    name: string;
    description?: string;
    link: string;
    isActive?: boolean;
    isEnabled: boolean;
}

export { MenuItem };