import './coachingprogram-editor.css';
import React, { useEffect, useState, useContext } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome"
import SearchClients from "components/clients/search-clients"
import { Header } from "components/common/header"
import Menu from "components/common/menu"
import { NavTitle } from "components/nav/nav-title"
import StatusEnum from "types/common/StatusEnum"
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import { useParams, useNavigate } from 'react-router-dom';
import { ClientContext } from './client-context';
import Client from "types/model/Client";
import { fetchClientFullname, fetchCoachingProgramEdit, fetchCoachingProgramTypes, fetchOrganizations, saveCoachingProgram } from "utils/api";
import { Copyright } from "components/common/copyright";
import ErrorPopup from "components/common/error-popup";
import CvsError from 'types/common/cvs-error';
import ClientQuery from "types/model/ClientQuery";
import { error } from "console";
import LabelField from 'components/common/label-field';
import { InputField } from 'components/common/input-field';
import { Label } from 'components/common/label';
import { Dropdown, DropdownObject } from 'components/common/dropdown';
import Organization from 'types/model/Organization';
import { DatePicker } from 'components/common/datepicker';
import { filterValidationErrors, ValidationErrorHash } from 'types/common/validation-error';
import CoachingProgramEdit from 'types/model/CoachingProgramEdit';
import Decimal from 'decimal.js-light';
import { DecimalInputField } from 'components/common/decimal-input-field';
import { Moment } from 'moment';
import SaveButton from 'components/common/save-button';
import ApiResult from 'types/common/api-result';
import ConfirmPopup from 'components/common/confirm-popup';
import GetClientFullnameDto from 'types/model/GetClientFullnameDto';
import GetCoachingProgramTypesDto from 'types/model/GetCoachingProgramTypesDto';

