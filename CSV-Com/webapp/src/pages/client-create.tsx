import './client-create.css';
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

function ClientCreate() {
    const [client, setClient] = useState<Client | null>(null);
    const [error, setError] = useState<string | null>(null);


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
                        <InputFieldWithLabel text='TextField' inputFieldProps={{ required: false, placeholder:'Placeholder', inputfieldtype:{ type:'text'} }}  />
                        <InputFieldWithLabel text='TextField' inputFieldProps={{ required: false, placeholder:'Placeholder', inputfieldtype:{ type:'text'} }}  />
                        <InputFieldWithLabel text='TextField' inputFieldProps={{ required: false, placeholder:'Placeholder', inputfieldtype:{ type:'text'} }}  />
                        <InputFieldWithLabel text='TextField' inputFieldProps={{ required: false, placeholder:'Placeholder', inputfieldtype:{ type:'text'} }}  />
                        <InputFieldWithLabel text='TextField' inputFieldProps={{ required: false, placeholder:'Placeholder', inputfieldtype:{ type:'text'} }}  />
                        <InputFieldWithLabel text='TextField' inputFieldProps={{ required: false, placeholder:'Placeholder', inputfieldtype:{ type:'text'} }}  />
                        <InputFieldWithLabel text='TextField' inputFieldProps={{ required: false, placeholder:'Placeholder', inputfieldtype:{ type:'text'} }}  />
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