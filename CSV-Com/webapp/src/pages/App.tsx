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
                    <NavButton text="Cliënten" icon="Gebruikers" />
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
                <Header text="Dit zijn alle beschikbare componenten." />

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
                            <td><p>Solid:</p></td>
                            <td><Button buttonType={{type:"Solid"}} text="Button1" className='w-200px h-50px' onClick={()=> {alert('Button1');}} /></td>
                        </tr>
                        <tr>
                            <td><p>Not solid:</p></td>
                            <td><Button buttonType={{type:"NotSolid"}} text="Button2" className='w-200px h-50px' onClick={()=> {alert('Button2');}} /></td>
                        </tr>
                        <tr>
                            <td><p>Underline:</p></td>
                            <td><Button buttonType={{type:"Underline"}} text="Button3" className='w-200px h-50px' onClick={()=> {alert('Button3');}} /></td>
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
                            <td><p>Solid:</p></td>
                            <td><LinkButton buttonType={{type:"Solid"}} text="Button1" href="https://specializedbrainsict.nl/" /></td>
                        </tr>
                        <tr>
                            <td><p>Not solid:</p></td>
                            <td><LinkButton buttonType={{type:"NotSolid"}} text="Button2" href="https://specializedbrainsict.nl/" /></td>
                        </tr>
                        <tr>
                            <td><p>Underline:</p></td>
                            <td><LinkButton buttonType={{type:"Underline"}} text="Button3" href="https://specializedbrainsict.nl/" /></td>
                        </tr>
                    </tbody>
                </table>               

                <p>InputField</p>
                <InputField placeholder="Test" />

                <p>ProfilePicture</p>                
                <ProfilePicture />

                <p>ProfilePicture</p>
                <ProfilePicture pictureUrl='https://media.licdn.com/dms/image/C5603AQG1ibjUUZ7NFQ/profile-displayphoto-shrink_800_800/0/1655221337005?e=2147483647&v=beta&t=KKnYXDtk5PeT9utOaIAjUPjDLqk55-IrkCu1R5GuaRg' />
                
            </div>
            <Copyright />
        </div>
    );
}

export default App;
