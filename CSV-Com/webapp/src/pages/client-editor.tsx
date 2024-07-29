import './client-editor.css';
import React, { useEffect, useState, useContext } from "react";
import { useParams, useNavigate, Navigate } from 'react-router-dom';
import Menu from 'components/common/menu';
import { NavTitle } from 'components/nav/nav-title';
import SearchClients from 'components/clients/search-clients';
import { Copyright } from 'components/common/copyright';
import { Header } from 'components/common/header';
import { Label } from 'components/common/label';
import SaveButton from 'components/common/save-button';
import LabelField from 'components/common/label-field';
import { InputField } from 'components/common/input-field';
import { Dropdown, DropdownObject } from 'components/common/dropdown';
import DropdownWithButton from "components/common/dropdown-with-button";
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
import { fetchBenefitForms, fetchDiagnosis, fetchMaritalStatuses, fetchDriversLicences, saveClient, fetchOrganizations } from 'utils/api';
import Client, { getCompleteClientName } from 'types/model/Client';
import { Moment } from 'moment';
import CVSError from 'types/common/cvs-error';
import { FieldOrderWorkingContract } from 'types/common/fieldorder';
import StatusEnum from 'types/common/StatusEnum';
import { fetchClientEditor } from 'utils/api';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faDiagnoses, faSpinner } from "@fortawesome/free-solid-svg-icons";
import ApiResult from 'types/common/api-result';
import DriversLicence from 'types/model/DriversLicence';
import MaritalStatus from 'types/model/MaritalStatus';
import BenefitForm from 'types/model/BenefitForm';
import { ClientContext } from './client-context';
import Organization from 'types/model/Organization';
import { nameof } from 'types/common/nameof';
import DropdownBoolean from 'components/common/dropdown-boolean';

