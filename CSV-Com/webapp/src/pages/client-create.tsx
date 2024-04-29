import './client-create.css';
import React, { useEffect, useState } from "react";
import { Sidebar } from 'components/sidebar/sidebar';
import { NavButton } from 'components/nav/nav-button';
import { SidebarGray } from 'components/sidebar/sidebar-gray';
import { NavTitle } from 'components/nav/nav-title';
import SearchClients from 'components/clients/search-clients';
import { Copyright } from 'components/common/copyright';
import { Header } from 'components/common/header';
import { Label } from 'components/common/label';
import SaveButton from 'components/common/save-button';
import LabelField from 'components/common/label-field';
import { InputField } from 'components/common/input-field';
import { Dropdown, DropdownObject } from 'components/common/dropdown';
import DropdownWithButton, { IDropdownObject } from "components/common/dropdown-with-button";
import { DatePicker } from 'components/common/datepicker';
import Textarea from "components/common/text-area";
import DomainObjectInput from 'components/common/domain-object-input';
import { SlideToggleLabel } from 'components/common/slide-toggle-label';
import EmergencyPerson from 'types/model/EmergencyPerson';
import WorkingContract from 'types/model/WorkingContract';
import Diagnosis from 'types/model/Diagnosis';
import ConfirmPopup from "components/common/confirm-popup";
import ErrorPopup from 'components/common/error-popup';
import CvsError from 'types/common/cvs-error';
import { fetchBenefitForms, fetchDiagnosis, fetchMaritalStatuses, fetchDriversLicences, saveClient } from 'utils/api';
import Client from 'types/model/Client';
import { Moment } from 'moment';
import CVSError from 'types/common/cvs-error';
import { FieldOrderWorkingContract } from 'types/common/fieldorder';
import ApiResult from 'types/common/api-result';

