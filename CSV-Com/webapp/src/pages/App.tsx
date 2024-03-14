import './App.css';
import '../index.css';
import { useState } from 'react';
import { Header } from '../components/common/header';
import { Label } from '../components/common/label';
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
import { SlideToggleLabel } from '../components/common/slide-toggle-label';
import { DatePicker } from '../components/common/datepicker';
import Textarea from "../components/common/Textarea";
import ConfirmPopup from "../components/common/confirm-popup";
import SaveButton from '../components/common/SaveButton';
import { Dropdown } from '../components/common/dropdown';
import DropdownWithButton from "../components/common/dropdown-with-button";
import PasswordField from '../components/common/password-field';
import ErrorPopup from '../components/common/error-popup';
import CvsError from '../types/common/cvs-error';
import DomainObjectInput from '../components/common/domain-object-input';
import EmergencyPerson from '../types/model/EmergencyPerson';
import WorkingContract from '../types/model/WorkingContract';
import LabelField from '../components/common/label-field';

function App() {
    const data = [
        {
            value: 1,
            label: "ADHD",
        },
        {
            value: 2,
            label: "Asperger",
        },
        {
            value: 3,
            label: "SBICT",
        },
    ];

    const [isConfirmPopupOneButtonOpen, setConfirmPopupOneButtonOpen] = useState<boolean>(false);
    const [isConfirmPopupTwoButtonsOpen, setConfirmPopupTwoButtonsOpen] = useState<boolean>(false);
    const handlePopUpConfirmClick = () => {
        alert('Comfirm popup');
        setConfirmPopupOneButtonOpen(false);
        setConfirmPopupTwoButtonsOpen(false);
    };

    const handlePopUpCancelClick = () => {
        alert('Cancel popup');
        setConfirmPopupOneButtonOpen(false);
        setConfirmPopupTwoButtonsOpen(false);
    };
    
    const [isErrorPopupOpen, setErrorPopupOpen] = useState<boolean>(false);
    const cvsError: CvsError = {
        id: 1,
        errorcode: 'E12345',
        message: "Dit is een foutmelding."
    };

    const emergencyPerson: EmergencyPerson = {
        name: 'Jasper Burkink',
        telephonenumber: '0123456789'
      };

      const emergencyPerson2: EmergencyPerson = {
        name: 'Jan Jansen',
        telephonenumber: '0123456789'
      };

    const [emergencyPeople, setEmergencyPeople] = useState<EmergencyPerson[]>([emergencyPerson, emergencyPerson2]);

    const addEmergencyPerson = ():EmergencyPerson => {
        return {
            name: '',
            telephonenumber: ''
        };
    };

    const handleEmergencyPersonChange = (updatedEmergencyPerson: EmergencyPerson, index: number) => {
        const updatedEmergencyPeople = emergencyPeople;    
        updatedEmergencyPeople[index] = updatedEmergencyPerson;
        setEmergencyPeople(updatedEmergencyPeople);
    };

    const onRemoveEmergencyPerson = (emergencyPerson: EmergencyPerson):void => {
        console.log(`onRemoveObject`);
    }
      
    const workingContract: WorkingContract = {
        companyname: 'SB-ICT',
        contracttype: 'Tijdelijk',
        fromdate: new Date('2020-01-01'),
        todate: new Date('2021-01-01'),
        function: 'Programmeur'
      };

      const workingContract2: WorkingContract = {
        companyname: 'Welkoop',
        contracttype: 'Tijdelijk',
        fromdate: new Date('1995-01-01'),
        todate: new Date('1998-01-01'),
        function: 'Verkoper'
      };

      const [workingContracts, setWorkingContracts] = useState<WorkingContract[]>([workingContract, workingContract2]);

      const addWorkingContract = ():WorkingContract => {
        return {
            companyname: '',
            contracttype: '',
            fromdate: new Date(),
            todate: new Date(),
            function: ''
        };
    };

    const handleWorkingContractChange = (updatedWorkingContract: WorkingContract, index: number) => {
        const updatedWorkingContracts = workingContracts;    
        updatedWorkingContracts[index] = updatedWorkingContract;
        setWorkingContracts(updatedWorkingContracts);
    };

    const onRemoveWorkingContract = (workingContract: WorkingContract):void => {
        console.log(`onRemoveObject`);
    }

    return (
        <div className="flex flex-col lg:flex-row h-screen lg:h-auto">
            <div id='staticSidebar' className='sidebarContentPush'></div>
                {/* TODO: Move menu to own control. Is already a task on the backlog. */}
                <div className='header-menu fixed'>
                    <Sidebar>
                        <NavButton text="Cliënten" icon="Gebruikers" />
                        <NavButton text="Uren registratie" icon="Klok" />
                        <NavButton text="Organistatie" icon="Gebouw" />
                        <NavButton text="Gebruiker" icon="Gebruiker" />
                        <NavButton text="Uitloggen" icon="Uitloggen" />
                    </Sidebar>
                    <SidebarGray>
                        <NavTitle lijstNaam="Empty list" />                        
                    </SidebarGray>    
                </div>
                <div className='lg:grid lg:grid-cols-2 lg:gap-10 pt-[140px] m-2'>
                    <p>Header</p>
                    <Header text="Dit zijn alle beschikbare componenten." />

                    <p>Label</p>         
                    <table className="w-full text-sm text-left text-gray-500 dark:text-gray-400">
                        <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                            <tr className='class="px-6 py-3'>
                                <th scope="col" className="px-6 py-3">Style</th>
                                <th scope="col" className="px-6 py-3">Name</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                <th><p>Standard:</p></th>
                                <td><Label text='Dit is een label!' /></td>
                            </tr>
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                <th><p>Strong:</p></th>
                                <td><Label text='Dit is een label!' strong={true} /></td>
                            </tr>
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                <th><p>Underline:</p></th>
                                <td><Label text='Dit is een label!' underline={true} /></td>
                            </tr>
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">                            
                                <th><p>Cursive:</p></th>
                                <td><Label text='Dit is een label!' cursive={true} /></td>
                            </tr>
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                <th><p>Strong, Underline, Cursive:</p></th>
                                <td><Label text='Dit is een label!' strong={true} underline={true} cursive={true} /></td>
                            </tr>
                        </tbody>
                    </table>

                    <p>Inputfield text empty</p>                
                    <InputField inputfieldtype={{type:'text'}} required={false} placeholder='Placeholder' />

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
                    <InputField inputfieldtype={{type:'text'}} required={false} placeholder='Placeholder' />

                    <p>Inputfield text with value</p>
                    <InputField inputfieldtype={{type:'text'}} required={false} value='Test' placeholder='Placeholder' />

                    <p>Inputfield required</p>
                    <InputField inputfieldtype={{type:'text'}} required={true} placeholder='Placeholder' />

                    <p>LabelField with inputfield</p>
                    <LabelField text='LabelField inputfield' required={true}>
                        <InputField inputfieldtype={{type:'text'}} required={true} placeholder='Placeholder' />
                    </LabelField>                    

                    <p>Slide toggle label</p>
                    <SlideToggleLabel textColapsed='Klap uit!' textExpanded='Klap in!' >
                        <div className='p-5 bg-mainGray text-mainBlue rounded-2xl border-2 border-black w-full h-44'>Dit is een paneel wat inklapbaar is!!!</div>
                    </SlideToggleLabel>

                    <p>Date picker</p>                    
                    <DatePicker placeholder='Selecteer een datum' required={true} />

                    <p>LabelField with datepicker</p>
                    <LabelField text='LabelField datepicker' required={true}>
                        <DatePicker placeholder='Selecteer een datum' required={true} />
                    </LabelField>

                    <p>ProfilePicture empty</p>
                    <ProfilePicture />

                    <p>ProfilePicture Maurice</p>
                    <ProfilePicture pictureUrl='https://media.licdn.com/dms/image/C5603AQG1ibjUUZ7NFQ/profile-displayphoto-shrink_800_800/0/1655221337005?e=2147483647&v=beta&t=KKnYXDtk5PeT9utOaIAjUPjDLqk55-IrkCu1R5GuaRg' />
                    
                    <p>Textarea component</p>
                    <Textarea text="Voeg een opmerking toe"/>

                    <p>Dropdown</p>
                    <Dropdown options={data} required={false} inputfieldname='dropdown' />

                    <p>LabelField with dropdownlist</p>
                    <LabelField text='LabelField dropdownlist' required={true}>
                        <Dropdown options={data} required={false} inputfieldname='dropdown' />
                    </LabelField>
                
                    <p>Dropdown with button</p>
                    <DropdownWithButton options={data} required={false} inputfieldname='dropdownWithButton' />

                    <p>Wachtwoord input</p>
                    <PasswordField inputfieldname='password' placeholder='Wachtwoord' />                                                                  

                    <p>Save button component</p>
                    <SaveButton
                    buttonText= "placeholder 1"
                    loadingText = "placeholder 2"
                    successText = "placeholder 3"
                    errorText = "placeholder 4"
                    onSave={() => console.log('Save successful')}
                    onError={() => console.error('Error saving')}
                    />

                    <p>Foutmelding pop-up</p>
                    <Button buttonType={{type:"Solid"}} text="Toon foutmelding" className='w-200px h-50px' 
                    onClick=
                    {
                        ()=> {setErrorPopupOpen(true);}
                    } />
                    <ErrorPopup isOpen={isErrorPopupOpen} onClose={() => {}} error={cvsError} />
              
                    <DomainObjectInput                    
                        label='Inputvelden voor contactpersoon' 
                        addObject={addEmergencyPerson} 
                        domainObjects={emergencyPeople} 
                        labelType='contactpersoon' 
                        typeName='EmergencyPerson' 
                        numMinimalRequired={1}
                        onRemoveObject={onRemoveEmergencyPerson}
                        onChangeObject={handleEmergencyPersonChange} />

                    <DomainObjectInput 
                        label='Inputvelden voor werkervaring' 
                        addObject={addWorkingContract} 
                        domainObjects={workingContracts} 
                        labelType='werkervaring' 
                        typeName='WorkingContract' 
                        numMinimalRequired={2}
                        onRemoveObject={onRemoveWorkingContract}
                        onChangeObject={handleWorkingContractChange} />

                    <p>Bevestigings pop-up met één button</p>
                    <Button buttonType={{type:"Solid"}} text="Toon pop-up met één button" className='w-200px h-50px' 
                    onClick={() => setConfirmPopupOneButtonOpen(true)} />
                    <ConfirmPopup
                    message="Dit is een bevestigings pop-up met één knop"
                    isOpen={isConfirmPopupOneButtonOpen}
                    onClose={handlePopUpCancelClick}
                    buttons={[{ text: 'Bevestigen', onClick: handlePopUpConfirmClick, buttonType: {type:"Solid"}}]} />

                    <p>Bevestigings pop-up met twee buttons</p>
                    <Button buttonType={{type:"Solid"}} text="Toon pop-up met twee buttons" className='w-200px h-50px' 
                        onClick={() => setConfirmPopupTwoButtonsOpen(true)} />
                        <ConfirmPopup
                        message="Dit is een bevestigings pop-up met twee knoppen"
                        isOpen={isConfirmPopupTwoButtonsOpen}
                        onClose={handlePopUpCancelClick}
                        buttons={
                            [
                                { text: 'Bevestigen', onClick: handlePopUpConfirmClick, buttonType: {type:"Solid"}},
                                { text: 'Annuleren', onClick: handlePopUpCancelClick, buttonType: {type:"NotSolid"}},
                            ]} />

                    <p>Foutmelding pop-up</p>
                    <Button buttonType={{type:"Solid"}} text="Toon foutmelding" className='w-200px h-50px' 
                        onClick={() => setErrorPopupOpen(true)} />
                        <ErrorPopup 
                        error={cvsError} 
                        isOpen={isErrorPopupOpen}
                        onClose={() => setErrorPopupOpen(false)} />  
                </div>
            <Copyright />
        </div>
    );
}

export default App;