const ClientEditor = () => {
    

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
        isintargetgroupregister: false,
        driverslicences: [],
        emergencypeople: [],
        workingcontracts: [],
        benefitforms: [],
    };

    const clientContext = useContext(ClientContext)
    var { id } = useParams();
    const navigate = useNavigate();
    
    const [client, setClient] = useState<Client>(initialClient);
    const [error, setError] = useState<string | null>(null);
    const [status, setStatus] = useState(StatusEnum.IDLE);

    const [confirmMessage, setConfirmMessage] = useState<string>('');
    const [isConfirmPopupOneButtonOpen, setConfirmPopupOneButtonOpen] = useState<boolean>(false);

    const handlePopUpConfirmClientSavedClick = () => {
        setConfirmPopupOneButtonOpen(false);
        navigate(`/clients/${client.id}`);
    }; // NOTE: this function is now specific for closing the confirm popup after saving a client.

    const [isErrorPopupOpen, setErrorPopupOpen] = useState<boolean>(false);
    const [cvsError, setCvsError] = useState<CvsError>(() => {
        return {
            id: 1,
            errorcode: 'E12345',
            message: "Dit is een foutmelding"
        }
    });

    const [diagnoses, setDiagnoses] = useState<Diagnosis[]>([]);
    const [benefitForms, setBenefitForms] = useState<BenefitForm[]>([]);
    const [maritalStatuses, setMaritalStatuses] = useState<MaritalStatus[]>([]);
    const [driversLicences, setDriversLicences] = useState<DriversLicence[]>([]);
    const [organizations, setOrganizations] = useState<Organization[]>([]);
    
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

    const handleGenderChange = (value: number) => {
        setClient(prevClient => ({
            ...prevClient,
            gender: value
        }));
    };
    
    const handleMaritalStatusChange = (value: number) => {
        let maritalStatusClient: MaritalStatus = maritalStatuses.filter(ms => ms.id === value)[0];

        setClient(prevClient => ({
            ...prevClient,
            maritalstatus: maritalStatusClient
        }));
    };

    const handleBenefitFormsChange = (values: number[]) => {
        let benefitFormsClient = benefitForms.filter(bf => values.includes(bf.id));

        setClient(prevClient => ({
            ...prevClient,
            benefitforms: benefitFormsClient
        }));
    };

    const handleDiagnosesChange = (values: number[]) => {
        let diagnosesClient = diagnoses.filter(d => values.includes(d.id));

        setClient(prevClient => ({
            ...prevClient,
            diagnoses: diagnosesClient
        }));
    };

    const handleDriversLicensesChange = (values: number[]) => {
        let driversLicencesClient = driversLicences.filter(dl => values.includes(dl.id));

        setClient(prevClient => ({
            ...prevClient,
            driverslicences: driversLicencesClient
        }));
    };

    const handleIsInTargetGroupRegisterChange = (value: boolean) => {
        setClient(prevClient => ({
            ...prevClient,
            isintargetgroupregister: value
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
        "contracttype": contracttypeDropdownOptions,
        "organizationid": organizations.map(organization => ({
            label: organization.organizationname,
            value: organization.id
        }))
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
            organizationid: 0,
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
        const fetchClientById = async () => {
            try {
              setStatus(StatusEnum.PENDING);
              const fetchedClient: Client = await fetchClientEditor(id!);

              setStatus(StatusEnum.SUCCESSFUL);
    
              setClient(fetchedClient);
            } catch (e) {
              // TODO: error handling
              console.error(e);
              setStatus(StatusEnum.REJECTED);
              //setError(e);
            }
          };

        const loadDiagnoses = async () => { 
            try {
                setDiagnoses(await fetchDiagnosis());
            } catch (error: any) {
                setCvsError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens het laden van alle beschikbare diagnoses. Foutmelding: ${error.message}`
                });
                setErrorPopupOpen(true);
            }
        };

        const loadBenefitForms = async () => { 
            try {
                setBenefitForms(await fetchBenefitForms());
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
                setMaritalStatuses(await fetchMaritalStatuses());
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
                setDriversLicences(await fetchDriversLicences());
            } catch (error: any) {
                setCvsError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens het laden van alle beschikbare rijbewijs codes. Foutmelding: ${error.message}`
                });
                setErrorPopupOpen(true);
            }
        };

        const loadOrganizations = async () => {
            try {

                var orgs = await fetchOrganizations();
                setOrganizations(orgs);
            } catch (error: any) {
                setCvsError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens het laden van alle beschikbare organisaties. Foutmelding: ${error.message}`
                });
                setErrorPopupOpen(true);
            }
        }

        if(id && status !== StatusEnum.PENDING) {
            fetchClientById();
        }

        loadDiagnoses();
        loadBenefitForms();
        loadMaritalStatuses();
        loadDriversLicences();
        loadOrganizations();
    }, [id]);

    return(
        <>
        <div className={`loading-spinner ${showLoadingScreen(status)}`}>
            <FontAwesomeIcon icon={faSpinner} className="fa fa-2x fa-refresh fa-spin" />
        </div>

        <div className="flex flex-col lg:flex-row h-screen lg:h-auto">
            <div className='lg:flex w-full'>
                <div id='staticSidebar' className='sidebarContentPush'></div>
                
                <Menu>
                    <NavTitle lijstNaam="Cliënten" />
                    <SearchClients />
                </Menu>

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
                                onChange={(value) => handleClientInputChange('firstname', value)}
                                dataTestId='firstname' />
                        </LabelField>

                        <LabelField text='Voorletters' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. A B' 
                                value={client.initials}
                                onChange={(value) => handleClientInputChange('initials', value)}
                                dataTestId='initials' />
                        </LabelField>

                        <LabelField text='Tussenvoegsel' required={false}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={false} 
                                placeholder='b.v. de' 
                                value={client.prefixlastname} 
                                onChange={(value) => handleClientInputChange('prefixlastname', value)}
                                dataTestId='prefixlastname' />
                        </LabelField>

                        <LabelField text='Achternaam' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='Achternaam' 
                                value={client.lastname} 
                                onChange={(value) => handleClientInputChange('lastname', value)}
                                dataTestId='lastname' />
                        </LabelField>

                        <LabelField text='Straatadres' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='Adres' 
                                value={client.streetname} 
                                onChange={(value) => handleClientInputChange('streetname', value)}
                                dataTestId='streetname' />
                        </LabelField>

                        <LabelField text='Huisnummer' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 11' 
                                className='house-number' 
                                value={client.housenumber}
                                onChange={(value) => handleClientInputChange('housenumber', value)}
                                dataTestId='housenumber' />
                            <LabelField text='Toevoeging' required={false} className='house-number-addition'>
                                <InputField 
                                    inputfieldtype={{type:'text'}} 
                                    required={false} 
                                    placeholder='b.v. A' 
                                    className='house-number-addition-field' 
                                    value={client.housenumberaddition}
                                    onChange={(value) => handleClientInputChange('housenumberaddition', value)}
                                    dataTestId='housenumberaddition' />
                            </LabelField>
                        </LabelField>

                        <LabelField text='Postcode' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 1234 AA' 
                                value={client.postalcode}
                                onChange={(value) => handleClientInputChange('postalcode', value)}
                                dataTestId='postalcode' />
                        </LabelField>

                        <LabelField text='Woonplaats' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='Woonplaats' 
                                value={client.residence}
                                onChange={(value) => handleClientInputChange('residence', value)}
                                dataTestId='residence' />
                        </LabelField>

                        <LabelField text='Telefoon' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 0543-123456' 
                                value={client.telephonenumber}
                                onChange={(value) => handleClientInputChange('telephonenumber', value)}
                                dataTestId='telephonenumber' />
                        </LabelField>

                        <LabelField text='E-mail' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. mail@mailbox.com' 
                                value={client.emailaddress}
                                onChange={(value) => handleClientInputChange('emailaddress', value)}
                                dataTestId='emailaddress' />
                        </LabelField>

                        <LabelField text='Geboortedatum' required={true}>
                            <DatePicker 
                                required={true} 
                                placeholder='Selecteer een datum' 
                                value={client.dateofbirth}
                                onChange={(value) => handleClientDatePickerChange('dateofbirth', value)}
                                dataTestId='dateofbirth' />
                        </LabelField>

                        <LabelField text='Geslacht' required={true}>
                            <Dropdown 
                                options={gendersDropdownOptions} 
                                required={true} 
                                inputfieldname='geslacht'
                                value={client.gender}
                                onChange={(value) => handleGenderChange(value)}
                                dataTestId='gender' />
                        </LabelField>
                    </div>

                    <DomainObjectInput 
                        className='client-domain-object'
                        label='In geval van nood'
                        addObject={addEmergencyPerson} 
                        value={client.emergencypeople!} 
                        labelType='contactpersoon' 
                        typeName='EmergencyPerson' 
                        numMinimalRequired={1}
                        onRemoveObject={onRemoveEmergencyPerson}
                        onChangeObject={handleEmergencyPersonChange}
                        key={JSON.stringify(client.id + "_emergencypeople")}
                        dataTestId='emergencypeople' />

                    <div className='client-remarks'>
                        <Label text='Opmerkingen' />
                        <Textarea 
                            placeholder="Voeg opmerkingen toe" 
                            value={client.remarks}
                            onChange={(value: string) => handleClientInputChange('remarks', value)}
                            dataTestId='remarks' />
                    </div>

                    <SlideToggleLabel text='Overige cliënt informatie' smallTextColapsed=' - klap uit voor meer opties' smallTextExpanded=' - klap in voor minder opties' >
                        <div className='client-extra-info'>
                            <LabelField text='Diagnose(s)' required={false}>
                                <DropdownWithButton 
                                    options={
                                        diagnoses.map(diagnosis => ({
                                            value: diagnosis.id,
                                            label: diagnosis.name
                                        }))
                                    } 
                                    required={false}
                                    inputfieldname='diagnoses'
                                    value={client.diagnoses?.map(d => d.id)}
                                    onChange={(values) => {handleDiagnosesChange(values)}}
                                    key={JSON.stringify(client.id + "_diagnoses")}
                                    dataTestId='diagnoses' />
                            </LabelField>

                            <LabelField text='Uitkeringsvorm' required={false}>
                                <DropdownWithButton 
                                    options={
                                        benefitForms.map(benefitForm => ({
                                            value: benefitForm.id,
                                            label: benefitForm.name
                                        }))
                                    } 
                                    required={false} 
                                    inputfieldname='benefitforms'
                                    value={client.benefitforms?.map(d => d.id)}
                                    onChange={(values) => {handleBenefitFormsChange(values)}}
                                    key={JSON.stringify(client.id + "_benefitforms")}
                                    dataTestId='benefitforms' />
                            </LabelField>

                            <LabelField text='Burgerlijke staat' required={false}>
                                <Dropdown 
                                    options={
                                        maritalStatuses.map(maritalStatus => ({
                                            value: maritalStatus.id,
                                            label: maritalStatus.name
                                        }))
                                    } 
                                    required={false} 
                                    inputfieldname='maritalstatus'
                                    value={client.maritalstatus?.id}
                                    onChange={(value) => {handleMaritalStatusChange(value)}}
                                    dataTestId='maritalstatus' />
                            </LabelField>

                            <LabelField text='Rijbewijs' required={false}>
                                <DropdownWithButton 
                                    options={
                                        driversLicences.map(driverLicence => ({
                                            value: driverLicence.id,
                                            label: `${driverLicence.category} (${driverLicence.description})`
                                        }))
                                    } 
                                    required={false} 
                                    inputfieldname='driverslicences'
                                    value={client.driverslicences?.map(d => d.id)}
                                    onChange={(values) => {handleDriversLicensesChange(values)}}
                                    key={JSON.stringify(client.id + "_driverslicences")}
                                    dataTestId='driverslicences' />
                            </LabelField>

                            <LabelField text='Doelgroepregister' required={false}>
                                <DropdownBoolean 
                                    required={false} 
                                    inputfieldname='doelgroepregister'
                                    value={client.isintargetgroupregister}
                                    onChange={(value) => handleIsInTargetGroupRegisterChange(value)} />
                            </LabelField>
                        </div>

                        <DomainObjectInput
                            className='client-domain-object'
                            label='Werkervaring' 
                            addObject={addWorkingContract} 
                            value={client.workingcontracts!}
                            fieldOrder={FieldOrderWorkingContract}
                            labelType='werkervaring' 
                            typeName='WorkingContract' 
                            numMinimalRequired={0}
                            onRemoveObject={onRemoveWorkingContract}
                            onChangeObject={handleWorkingContractChange}
                            optionsDictionary={optionsDictionaryWorkingContract}
                            key={JSON.stringify(client.id + "_workingcontracts")}
                            dataTestId='workingcontracts' />

                    </SlideToggleLabel>

                    <div className='button-container'>
                        <SaveButton
                            buttonText= "Opslaan"
                            loadingText = "Bezig met oplaan"
                            successText = "Cliënt opgeslagen"
                            errorText = "Fout tijdens opslaan"
                            onSave={async () => await saveClient(client!)}                            
                            onResult={(apiResult) => handleSaveResult(apiResult, setConfirmMessage, setConfirmPopupOneButtonOpen, setCvsError, setErrorPopupOpen, setClient)}
                            dataTestId='button.save' />
                    </div>
                </div>
            </div>

            <ConfirmPopup
                data-testid='confirm-popup'
                message={confirmMessage}
                isOpen={isConfirmPopupOneButtonOpen}
                onClose={handlePopUpConfirmClientSavedClick}
                buttons={[{ text: 'Bevestigen', dataTestId: 'button.confirm', onClick: handlePopUpConfirmClientSavedClick, buttonType: {type:"Solid"}}]} />

            <ErrorPopup 
                error={cvsError} 
                isOpen={isErrorPopupOpen}
                onClose={() => setErrorPopupOpen(false)} />  
            <Copyright />
        </div>
        </>
    );
}

export default ClientEditor;

function handleSaveResult(
    apiResult: ApiResult<Client>, 
    setConfirmMessage: React.Dispatch<React.SetStateAction<string>>, 
    setConfirmPopupOneButtonOpen: React.Dispatch<React.SetStateAction<boolean>>, 
    setCvsError: React.Dispatch<React.SetStateAction<CvsError>>, 
    setErrorPopupOpen: React.Dispatch<React.SetStateAction<boolean>>, 
    setClient: React.Dispatch<React.SetStateAction<Client>>) {
    if (apiResult.Ok) {
        setConfirmMessage('Client succesvol opgeslagen');
        setConfirmPopupOneButtonOpen(true);

        setClient(apiResult.ReturnObject!);
    }
    else {
        setCvsError({
            id: 0,
            errorcode: 'E',
            message: `Er is een opgetreden tijdens het opslaan van een client. Foutmelding: ${apiResult.Errors!.join(', ')}`
        });
        setErrorPopupOpen(true);
    }
}

function showLoadingScreen(status: string): string | undefined {
    return ` ${status === StatusEnum.PENDING ? 'loading-spinner-visible' : 'loading-spinner-hidden'}`;
}
