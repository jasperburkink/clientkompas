import '../../index.css';

import { Sidebar } from './sidebar';
import { SidebarGray } from './sidebar-gray';
import { NavButton } from '../nav/nav-button';
import { NavButtonGray } from '../nav/nav-button-gray';
import { SearchInputField } from '../common/search-input-field';
import { NavTitle } from '../nav/nav-title';

export function SidebarFull(props) {  
    //if(props.client == null) {
    //    return "loading...";
    //}
    return(
        <div className="lg:flex">
                <Sidebar>
                    <NavButton text="Cliënten" icon="Gebruikers"/>
                    <NavButton text="Uren registratie" icon="Klok"/>
                    <NavButton text="Organistatie" icon="Gebouw"/>
                    <NavButton text="Gebruiker" icon="Gebruiker"/>
                    <NavButton text="Uitloggen" icon="Uitloggen"/>
                </Sidebar>
                <SidebarGray>
                    <NavTitle lijstNaam="Cliëntenlijst" path="/ClientsAdd"/>
                    <SearchInputField />
                    <div className="h-fit">
                        {props.client.map((infoClient, pathNumber) => {
                            return (
                                <NavButtonGray key={pathNumber} path={"/Clients/" + pathNumber} text={infoClient.firstName+ " " + infoClient.prefixLastName + " " + infoClient.lastName} />
                            )
                        })}
                    </div>
                </SidebarGray>
            </div>
    )
}