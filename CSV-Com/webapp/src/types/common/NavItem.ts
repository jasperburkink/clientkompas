export default interface NavItem {
    to: string; 
    text: string; 
    icon: string
};

export const DefaultNavItems: NavItem[] = [
    { to:'/clients', text: 'Cliënten', icon: 'Gebruikers' },
    { to:'/hours_registration', text: 'Uren registratie', icon: 'Klok' },
    { to:'/organizations', text: 'Organisatie', icon: 'Gebouw' },
    { to:'/users', text: 'Gebruiker', icon: 'Gebruiker' },
    { to:'/logout', text: 'Uitloggen', icon: 'Uitloggen' },
  ];