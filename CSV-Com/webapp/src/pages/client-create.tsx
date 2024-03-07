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
import { InputFieldWithLabel } from '../components/common/input-field-with-label';
import { Dropdown, DropdownObject } from '../components/common/dropdown';
import { DatePicker } from '../components/common/datepicker';

function ClientCreate() {
    const [client, setClient] = useState<Client | null>(null);    
    const [error, setError] = useState<string | null>(null);

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

                    <div className='client-create-fields grid'>
                        <InputFieldWithLabel text='Voornaam' inputFieldProps={{ required: true, placeholder:'Voornaam', inputfieldtype:{ type:'text'} }}  />
                        <InputFieldWithLabel text='Voorletters' inputFieldProps={{ required: true, placeholder:'b.v. A B', inputfieldtype:{ type:'text'} }}  />
                        <InputFieldWithLabel text='Tussenvoegsel' inputFieldProps={{ required: false, placeholder:'b.v. de', inputfieldtype:{ type:'text'} }}  />
                        <InputFieldWithLabel text='Achternaam' inputFieldProps={{ required: true, placeholder:'Achternaam', inputfieldtype:{ type:'text'} }}  />
                        <InputFieldWithLabel text='Straatadres' inputFieldProps={{ required: true, placeholder:'Adres', inputfieldtype:{ type:'text'} }}  />
                        <InputFieldWithLabel text='Huisnummer' inputFieldProps={{ required: true, placeholder:'b.v. 11', inputfieldtype:{ type:'text'} }}  />
                        <InputFieldWithLabel text='Toevoeging' inputFieldProps={{ required: false, placeholder:'b.v. A', inputfieldtype:{ type:'text'} }}  />
                        <InputFieldWithLabel text='Postcode' inputFieldProps={{ required: true, placeholder:'b.v. 1234 AA', inputfieldtype:{ type:'text'} }}  />
                        <InputFieldWithLabel text='Woonplaats' inputFieldProps={{ required: true, placeholder:'Woonplaats', inputfieldtype:{ type:'text'} }}  />
                        <InputFieldWithLabel text='Telefoon' inputFieldProps={{ required: true, placeholder:'b.v. 0543-123456', inputfieldtype:{ type:'text'} }}  />                        
                        <InputFieldWithLabel text='E-mail' inputFieldProps={{ required: true, placeholder:'b.v. mail@mailbox.com', inputfieldtype:{ type:'text'} }}  />
                        <div>
                            <Label text='Geboortedatum*' /><DatePicker placeholder='Selecteer een datum' />
                        </div>
                        <div>
                            <Label text='Geslacht*' /><Dropdown options={genders} required={true} inputfieldname='dropdown' />
                        </div>
                    </div>

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