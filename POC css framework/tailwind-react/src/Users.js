import './index.css';

import { Copyright } from './components/copyright';
import { Sidebar } from './components/sidebar';
import { SidebarGray } from './components/sidebarGray';
import { NavButton } from './components/navButton';
import { NavButtonGray } from './components/navButtonGray';
import { SearchInputField } from './components/searchInputField';
import { NavTitle } from './components/navTitle';
import { Button } from './components/button';
import { InputField } from './components/inputField';
import { ProfilePicture } from './components/profilePicture';
import { InfoBox } from './components/infoBox';
import { InfoBoxPartClientInfo } from './components/infoBoxPartClientInfo';


function Users() {
    return (
        <div className="flex flex-col md:flex-row h-screen md:h-auto">
            <div className="lg:flex">
                <Sidebar>
                    <NavButton text="Cliënten" icon="Gebruikers"/>
                    <NavButton text="Uren registratie" icon="Klok"/>
                    <NavButton text="Organistatie" icon="Gebouw"/>
                    <NavButton text="Gebruiker" icon="Gebruiker"/>
                    <NavButton text="Uitloggen" icon="Uitloggen"/>
                </Sidebar>
                <SidebarGray>
                    <NavTitle lijstNaam="Cliëntenlijst"/>
                    <SearchInputField />
                    <div className="h-fit">
                        <NavButtonGray text="Onderdeel lijst" />
                        <NavButtonGray text="Placeholder" />
                        <NavButtonGray text="HolderPlace" />
                        <NavButtonGray text="Placie Holderie" />
                    </div>
                </SidebarGray>
            </div>
            <div className="flex w-screen overflow-scroll snap-x snap-mandatory md:overflow-visible md:grid md:grid-cols-3 md:grid-rows-infoBox md:mt-100px md:gap-50px md:mx-50px">
                <InfoBox type="moreInfo" button1="Cliënt Aanpassen" button2="Urenoverzicht">
                     <InfoBoxPartClientInfo 
                        naam="Naam Naam" mobiel="06-12345678" straat="Straatstraat 2" email="email@mail.mail" adres="1234 AB Adres" geboortedatum="1-2-2000" bsn="bsn" 
                        contactNaam="Naam Naam" contactStaat="Ongehuwd" contactMobiel="06-12345678" contactRijbewijs="Geen"
                        diagnose="Autisme" contract="/" uitkeringsvorm="/" van="/" werk="SBICT" tot="/" functie="/"
                        opmerking="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                     />
                </InfoBox>
                <ProfilePicture />
                <InfoBox button1="Traject Aanpassen" button2="Nieuw Traject">
                    <div className='p-3'>
                        <ul className="twoSpaceUlBox md:h-150px">
                            <li className="pieceTitle">Traject info</li>
                            <InputField type="dropdown"> 
                                <ul>
                                    <li>Test1</li>
                                    <li>Test2</li>
                                </ul>
                            </InputField>
                            <li>Begindatum</li>
                            <li>Traject type</li>
                            <li>Einddatum</li>
                        </ul>
                    </div>
                </InfoBox>
                <Button type="btnSollid" text="De-activeer cliënt" className="hidden"/>
                
            </div>
            <Copyright />
        </div>
    )
}

export default Users;