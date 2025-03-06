import React, { ReactNode, useEffect, useState } from 'react';
import './menu.css';
import { Sidebar } from 'components/sidebar/sidebar';
import { NavButton } from 'components/nav/nav-button';
import { SidebarGray } from 'components/sidebar/sidebar-gray';
import NavItem, { LOGOUT_ID } from 'types/common/NavItem';
import ConfirmPopup from './confirm-popup';
import { useNavigate } from 'react-router-dom';
import ErrorPopup from './error-popup';
import LogoutCommandDto from 'types/model/logout/logout-command-dto';
import { fetchMenuByUserId, logout } from 'utils/api';
import ApiResultOld from 'types/common/api-result-old';
import LogoutCommand from 'types/model/logout/logout-command';
import RefreshTokenService from 'utils/refresh-token-service';
import CVSError from 'types/common/cvs-error';
import AccessTokenService from 'utils/access-token-service';
import GetMenuByUserDto from 'types/model/menu/get-menu-by-user-dto';

export interface MenuComponentProps {
    children?: ReactNode;
}

const MenuComponent: React.FC<MenuComponentProps> = (props: MenuComponentProps) => {
    const [navItems, setNavItems] = useState<NavItem[]>([]);

    useEffect(() => {
        fetchMenu();
    }, []);

    const fetchMenu = async () => {
        try {
            let userId = AccessTokenService.getInstance().getUserId();            

            const getMenuByUserDto: GetMenuByUserDto = await fetchMenuByUserId(userId);

            if(getMenuByUserDto && getMenuByUserDto.menuitems) {
                setNavItems(getMenuByUserDto.menuitems);
                // TODO: maybe save the menu items in the session? Now every page reload, the menu will be fetched again via api calls.
            }
          
        } catch (error: any) {
            console.log(`An error has occured while loading menu. Error:${error.message}`);
        }
    };

    const navigate = useNavigate();

    const [isConfirmPopupOneButtonOpen, setConfirmPopupOneButtonOpen] = useState<boolean>(false);
    const [confirmMessage, setConfirmMessage] = useState<string>('');

    const handleLogoutClick = () => {        
        setConfirmMessage('Weet U zeker dat U wilt uitloggen?');
        setConfirmPopupOneButtonOpen(true);
    };

    const handlePopUpConfirmLogoutClick = async () => {
        setConfirmPopupOneButtonOpen(false);

        try {
            let refreshtoken: string | null = RefreshTokenService.getInstance().getRefreshToken();

            if(!refreshtoken){
                return;
            }            

            let logoutCommand: LogoutCommand = { 
                refreshtoken: refreshtoken
            };

            // API-aanroep om uit te loggen
            const result: ApiResultOld<LogoutCommandDto> = await logout(logoutCommand);
    
            // Check of de logout succesvol is
            if (result.Ok && result.ReturnObject && result.ReturnObject.success) {
                return;
            } 
            else {
                throw new Error('Er is een fout opgetreden tijdens het uitloggen.');
            }            
        } catch (error: any) {
            setCvsError({
                id: 0,
                errorcode: 'E',
                message: `Er is een opgetreden tijdens het uitloggen. Foutmelding: ${error.message}`
            });
            setErrorPopupOpen(true);
        }
        finally {
            RefreshTokenService.getInstance().removeRefreshToken();
            navigate(`/login`);
        }        
    }; // NOTE: this function is now specific for closing the confirm popup after logging out

    const [isErrorPopupOpen, setErrorPopupOpen] = useState<boolean>(false);
    const [cvsError, setCvsError] = useState<CVSError>(() => {
        return {
            id: 1,
            errorcode: 'E12345',
            message: "Dit is een foutmelding"
        }
    });

    return (
        <>
            <div className='header-menu fixed'>
                <Sidebar>
                    {navItems.map((item) => (
                        <NavButton to={item.to} key={item.text} text={item.text} icon={item.icon} onClick={item.id === LOGOUT_ID ? handleLogoutClick : undefined} />
                    ))}
                </Sidebar>
                <SidebarGray>
                    {props.children}
                </SidebarGray>
            </div>

            <ConfirmPopup
                data-testid='confirm-popup'
                message={confirmMessage}
                isOpen={isConfirmPopupOneButtonOpen}
                onClose={() => setConfirmPopupOneButtonOpen(false)}
                buttons={[{ text: 'Bevestigen', dataTestId: 'button.confirm', onClick: handlePopUpConfirmLogoutClick, buttonType: {type:"Solid"}}]} />

            <ErrorPopup 
                error={cvsError} 
                isOpen={isErrorPopupOpen}
                onClose={() => setErrorPopupOpen(false)} />  
        </>
    );
};

export default MenuComponent;