const CoachingProgramEditor = () => {
    var { clientid, id } = useParams();

    const navigate = useNavigate();

    const initialCoachingProgram: CoachingProgramEdit = new CoachingProgramEdit(
        0,
        parseInt(clientid!, 10),
        "",
        0,
        new Decimal(0),
        "",
        0,
        undefined,
        undefined,
        new Decimal(0)
    );

    const [coachingProgram, setCoachingProgram] = useState<CoachingProgramEdit>(initialCoachingProgram);

    const [status, setStatus] = useState(StatusEnum.IDLE);        
    const [client, setClient] = useState<GetClientFullnameDto | null>(null);    
    const [organizations, setOrganizations] = useState<Organization[]>([]);
    const [coachingProgramTypes, setTrajectTypes] = useState<GetCoachingProgramTypesDto[]>([]);

    const [validationErrors, setValidationErrors] = useState<ValidationErrorHash>({});
    const [error, setError] = useState<CvsError>({
        id: 1,
        errorcode: 'E12345',
        message: "Dit is een foutmelding"
    });
    const [isErrorPopupOpen, setErrorPopupOpen] = useState<boolean>(false);    

    const [confirmMessage, setConfirmMessage] = useState<string>('');
    const [isConfirmPopupOneButtonOpen, setConfirmPopupOneButtonOpen] = useState<boolean>(false);

    const handlePopUpConfirmSavedClick = () => {
        setConfirmPopupOneButtonOpen(false);
        navigate(`/clients/${clientid}`);
    };

    useEffect(() => {
        const fetchClientById = async () => {
            try {
                if(clientid === null){
                    throw new Error(`Client id is not specified.`);
                }

                setStatus(StatusEnum.PENDING);
                const clientFullname: GetClientFullnameDto = await fetchClientFullname(clientid!);

                if(clientFullname === null){
                    throw new Error(`Client with id ${clientid} was not found.`);
                }

                coachingProgram.clientid = parseInt(clientid!, 10);

                setStatus(StatusEnum.SUCCESSFUL);
        
                setClient(clientFullname);
            } catch (e: any) {
              // TODO: error handling
              console.error(e);
              setStatus(StatusEnum.REJECTED);

              setError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens het ophalen van de cliënt informatie. Foutmelding: ${e.message}.}`
                });

                setErrorPopupOpen(true);
            }
          };       

        if(clientid && status !== StatusEnum.PENDING) {
            fetchClientById();
        }

        const loadOrganizations = async () => {
            try {
    
                var orgs = await fetchOrganizations();
                setOrganizations(orgs);
            } catch (error: any) {
                setError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens het laden van alle beschikbare organisaties. Foutmelding: ${error.message}`
                });
                setErrorPopupOpen(true);
            }
        }

        const loadCoachingProgramTypes = async () => {
            try {
    
                var types = await fetchCoachingProgramTypes();
                setTrajectTypes(types);
            } catch (error: any) {
                setError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens het laden van alle beschikbare trajecttypes. Foutmelding: ${error.message}`
                });
                setErrorPopupOpen(true);
            }
        }        
    
        loadOrganizations();
        loadCoachingProgramTypes();
    }, [clientid]);

    useEffect(() => {
        if(id === undefined)
            return;

        const fetchCoachingProgramEditById = async () => {
            try {
                if(id === null){
                    throw new Error(`Id is not specified.`);
                }

                setStatus(StatusEnum.PENDING);
                const coachingProgramEdit: CoachingProgramEdit = await fetchCoachingProgramEdit(id!);
                setStatus(StatusEnum.SUCCESSFUL);
        
                setCoachingProgram(coachingProgramEdit);
            } catch (e: any) {
              // TODO: error handling
              console.error(e);
              setStatus(StatusEnum.REJECTED);

              setError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens het ophalen van de traject informatie. Foutmelding: ${e.message}.}`
                });

                setErrorPopupOpen(true);
            }
          };       

          fetchCoachingProgramEditById();
    }, [id]);

    const handleInputChange = (fieldName: keyof CoachingProgramEdit, value: any) => {
        setCoachingProgram(prevCoachingProgram => {            
            const updatedCoachingProgram = new CoachingProgramEdit(
                prevCoachingProgram.id,
                prevCoachingProgram.clientid,
                prevCoachingProgram.title,
                prevCoachingProgram.coachingprogramtype,                                              
                prevCoachingProgram.hourlyrate,
                prevCoachingProgram.ordernumber,
                prevCoachingProgram.organizationid,
                prevCoachingProgram.begindate,
                prevCoachingProgram.enddate,  
                prevCoachingProgram.budgetammount
            );
    
            updatedCoachingProgram.updateField(fieldName, value);
            return updatedCoachingProgram;
        });
    };

    const handleSaveResult = (
        apiResult: ApiResult<CoachingProgramEdit>, 
        setConfirmMessage: React.Dispatch<React.SetStateAction<string>>, 
        setConfirmPopupOneButtonOpen: React.Dispatch<React.SetStateAction<boolean>>, 
        setCvsError: React.Dispatch<React.SetStateAction<CvsError>>, 
        setErrorPopupOpen: React.Dispatch<React.SetStateAction<boolean>>, 
        setCoachingProgram: React.Dispatch<React.SetStateAction<CoachingProgramEdit>>): void => {
        if (apiResult.Ok) {
            setConfirmMessage('Client succesvol opgeslagen');
            setConfirmPopupOneButtonOpen(true);
    
            setCoachingProgram(apiResult.ReturnObject!);
        }
        else {
            if(apiResult.ValidationErrors) {
                setValidationErrors(apiResult.ValidationErrors);
            }
            else {
                setCvsError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens het opslaan van een traject. Foutmelding: ${(apiResult.Errors as string[]).join(', ')}`
                });
                
                setErrorPopupOpen(true);    
            }
        }
    }

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

            <div className="coachingprogram-create-container">
                <div className='coachingprogram-create-header'>
                    <Header text="Traject aanmaken" className='coachingprogram-create-header-main' />
                    <p className='coachingprogram-create-header-sub'> - Velden met * zijn verplicht</p>
                </div>

                <div className='coachingprogram-create-fields'>
                    <LabelField text='Cliënt' required={false}>
                        <Label 
                            className='client-fullname' 
                            text={client !== null ? client.clientfullname : ''}
                            errors={validationErrors.clientid}
                            dataTestId='client-fullname' />
                    </LabelField>
                </div>

                <div className='coachingprogram-create-fields'>
                    <LabelField text='Traject titel' required={true}>
                        <InputField 
                            inputfieldtype={{type:'text'}} 
                            required={true} 
                            placeholder='Titel' 
                            value={coachingProgram.title} 
                            onChange={(value) => handleInputChange('title', value)}
                            dataTestId='title'
                            errors={validationErrors.title} />
                    </LabelField>

                    <LabelField text='Ordernummer' required={false}>
                        <InputField 
                            inputfieldtype={{type:'text'}} 
                            required={false} 
                            placeholder='Ordernummer' 
                            value={coachingProgram.ordernumber} 
                            onChange={(value) => handleInputChange('ordernumber', value)}
                            dataTestId='ordernumber'
                            errors={validationErrors.lastname} />
                    </LabelField>

                    <LabelField text='Opdrachtgever' required={false}>
                        <Dropdown 
                            options={
                                organizations.map(organization => ({
                                    value: organization.id,
                                    label: organization.organizationname
                                }))
                            } 
                            required={false} 
                            inputfieldname='organization'
                            value={coachingProgram.organizationid}
                            onChange={(value) => {handleInputChange('organizationid', value)}}
                            dataTestId='organization'
                            errors={validationErrors.organizationid} />
                    </LabelField>

                    <LabelField text='Traject type' required={true}>
                        <Dropdown 
                            options={
                                coachingProgramTypes.map(coachingProgramType => ({
                                    value: coachingProgramType.id,
                                    label: coachingProgramType.name
                                }))
                            } 
                            required={true} 
                            inputfieldname='coachingprogramtype'
                            value={coachingProgram.coachingprogramtype}
                            onChange={(value) => {handleInputChange('coachingprogramtype', value)}}
                            dataTestId='coachingprogramtype'
                            errors={validationErrors.coachingprogramtype} />
                    </LabelField>

                    <LabelField text='Aanvangsdatum' required={true}>
                        <DatePicker 
                            required={true} 
                            placeholder='Selecteer een datum' 
                            value={coachingProgram.begindate}
                            onChange={(value) => handleInputChange('begindate', value)}
                            dataTestId='begindate'
                            errors={validationErrors.begindate} />
                    </LabelField>

                    <LabelField text='Einddatum' required={true}>
                        <DatePicker 
                            required={true} 
                            placeholder='Selecteer een datum' 
                            value={coachingProgram.enddate}
                            onChange={(value) => handleInputChange('enddate', value)}
                            dataTestId='enddate'
                            errors={validationErrors.enddate} />
                    </LabelField>

                    <LabelField text='Budgetbedrag' required={false}>
                        <DecimalInputField 
                            inputfieldtype={{type:'text'}} 
                            required={false} 
                            placeholder='Budgetbedrag' 
                            value={coachingProgram.budgetammount?.toString()} 
                            onChange={(value) => handleInputChange('budgetammount', value)}
                            dataTestId='budgetammount'
                            errors={validationErrors.budgetammount} />
                    </LabelField>

                    <LabelField text='Uurtarief' required={true}>
                        <DecimalInputField 
                            inputfieldtype={{type:'text'}} 
                            required={true} 
                            placeholder='Uurtarief' 
                            value={coachingProgram.hourlyrate.toString()} 
                            onChange={(value) => handleInputChange('hourlyrate', value)}
                            dataTestId='hourlyrate'
                            errors={validationErrors.hourlyrate} />
                    </LabelField>

                    <LabelField text='Te besteden uren' required={false}>
                        <DecimalInputField 
                            inputfieldtype={{type:'text'}} 
                            required={false} 
                            placeholder='Te besteden uren' 
                            value={coachingProgram.remaininghours.toString().replace('.', ',')}
                            dataTestId='remaininghours'
                            readOnly={true} />
                    </LabelField>
                </div>

                <div className='button-container'>
                    <SaveButton
                        buttonText= "Opslaan"
                        loadingText = "Bezig met oplaan"
                        successText = "Traject opgeslagen"
                        errorText = "Fout tijdens opslaan"
                        onSave={async () => await saveCoachingProgram(coachingProgram)}
                        onResult={(apiResult) => handleSaveResult(apiResult, setConfirmMessage, setConfirmPopupOneButtonOpen, setError, setErrorPopupOpen, setCoachingProgram)}
                        dataTestId='button.save' />
                </div>

            </div>
        </div>

        <ConfirmPopup
                data-testid='confirm-popup'
                message={confirmMessage}
                isOpen={isConfirmPopupOneButtonOpen}
                onClose={handlePopUpConfirmSavedClick}
                buttons={[{ text: 'Bevestigen', dataTestId: 'button.confirm', onClick: handlePopUpConfirmSavedClick, buttonType: {type:"Solid"}}]} />

        <ErrorPopup 
                error={error} 
                isOpen={isErrorPopupOpen}
                onClose={() => setErrorPopupOpen(false)} />  
        <Copyright />
    </div>
    </>
    );
}

export default CoachingProgramEditor;

function showLoadingScreen(status: string): string | undefined {
    return ` ${status === StatusEnum.PENDING ? 'loading-spinner-visible' : 'loading-spinner-hidden'}`;
}