const ClientCreate = () => {
    const initialClient: Client = { 
        id: 0,
        firstname: '',
        initials: '',
        lastname: '',
        gender: 0,
        streetname: '',
        housenumber: '',
        postalcode: '',
        residence: '',
        telephonenumber: '',        
        emailaddress: '',
        driverslicences: [],
        doelgroepregister: false,
        emergencypeople: [],
        workingcontracts: [],
        benefitforms: [],
    };

    const [client, setClient] = useState<Client>(initialClient);
    const [error, setError] = useState<string | null>(null);

    const [confirmMessage, setConfirmMessage] = useState<string>('');
    const [isConfirmPopupOneButtonOpen, setConfirmPopupOneButtonOpen] = useState<boolean>(false);
    const handlePopUpConfirmClick = () => {
        setConfirmPopupOneButtonOpen(false);
    };
    const handlePopUpCancelClick = () => {
        setConfirmPopupOneButtonOpen(false);
    };

    const [isErrorPopupOpen, setErrorPopupOpen] = useState<boolean>(false);
    const [cvsError, setCvsError] = useState<CvsError>(() => {
        return {
            id: 1,
            errorcode: 'E12345',
            message: "Dit is een foutmelding"
        }
    });

    const [diagnoses, setDiagnoses] = useState<IDropdownObject[]>([]);
    const [benefitForms, setBenefitForms] = useState<IDropdownObject[]>([]);
    const [maritalStatuses, setMaritalStatuses] = useState<DropdownObject[]>([]);
    const [driversLicences, setDriversLicences] = useState<DropdownObject[]>([]);
    const workingContracts : WorkingContract[] = [];
    
    const handleClientInputChange = (fieldName: string, value: string) => {
        setClient(prevClient => ({
            ...prevClient,
            [fieldName]: value
        }));
    };

    const handleClientDatePickerChange = (fieldName: string, value: Moment | null) => {
        setClient(prevClient => ({
            ...prevClient,
            [fieldName]: value
        }));
    };

    const handleDropdownChange = (fieldName: string, value: string | number) => {
        setClient(prevClient => ({
            ...prevClient,
            [fieldName]: value
        }));
    };

    const handleDropdownWithButtonChange = (fieldName: string, value: number[]) => {
        setClient(prevClient => ({
            ...prevClient,
            [fieldName]: value
        }));
    };

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
        } // TODO: replace with yes/no dropdown component
    ];

    const contracttypeDropdownOptions: DropdownObject[] = [
        {
            label: 'Tijdelijk',
            value: 0
        },
        {
            label: 'Permanent',
            value: 1
        }// TODO: replace to contants file
    ]; // TODO: maybe make dynamic in the future

    const optionsDictionaryWorkingContract: { [key: string]: DropdownObject[] } = {
        "contracttype": contracttypeDropdownOptions
    };

    const addEmergencyPerson = ():EmergencyPerson => {
        const newEmergencyPerson: EmergencyPerson = {
            name: '',
            telephonenumber: ''
        };
        setClient(prevClient => ({
            ...prevClient,
            emergencypeople: [...prevClient.emergencypeople!, newEmergencyPerson]
        }));
        return newEmergencyPerson;
    };

    const handleEmergencyPersonChange = (updatedPerson: EmergencyPerson, index: number) => {
        const updatedClient = { ...client };    
        const updatedEmergencyPeople = [...updatedClient.emergencypeople!];

        updatedEmergencyPeople[index] = updatedPerson;
        updatedClient.emergencypeople = updatedEmergencyPeople;

        setClient(updatedClient);
    };

    const onRemoveEmergencyPerson = (emergencyPerson: EmergencyPerson):void => {
        let emergencyPeople: EmergencyPerson[] = client.emergencypeople ?? [];
        emergencyPeople = emergencyPeople.filter(ep => ep !== emergencyPerson);

        setClient(prevClient => ({
            ...prevClient,
            ["emergencypeople"]: emergencyPeople
        }));
    }

    const addWorkingContract = ():WorkingContract => {
        return {
            companyname: '',
            contracttype: 0,
            function: ''
        };
    };

    const handleWorkingContractChange = (updatedWorkingContract: WorkingContract, index: number) => {
        const updatedClient = { ...client };    
        const updatedWorkingContracts = [...updatedClient.workingcontracts!];
    
        updatedWorkingContracts[index] = updatedWorkingContract;
        updatedClient.workingcontracts = updatedWorkingContracts;
    
        setClient(updatedClient);
    };

    const onRemoveWorkingContract = (workingContract: WorkingContract):void => {
        let workingContracts: WorkingContract[] = client.workingcontracts ?? [];
        workingContracts = workingContracts.filter(wc => wc !== workingContract);

        setClient(prevClient => ({
            ...prevClient,
            ["workingcontracts"]: workingContracts
        }));
    }

    useEffect(() => {
        const loadDiagnoses = async () => { 
            try {
                const apiDiagnoses = await fetchDiagnosis();
                const formattedDiagnoses = apiDiagnoses.map(diagnosis => ({
                    value: diagnosis.id,
                    label: diagnosis.name
                }));
                setDiagnoses(formattedDiagnoses);
            } catch (error: any) {
                setCvsError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens het laden van alle beschikbare diagnoses. Foutmelding: ${error.message}`
                });
                setErrorPopupOpen(true);
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
            } catch (error: any) {
                setCvsError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens het laden van alle beschikbare uitkeringsvormen. Foutmelding: ${error.message}`
                });
                setErrorPopupOpen(true);
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
            } catch (error: any) {
                setCvsError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens het laden van alle beschikbare burgerlijke staat opties. Foutmelding: ${error.message}`
                });
                setErrorPopupOpen(true);
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
            } catch (error: any) {
                setCvsError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens het laden van alle beschikbare rijbewijs codes. Foutmelding: ${error.message}`
                });
                setErrorPopupOpen(true);
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
                                onChange={(value) => handleClientInputChange('firstname', value)} />
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
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={false} 
                                placeholder='b.v. de' 
                                value={client.prefixlastname} 
                                onChange={(value) => handleClientInputChange('prefixlastname', value)} />
                        </LabelField>

                        <LabelField text='Achternaam' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='Achternaam' 
                                value={client.lastname} 
                                onChange={(value) => handleClientInputChange('lastname', value)} />
                        </LabelField>

                        <LabelField text='Straatadres' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='Adres' 
                                value={client.streetname} 
                                onChange={(value) => handleClientInputChange('streetname', value)} />
                        </LabelField>

                        <LabelField text='Huisnummer' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 11' 
                                className='house-number' 
                                value={client.housenumber}
                                onChange={(value) => handleClientInputChange('housenumber', value)} />
                            <LabelField text='Toevoeging' required={false} className='house-number-addition'>
                                <InputField 
                                    inputfieldtype={{type:'text'}} 
                                    required={false} 
                                    placeholder='b.v. A' 
                                    className='house-number-addition-field' 
                                    value={client.housenumberaddition}
                                    onChange={(value) => handleClientInputChange('housenumberaddition', value)} />
                            </LabelField>
                        </LabelField>

                        <LabelField text='Postcode' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 1234 AA' 
                                value={client.postalcode}
                                onChange={(value) => handleClientInputChange('postalcode', value)} />
                        </LabelField>

                        <LabelField text='Woonplaats' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='Woonplaats' 
                                value={client.residence}
                                onChange={(value) => handleClientInputChange('residence', value)} />
                        </LabelField>

                        <LabelField text='Telefoon' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 0543-123456' 
                                value={client.telephonenumber}
                                onChange={(value) => handleClientInputChange('telephonenumber', value)} />
                        </LabelField>

                        <LabelField text='E-mail' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. mail@mailbox.com' 
                                value={client.emailaddress}
                                onChange={(value) => handleClientInputChange('emailaddress', value)} />
                        </LabelField>

                        <LabelField text='Geboortedatum' required={true}>
                            <DatePicker 
                                required={true} 
                                placeholder='Selecteer een datum' 
                                value={client.dateofbirth}
                                onChange={(value) => handleClientDatePickerChange('dateofbirth', value)} />
                        </LabelField>

                        <LabelField text='Geslacht' required={true}>
                            <Dropdown 
                                options={gendersDropdownOptions} 
                                required={true} 
                                inputfieldname='geslacht'
                                value={client.gender}
                                onChange={(value) => handleDropdownChange('gender', value)} />
                        </LabelField>
                    </div>

                    <DomainObjectInput 
                        className='client-domain-object'
                        label='In geval van nood'
                        addObject={addEmergencyPerson} 
                        domainObjects={client.emergencypeople!} 
                        labelType='contactpersoon' 
                        typeName='EmergencyPerson' 
                        numMinimalRequired={1}
                        onRemoveObject={onRemoveEmergencyPerson}
                        onChangeObject={handleEmergencyPersonChange} />

                    <div className='client-remarks'>
                        <Label text='Opmerkingen' />
                        <Textarea 
                            placeholder="Voeg opmerkingen toe" 
                            value={client.remarks}
                            onChange={(value: string) => handleClientInputChange('remarks', value)} />
                    </div>

                    <SlideToggleLabel textColapsed='Klap uit voor meer opties' textExpanded='Klap in' >
                        <div className='client-extra-info'>
                            <LabelField text='Diagnose(s)' required={false}>
                                <DropdownWithButton 
                                    options={diagnoses} 
                                    required={false}
                                    inputfieldname='diagnoses'
                                    value={client.diagnoses?.map(d => d.id)}
                                    onChange={(value) => {handleDropdownWithButtonChange('diagnoses', value)}} />
                            </LabelField>

                            <LabelField text='Uitkeringsvorm' required={false}>
                                <DropdownWithButton 
                                    options={benefitForms} 
                                    required={false} 
                                    inputfieldname='benefitforms'
                                    value={client.benefitforms?.map(d => d.id)}
                                    onChange={(value) => {handleDropdownWithButtonChange('benefitforms', value)}} />
                            </LabelField>

                            <LabelField text='Burgerlijke staat' required={false}>
                                <Dropdown 
                                    options={maritalStatuses} 
                                    required={false} 
                                    inputfieldname='maritalstatus'
                                    value={client.maritalstatus}
                                    onChange={(value) => {handleDropdownChange('maritalstatus', value)}} />
                            </LabelField>

                            <LabelField text='Rijbewijs' required={false}>
                                <DropdownWithButton 
                                    options={driversLicences} 
                                    required={false} 
                                    inputfieldname='driverslicences'
                                    value={client.driverslicences?.map(d => d.id)}
                                    onChange={(value) => {handleDropdownWithButtonChange('driverslicences', value)}} />
                            </LabelField>

                            {/* TODO: doelgroepregister with yes no dropdown
                             <LabelField text='Doelgroepregister' required={false}>
                                <Dropdown 
                                    options={doelgroepDropdownOptions} 
                                    required={false} 
                                    inputfieldname='doelgroepregister'
                                    value={client.doelgroepregister}
                                    onChange={(value) => handleDropdownChange('doelgroepregister', value)} />
                            </LabelField> */}
                        </div>

                        <DomainObjectInput
                            className='client-domain-object'
                            label='Werkervaring' 
                            addObject={addWorkingContract} 
                            domainObjects={workingContracts}
                            fieldOrder={FieldOrderWorkingContract}
                            labelType='werkervaring' 
                            typeName='WorkingContract' 
                            numMinimalRequired={0}
                            onRemoveObject={onRemoveWorkingContract}
                            onChangeObject={handleWorkingContractChange}
                            optionsDictionary={optionsDictionaryWorkingContract} />

                    </SlideToggleLabel>

                    </div>
                <div className='button-container'>
                    <SaveButton
                    buttonText= "Opslaan"
                    loadingText = "Bezig met oplaan"
                    successText = "Cliënt opgeslagen"
                    errorText = "Fout tijdens opslaan"
                    onSave={() => {
                        let result: ApiResult = {
                            Ok: false,
                            Errors: ['Er is iets mis gegaan!'] 
                        }
                        return new Promise<ApiResult>(resolve => setTimeout(() => resolve(result), 2000));
                    }}
                    onResult={(result) => console.error('Result: ', result.Ok)}
                    />

                </div>
            </div>

            <ConfirmPopup
                message={confirmMessage}
                isOpen={isConfirmPopupOneButtonOpen}
                onClose={handlePopUpCancelClick}
                buttons={[{ text: 'Bevestigen', onClick: handlePopUpConfirmClick, buttonType: {type:"Solid"}}]} />

            <ErrorPopup 
                error={cvsError} 
                isOpen={isErrorPopupOpen}
                onClose={() => setErrorPopupOpen(false)} />  
            <Copyright />
        </div>
    );
}

export default ClientCreate;