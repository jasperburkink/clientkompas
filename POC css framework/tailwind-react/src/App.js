import './App.css';
import './index.css';


import { Button } from './components/button';
import { Sidebar } from './components/sidebar';
import { NavButton } from './components/navButton';
import { SidebarGray } from './components/sidebarGray';
import { ProfilePicture } from './components/profilePicture';
import { InfoBox } from './components/infoBox';
import { InfoBoxPartClientInfo } from './components/infoBoxPartClientInfo';
import { Copyright } from './components/copyright';
import { InputField } from './components/inputField';
import { NavButtonGray } from './components/navButtonGray';
import { SearchInputField } from './components/searchInputField';
import { NavTitle } from './components/navTitle';


function App() {
  return (
    <div className="md:flex">
      <div className='md:flex'>
        <Sidebar> 
          <NavButton text="Cliënten" icon="Gebruikers"/>
          <NavButton text="Uren registratie" icon="Klok"/>
          <NavButton text="Organistatie" icon="Gebouw"/>
          <NavButton text="Gebruiker" icon="Gebruiker"/>
          <NavButton text="Uitloggen" icon="Uitloggen"/>
        </Sidebar>
        <SidebarGray> 
          <NavTitle lijstNaam="Naam Lijst"/>
          <SearchInputField />
          <div className="h-fit">
            <NavButtonGray text="Onderdeel lijst" />
            <NavButtonGray text="Placeholder" />
            <NavButtonGray text="HolderPlace" />
            <NavButtonGray text="Placie Holderie" />
          </div>
        </SidebarGray>
      </div>
      <div className=''>
        <Button type="Sollid" text="Test" href="#" />
        <Button type="NotSollid" text="Test2" href="#" />
        <InputField placeholder="Test"/>
        <ProfilePicture />
        <div className="md:flex md:flex-col w-2/3"> 
          <InfoBox>
             <InfoBoxPartClientInfo naam="Naam" mobiel="Mobiel" straat="Straat" email="Email" adres="Adres" geboortedatum="Geboorte" bsn="bsn" />
          </InfoBox>
          <InfoBox>
             <InfoBoxPartClientInfo naam="Naam" mobiel="Mobiel" straat="Straat" email="Email" adres="Adres" geboortedatum="Geboorte" bsn="bsn" />
          </InfoBox>
        </div>
      </div>
      <Copyright />
    </div>
  );
}

export default App;
