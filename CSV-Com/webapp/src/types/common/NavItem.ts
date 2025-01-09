export default interface NavItem {
    to: string; 
    text: string; 
    icon: string;
    id?: string;
};

export const LOGOUT_ID: string = 'logout';