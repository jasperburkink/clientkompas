import './App.css';
import '../index.css';

import { Header } from '../components/common/header';
import { Button } from '../components/common/button';
import { LinkButton } from '../components/common/link-button';
import { Sidebar } from '../components/sidebar/sidebar';
import { NavButton } from '../components/nav/nav-button';
import { SidebarGray } from '../components/sidebar/sidebar-gray';
import { ProfilePicture } from '../components/common/profile-picture';
import { Copyright } from '../components/common/copyright';
import { InputField } from '../components/common/input-field';
import { NavButtonGray } from '../components/nav/nav-button-gray';
import { NavTitle } from '../components/nav/nav-title';
import { InputFieldWithLabel } from '../components/common/input-field-with-label';
import { Dropdown } from '../components/common/dropdown';
import DropdownWithButton from "../components/common/dropdown-with-button"

function App() {
    const data = [
        {
            Label: "option 1",
            Value: 1,
        },
        {
            Label: "option 2",
            Value: 2,
        },
        {
            Label: "option 3",
            Value: 3,
        },
    ];
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
                    {/* <SearchInputField /> */}
                    <div className="h-fit">
                        <NavButtonGray text="Onderdeel lijst" />
                        <NavButtonGray text="Placeholder" />
                        <NavButtonGray text="HolderPlace" />
                        <NavButtonGray text="Placie Holderie" />
                    </div>
                </SidebarGray>
            </div>
            <div className='grid grid-cols-2 gap-10 m-5'>
                <p>Header</p>
                <Header text="Dit zijn alle beschikbare componenten." />

                <p>Button</p>
                <table className="w-full text-sm text-left text-gray-500 dark:text-gray-400">
                    <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                        <tr className='class="px-6 py-3'>
                            <th scope="col" className="px-6 py-3">Style</th>
                            <th scope="col" className="px-6 py-3">Name</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                            <th><p>Solid:</p></th>
                            <td><Button buttonType={{type:"Solid"}} text="Button1" className='w-200px h-50px' onClick={()=> {alert('Button1');}} /></td>
                        </tr>
                        <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                            <th><p>Not solid:</p></th>
                            <td><Button buttonType={{type:"NotSolid"}} text="Button2" className='w-200px h-50px' onClick={()=> {alert('Button2');}} /></td>
                        </tr>
                        <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                            <th><p>Underline:</p></th>
                            <td><Button buttonType={{type:"Underline"}} text="Button3" className='w-200px h-50px' onClick={()=> {alert('Button3');}} /></td>
                        </tr>
                    </tbody>
                </table>

                <p>Link button</p>
                <table className="w-full text-sm text-left text-gray-500 dark:text-gray-400">
                    <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                        <tr className='class="px-6 py-3'>
                            <th scope="col" className="px-6 py-3">Style</th>
                            <th scope="col" className="px-6 py-3">Name</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                            <th><p>Solid:</p></th>
                            <td><LinkButton buttonType={{type:"Solid"}} text="Button1" href="https://specializedbrainsict.nl/" /></td>
                        </tr>
                        <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                            <th><p>Not solid:</p></th>
                            <td><LinkButton buttonType={{type:"NotSolid"}} text="Button2" href="https://specializedbrainsict.nl/" /></td>
                        </tr>
                        <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                            <th><p>Underline:</p></th>
                            <td><LinkButton buttonType={{type:"Underline"}} text="Button3" href="https://specializedbrainsict.nl/" /></td>
                        </tr>
                    </tbody>
                </table>

                <p>Inputfield text empty</p>                
                <InputField inputFieldType={{type:'text'}} required={false} placeholder='Placeholder' />

                <p>Inputfield text with value</p>
                <InputField inputFieldType={{type:'text'}} required={false} value='Test' placeholder='Placeholder' />

                <p>Inputfield required</p>
                <InputField inputFieldType={{type:'text'}} required={true} placeholder='Placeholder' />

                <p>Inputfield with label</p>
                <InputFieldWithLabel text='TextField' inputFieldProps={{ required: false, placeholder:'Placeholder', inputFieldType:{ type:'text'} }} />

                <p>Inputfield required with label </p>
                <InputFieldWithLabel text='TextField' inputFieldProps={{ required: true, placeholder:'Placeholder', inputFieldType:{ type:'text'} }} />

                <p>ProfilePicture empty</p>
                <ProfilePicture />

                <p>ProfilePicture Maurice</p>
                <ProfilePicture pictureUrl='https://media.licdn.com/dms/image/C5603AQG1ibjUUZ7NFQ/profile-displayphoto-shrink_800_800/0/1655221337005?e=2147483647&v=beta&t=KKnYXDtk5PeT9utOaIAjUPjDLqk55-IrkCu1R5GuaRg' />
                
                
             
                <p>Dropdown</p>
                <Dropdown  array={data} required={false}  />
                

                <p>Dropdown with button</p>
                <DropdownWithButton array={data} required={false} />

            </div>
            <Copyright />
        </div>
    );
}

export default App;
