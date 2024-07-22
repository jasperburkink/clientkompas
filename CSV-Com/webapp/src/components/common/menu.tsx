import React, { ReactNode } from 'react';
import './menu.css';
import { Sidebar } from 'components/sidebar/sidebar';
import { NavButton } from 'components/nav/nav-button';
import { SidebarGray } from 'components/sidebar/sidebar-gray';
import NavItem, { DefaultNavItems } from 'types/common/NavItem';

interface MenuComponentProps {
    children: ReactNode;
    navItems?: NavItem[];
}

const MenuComponent: React.FC<MenuComponentProps> = (props: MenuComponentProps) => {
    let navItems = props.navItems ?? DefaultNavItems;

    return (
        <div className='header-menu fixed'>
        <Sidebar>
            {navItems.map((item) => (
            <NavButton to={item.to} key={item.text} text={item.text} icon={item.icon} />
            ))}
        </Sidebar>
        <SidebarGray>
            {props.children}
        </SidebarGray>
        </div>
    );
};

export default MenuComponent;