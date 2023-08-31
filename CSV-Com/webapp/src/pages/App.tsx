import '../styles/App.css';
import '../index.css';

import { Header } from '../components/common/header';
import { Button } from '../components/common/button';
import { LinkButton } from '../components/common/link-button';
import { Sidebar } from '../components/sidebar/sidebar';
import { NavButton } from '../components/nav/nav-button';
import { SidebarGray } from '../components/sidebar/sidebar-gray';
import { ProfilePicture } from '../components/common/profile-picture';
import { InfoBox } from '../components/infobox/infobox';
import { InfoBoxPartClientInfo } from '../components/infobox/infobox-part-client-info';
import { Copyright } from '../components/common/copyright';
import { InputField } from '../components/common/input-field';
import { NavButtonGray } from '../components/nav/nav-button-gray';
import { SearchInputField } from '../components/common/search-input-field';
import { NavTitle } from '../components/nav/nav-title';

function App() {
    return (
        <div className="md:flex">
            <div className='md:flex'>
                <Sidebar>
                    <NavButton text="Cli�nten" icon="Gebruikers" />
                    <NavButton text="Uren registratie" icon="Klok" />
                    <NavButton text="Organistatie" icon="Gebouw" />
                    <NavButton text="Gebruiker" icon="Gebruiker" />
                    <NavButton text="Uitloggen" icon="Uitloggen" />
                </Sidebar>
                <SidebarGray>
                    <NavTitle lijstNaam="Naam Lijst" />
                    <SearchInputField />
                    <div className="h-fit">
                        <NavButtonGray text="Onderdeel lijst" />
                        <NavButtonGray text="Placeholder" />
                        <NavButtonGray text="HolderPlace" />
                        <NavButtonGray text="Placie Holderie" />
                    </div>
                </SidebarGray>
            </div>
            <div className='grid grid-cols-2'>
                <p>Header</p>
                <Header text="Test header. Dit zijn alle Components!" />

                <p>Button</p>
                <table className="table-auto">
                    <thead>
                        <tr className='border border-MainBlue'>
                            <th>Style</th>
                            <th>Name</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td><p>Solid</p></td>
                            <td><Button buttonType={{type:"Solid"}} text="Test1" onClick={()=> {alert('test1');}} /></td>
                        </tr>
                        <tr>
                            <td><p>Not solid</p></td>
                            <td><Button buttonType={{type:"NotSolid"}} text="Test2" onClick={()=> {alert('test2');}} /></td>
                        </tr>
                        <tr>
                            <td><p>Underline</p></td>
                            <td><Button buttonType={{type:"Underline"}} text="Test3" onClick={()=> {alert('test3');}} /></td>
                        </tr>
                    </tbody>
                </table>

                <p>Link button</p>
                <table className="table-auto">
                    <thead>
                        <tr className='border border-MainBlue'>
                            <th>Style</th>
                            <th>Name</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td><p>Solid</p></td>
                            <td><LinkButton buttonType={{type:"Solid"}} text="Test" href="https://specializedbrainsict.nl/" /></td>
                        </tr>
                        <tr>
                            <td><p>Not solid</p></td>
                            <td><LinkButton buttonType={{type:"NotSolid"}} text="Test2" href="https://specializedbrainsict.nl/" /></td>
                        </tr>
                        <tr>
                            <td><p>Underline</p></td>
                            <td><LinkButton buttonType={{type:"Underline"}} text="Test2" href="https://specializedbrainsict.nl/" /></td>
                        </tr>
                    </tbody>
                </table>               

                <label>InputField</label>
                <InputField placeholder="Test" />
                <ProfilePicture />
                <div className="md:flex md:flex-col w-2/3">
                    {/*<InfoBox>*/}
                    {/*    <InfoBoxPartClientInfo naam="Naam" mobiel="Mobiel" straat="Straat" email="Email" adres="Adres" geboortedatum="Geboorte" bsn="bsn" />*/}
                    {/*</InfoBox>*/}
                {/*    <InfoBox>*/}
                {/*        <InfoBoxPartClientInfo naam="Naam" mobiel="Mobiel" straat="Straat" email="Email" adres="Adres" geboortedatum="Geboorte" bsn="bsn" />*/}
                {/*    </InfoBox>*/}
                </div>
            </div>
            <Copyright />
        </div>
    );
}

export default App;
