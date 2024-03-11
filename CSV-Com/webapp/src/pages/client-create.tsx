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
import DropdownWithButton, { IDropdownObject } from "../components/common/dropdown-with-button";
import { DatePicker } from '../components/common/datepicker';
import Textarea from "../components/common/Textarea";
import DomainObjectInput from '../components/common/domain-object-input';
import { SlideToggleLabel } from '../components/common/slide-toggle-label';
import EmergencyPerson from '../types/model/EmergencyPerson';
import WorkingContract from '../types/model/WorkingContract';
import Diagnosis from '../types/model/Diagnosis';
import ConfirmPopup from "../components/common/confirm-popup";
import ErrorPopup from '../components/common/error-popup';
import {keyof} from "ts-keyof";
import CvsError from '../types/common/cvs-error';
import { fetchBenefitForms, fetchDiagnosis, fetchMaritalStatuses, fetchDriversLicences, saveClient } from '../utils/api';

const ClientCreate = () => {
    const initialClient: Client = { 
        id: 0,
        firstname: '',
        initials: '',
        lastname: '',
        gender: '',
        streetname: '',
        housenumber: '',
        postalcode: '',
        residence: '',
        telephonenumber: '',
        dateofbirth: new Date(),
        emailaddress: '',
        maritalstatus: '',
        driverslicences: ''
    };

    const handleClientInputChange = (fieldName: string, value: string) => {
        setClient(prevClient => ({
            ...prevClient,
            [fieldName]: value
        }));
    };

    const [client, setClient] = useState<Client>(initialClient);
    const [error, setError] = useState<string | null>(null);

    const [isErrorPopupOpen, setErrorPopupOpen] = useState<boolean>(false);
    const [cvsError, setCvsError] = useState<CvsError>(() => {
        return {
            id: 1,
            errorcode: 'E12345',
            message: "Dit is een foutmelding"
        }
    });

    const emergencyPersons : EmergencyPerson[] = [];
    const [diagnoses, setDiagnoses] = useState<IDropdownObject[]>([]);
    const [benefitForms, setBenefitForms] = useState<IDropdownObject[]>([]);
    const [maritalStatuses, setMaritalStatuses] = useState<DropdownObject[]>([]);
    const [driversLicences, setDriversLicences] = useState<DropdownObject[]>([]);
    const workingContracts : WorkingContract[] = [];

    const gendersDropdownOptions: DropdownObject[] = [
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
    ]; // TODO: maybe make dynamic in the future

    const doelgroepDropdownOptions: DropdownObject[] = [
        {
            label: 'Nee',
            value: 0
        },
        {
            label: 'Ja',
            value: 1
        } // TODO: replace to contants file
    ];

    const addEmergencyPerson = ():EmergencyPerson => {
        return {
            name: '',
            telephonenumber: ''
        };
    };

    const addWorkingContract = ():WorkingContract => {
        return {
            companyname: '',
            fromdate: new Date(),
            todate: new Date(),
            contracttype: '',
            function: ''
        };
    };

    useEffect(() => {
        const loadDiagnoses = async () => { 
            try {
                const apiDiagnoses = await fetchDiagnosis();
                const formattedDiagnoses = apiDiagnoses.map(diagnosis => ({
                    value: diagnosis.id,
                    label: diagnosis.name
                }));
                setDiagnoses(formattedDiagnoses);
            } catch (error) {
                console.error('Error loading diagnoses:', error);
                setCvsError({
                    errorcode: 'TEST',
                    id: 1,
                    message: 'Er is iets fout gegaan!'
                });
                setErrorPopupOpen(true);
                //tODO: show errormessage popup
            }
        };

        const loadBenifitForms = async () => { 
            try {
                const apiBenefitForms = await fetchBenefitForms();
                const formattedBenefitForms = apiBenefitForms.map(benefitForm => ({
                    value: benefitForm.id,
                    label: benefitForm.name
                }));
                setBenefitForms(formattedBenefitForms);
            } catch (error) {
                console.error('Error loading benefitforms:', error);
                //tODO: show errormessage popup
            }
        };

        const loadMaritalStatuses = async () => { 
            try {
                const apiMaritalStatuses = await fetchMaritalStatuses();
                const formattedMaritalStatuses = apiMaritalStatuses.map(maritalStatus => ({
                    value: maritalStatus.id,
                    label: maritalStatus.name
                }));
                setMaritalStatuses(formattedMaritalStatuses);
            } catch (error) {
                console.error('Error loading marital statuses:', error);
                //tODO: show errormessage popup
            }
        };

        const loadDriversLicences = async () => { 
            try {
                const apiDriversLicences = await fetchDriversLicences();
                const formattedDriversLicences = apiDriversLicences.map(driverLicence => ({
                    value: driverLicence.id,
                    label: `${driverLicence.category} (${driverLicence.description})`
                }));
                setDriversLicences(formattedDriversLicences);
            } catch (error) {
                console.error('Error loading drivers licences:', error);
                //tODO: show errormessage popup
            }
        };

        loadDiagnoses();
        loadBenifitForms();
        loadMaritalStatuses();
        loadDriversLicences();
    }, []);    

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
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='Voornaam' 
                                value={client.firstname} 
                                onChange={(value) => handleClientInputChange(keyof(client.firstname), value)} />
                        </LabelField>

                        <LabelField text='Voorletters' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. A B' 
                                value={client.initials}
                                onChange={(value) => handleClientInputChange('initials', value)} />
                        </LabelField>

                        <LabelField text='Tussenvoegsel' required={false}>
                            <InputField inputfieldtype={{type:'text'}} required={false} placeholder='b.v. de' value={client.prefixlastname} />
                        </LabelField>

                        <LabelField text='Achternaam' required={true}>
                            <InputField inputfieldtype={{type:'text'}} required={true} placeholder='Achternaam' value={client.lastname} />
                        </LabelField>

                        <LabelField text='Straatadres' required={true}>
                            <InputField inputfieldtype={{type:'text'}} required={true} placeholder='Adres' value={client.streetname} />
                        </LabelField>

                        <LabelField text='Huisnummer' required={true}>
                            <InputField inputfieldtype={{type:'text'}} required={true} placeholder='b.v. 11' className='house-number' value={client.housenumber} />
                            <LabelField text='Toevoeging' required={false} className='house-number-addition'>
                                <InputField inputfieldtype={{type:'text'}} required={false} placeholder='b.v. A' className='house-number-addition-field' value={client.housenumberaddition} />
                            </LabelField>
                        </LabelField>

                        <LabelField text='Postcode' required={true}>
                            <InputField inputfieldtype={{type:'text'}} required={true} placeholder='b.v. 1234 AA' value={client.postalcode} />
                        </LabelField>

                        <LabelField text='Woonplaats' required={true}>
                            <InputField inputfieldtype={{type:'text'}} required={true} placeholder='Woonplaats' value={client.residence} />
                        </LabelField>

                        <LabelField text='Telefoon' required={true}>
                            <InputField inputfieldtype={{type:'text'}} required={true} placeholder='b.v. 0543-123456' value={client.telephonenumber} />
                        </LabelField>

                        <LabelField text='E-mail' required={true}>
                            <InputField inputfieldtype={{type:'text'}} required={true} placeholder='b.v. mail@mailbox.com' value={client.emailaddress} />
                        </LabelField>

                        <LabelField text='Geboortedatum' required={true}>
                            <DatePicker required={true} placeholder='Selecteer een datum' value={client.dateofbirth} />
                        </LabelField>

                        <LabelField text='Geslacht' required={true}>
                            <Dropdown options={gendersDropdownOptions} required={true} inputfieldname='geslacht'  />
                        </LabelField>                       
                    </div>

                    <DomainObjectInput label='In geval van nood' addObject={addEmergencyPerson} domainObjects={emergencyPersons} labelType='contactpersoon' typeName='EmergencyPerson' numMinimalRequired={1} />

                    <div className='client-remarks'>
                        <Label text='Opmerkingen' />
                        <Textarea text="Voeg opmerkingen toe" />
                    </div>

                    <SlideToggleLabel textColapsed='Klap uit voor meer opties' textExpanded='Klap in' >
                        <div className='client-extra-info'>
                            <LabelField text='Diagnose(s)' required={false}>
                                <DropdownWithButton options={diagnoses} required={false} inputfieldname='diagnoses' />
                            </LabelField>

                            <LabelField text='Uitkeringsvorm' required={false}>
                                <DropdownWithButton options={benefitForms} required={false} inputfieldname='benefitforms' />
                            </LabelField>

                            <LabelField text='Burgerlijke staat' required={false}>
                                <Dropdown options={maritalStatuses} required={false} inputfieldname='maritalstatuses' />
                            </LabelField>

                            <LabelField text='Rijbewijs' required={false}>
                                <DropdownWithButton options={driversLicences} required={false} inputfieldname='driverslicences' />
                            </LabelField>

                            <LabelField text='Doelgroepregister' required={false}>
                                <Dropdown options={doelgroepDropdownOptions} required={false} inputfieldname='doelgroepregister' />
                            </LabelField>
                        </div>

                        <DomainObjectInput label='Werkervaring' addObject={addWorkingContract} domainObjects={workingContracts} labelType='werkervaring' typeName='WorkingContract' numMinimalRequired={1} />
                    </SlideToggleLabel>

                </div>
                <div className='button-container'>
                    <SaveButton
                    buttonText= "Opslaan"
                    loadingText = "Bezig met oplaan"
                    successText = "Cliënt opgeslagen"
                    errorText = "Fout tijdens opslaan"
                    onSave={() => {
                        try {
                            saveClient(client!);
                        } catch (error) {
                            //tODO show errors!
                            alert('Error while saving a client!!!');
                        }
                        
                        console.log('Save successful')
                    }}
                    onError={() => console.error('Error saving')}
                    />
                </div>
            </div>

            <ErrorPopup 
                error={cvsError} 
                isOpen={isErrorPopupOpen}
                onClose={() => setErrorPopupOpen(false)} />  
            <Copyright />
        </div>
    );
}

export default ClientCreate;