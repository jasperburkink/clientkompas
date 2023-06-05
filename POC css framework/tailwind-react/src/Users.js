import './index.css';

import { Copyright } from './components/copyright';
import { Sidebar } from './components/sidebar';
import { SidebarGray } from './components/sidebarGray';
import { NavButton } from './components/navButton';
import { NavButtonGray } from './components/navButtonGray';
import { SearchInputField } from './components/searchInputField';
import { NavTitle } from './components/navTitle';
import { Button } from './components/button';
import { ProfilePicture } from './components/profilePicture';
import { InfoBox } from './components/infoBox';
import { InfoBoxPartClientInfo } from './components/infoBoxPartClientInfo';
import { InfoBoxPartTrajectInfo } from './components/infoBoxPartTrajectInfo';

// Deze data is een mock voor nu, zoals besproken in de sprint planning
var client = {
    naam: "Naam Naam",
    mobiel: "06-12345678",
    straat: "Straatstraat 2",
    email: "email@email.mail",
    adres: "1234 AB Adres"
};

function Users() {
    return (
        <div className="flex flex-col lg:flex-row h-screen lg:h-auto">
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
            <div className="flex w-screen md:w-fit overflow-scroll snap-x snap-mandatory md:overflow-visible md:grid md:grid-cols-3 md:grid-rows-infoBox md:m-5 lg:my-100px lg:mx-50px lg:gap-clienten">
                <InfoBox type="Client" button1="Cliënt Aanpassen" button2="Urenoverzicht" classNameMoreInfoBtns="md:bg-gradient-to-t md:from-white md:from-30% md:to-transparent md:to-30% ">
                     <InfoBoxPartClientInfo 
                        client={client}
                        naam="Naam Naam" mobiel="06-12345678" straat="Straatstraat 2" email="email@mail.mail" adres="1234 AB Adres" geboortedatum="1-2-2000" bsn="bsn" 
                        contactNaam="Naam Naam" contactStaat="Ongehuwd" contactMobiel="06-12345678" contactRijbewijs="Geen"
                        diagnose="Autisme" contract="/" uitkeringsvorm="/" van="/" werk="SBICT" tot="/" functie="/"
                        opmerking="Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
                     />
                </InfoBox>
                <ProfilePicture />
                <InfoBox type="Traject" button1="Traject Aanpassen" button2="Nieuw Traject" classNameMoreInfoBtns="md:bg-gradient-to-t md:from-white md:from-70% md:to-transparent md:to-70% ">
                    <InfoBoxPartTrajectInfo 
                        ordernummer="1234" trajectType="Jobcoach extern" opdrachtgever="UWV" 
                        begindatum="01-01-2022" einddatum="31-12-2022" budgetBedrag="5000" uurtarief="40"
                        teBestedenUren="125" coachWerktVoor="/"
                    />
                </InfoBox>
                <div className='h-[300px] flex flex-wrap justify-end content-end'>
                    <Button type="btnSollid" text="De-activeer cliënt" className="hidden md:block"/>
                </div>
                
            </div>
            <Copyright />
        </div>
    )
}

export default Users;