import './client-create.css';
import Client from '../types/model/Client';
import React, { useEffect, useState } from "react";
import { Sidebar } from '../components/sidebar/sidebar';
import { NavButton } from '../components/nav/nav-button';
import { SidebarGray } from '../components/sidebar/sidebar-gray';
import { NavTitle } from '../components/nav/nav-title';
import SearchClients from '../components/clients/search-clients';
import { Copyright } from '../components/common/copyright';
import { Header } from '../components/common/header';
import { Label } from '../components/common/label';
import SaveButton from '../components/common/SaveButton';
import LabelField from '../components/common/label-field';
import { InputField } from '../components/common/input-field';
import { Dropdown, DropdownObject } from '../components/common/dropdown';
import { DatePicker } from '../components/common/datepicker';
import DomainObjectInput from '../components/common/domain-object-input';
import EmergencyPerson from '../types/model/EmergencyPerson';
import WorkingContract from '../types/model/WorkingContract';

function ClientCreate() {
    const [client, setClient] = useState<Client | null>(null);
    const [error, setError] = useState<string | null>(null);

    const emergencyPersons : EmergencyPerson[] = [];

    // const genders:  =
    const [genders, setGenders] = useState<DropdownObject[]>([
        {
            label: 'Man',
            value: 0
        },
        {
            label: 'Vrouw',
            value: 1
        },
        {
            label: 'Non-binair',
            value: 2   
        } // TODO: replace to contants file
    ]); // TODO: maybe make dynamic in the future

    const addEmergencyPerson = ():EmergencyPerson => {
        return {
            name: '',
            telephonenumber: ''
        };
    };

    return(
        <div className="flex flex-col lg:flex-row h-screen lg:h-auto">
            <div className='lg:flex w-full'>
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
                        <NavTitle lijstNaam="Cliënten" />
                        <SearchClients />
                    </SidebarGray>    
                </div>
                <div className="client-create-container">
                    <div className='client-create-header'>
                        <Header text="Cliënt aanmaken" className='client-create-header-main' />
                        <p className='client-create-header-sub'> - Velden met * zijn verplicht</p>
                    </div>

                    <Label text='Cliëntgegevens' strong={true} className='client-create-subheader' />

                    <div className='client-create-fields'>
                        <LabelField text='Voornaam' required={true}>
                            <InputField inputfieldtype={{type:'text'}} required={true} placeholder='Voornaam' />
                        </LabelField>

                        <LabelField text='Voorletters' required={true}>
                            <InputField inputfieldtype={{type:'text'}} required={true} placeholder='b.v. A B' />
                        </LabelField>

                        <LabelField text='Tussenvoegsel' required={false}>
                            <InputField inputfieldtype={{type:'text'}} required={false} placeholder='b.v. de' />
                        </LabelField>

                        <LabelField text='Achternaam' required={true}>
                            <InputField inputfieldtype={{type:'text'}} required={true} placeholder='Achternaam' />
                        </LabelField>

                        <LabelField text='Straatadres' required={true}>
                            <InputField inputfieldtype={{type:'text'}} required={true} placeholder='Adres' />
                        </LabelField>

                        <LabelField text='Huisnummer' required={true}>
                            <InputField inputfieldtype={{type:'text'}} required={true} placeholder='b.v. 11' className='house-number' />
                            <LabelField text='Toevoeging' required={false} className='house-number-addition'>
                                <InputField inputfieldtype={{type:'text'}} required={false} placeholder='b.v. A' className='house-number-addition-field' />
                            </LabelField>
                        </LabelField>

                        <LabelField text='Postcode' required={true}>
                            <InputField inputfieldtype={{type:'text'}} required={true} placeholder='b.v. 1234 AA' />
                        </LabelField>

                        <LabelField text='Woonplaats' required={true}>
                            <InputField inputfieldtype={{type:'text'}} required={true} placeholder='Woonplaats' />
                        </LabelField>

                        <LabelField text='Telefoon' required={true}>
                            <InputField inputfieldtype={{type:'text'}} required={true} placeholder='b.v. 0543-123456' />
                        </LabelField>

                        <LabelField text='E-mail' required={true}>
                            <InputField inputfieldtype={{type:'text'}} required={true} placeholder='b.v. mail@mailbox.com' />
                        </LabelField>

                        <LabelField text='Geboortedatum' required={true}>
                            <DatePicker required={true} placeholder='Selecteer een datum' />
                        </LabelField>

                        <LabelField text='Geslacht' required={true}>
                            <Dropdown options={genders} required={true} inputfieldname='geslacht' />
                        </LabelField>                       
                    </div>

                    <Label text='In geval van nood' strong={true} className='client-create-subheader' />

                    <DomainObjectInput label='Inputvelden voor contactpersoon' addObject={addEmergencyPerson} domainObjects={emergencyPersons} labelType='contactpersoon' typeName='EmergencyPerson' numMinimalRequired={1} />

                </div>
                <div className='button-container'>
                    <SaveButton
                    buttonText= "placeholder 1"
                    loadingText = "placeholder 2"
                    successText = "placeholder 3"
                    errorText = "placeholder 4"
                    onSave={() => console.log('Save successful')}
                    onError={() => console.error('Error saving')}
                    />
                </div>
            </div>
            <Copyright />
        </div>
    );
}

export default ClientCreate;