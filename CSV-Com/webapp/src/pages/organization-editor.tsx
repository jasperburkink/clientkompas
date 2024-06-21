import './organization-editor.css';
import React, { useEffect, useState, useContext } from "react";
import { useNavigate, useParams } from 'react-router-dom';
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
import ConfirmPopup from "components/common/confirm-popup";
import ErrorPopup from 'components/common/error-popup';
import { fetchOrganizationEditor, saveClient, saveOrganization } from 'utils/api';
import StatusEnum from 'types/common/StatusEnum';
import CVSError from 'types/common/cvs-error';
import { fetchClientEditor } from 'utils/api';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faDiagnoses, faSpinner } from "@fortawesome/free-solid-svg-icons";
import ApiResult from 'types/common/api-result';
import { OrganizationContext } from './organization-context';
import Organization from 'types/model/Organization';

const OrganizationEditor = () => {
    const initialOrganization: Organization = { 
        id: 0,
        organizationname: '',
        visitstreetname: '',
        visithousenumber: '',
        visitpostalcode: '',
        visitresidence: '',
        invoicestreetname: '',
        invoicehousenumber: '',
        invoicepostalcode: '',
        invoiceresidence: '',
        poststreetname: '',
        posthousenumber: '',
        postpostalcode: '',
        postresidence: '',
        contactpersonname: '',
        contactpersonfunction: '',
        contactpersontelephonenumber: '',
        contactpersonmobilephonenumber: '',
        contactpersonemailaddress: '',
        phonenumber: '',
        website: '',
        emailaddress: '',
        kvknumber: '',
        btwnumber: '',
        ibannumber: '',
        bic: '',
    };

    const organizationContext = useContext(OrganizationContext)
    var { id } = useParams();
    const navigate = useNavigate();

    const [organization, setOrganization] = useState<Organization>(initialOrganization);
    const [error, setError] = useState<string | null>(null);
    const [status, setStatus] = useState(StatusEnum.IDLE);

    const [confirmMessage, setConfirmMessage] = useState<string>('');
    const [isConfirmPopupOneButtonOpen, setConfirmPopupOneButtonOpen] = useState<boolean>(false);

    const handlePopUpConfirmOrganizationSavedClick = () => {
        setConfirmPopupOneButtonOpen(false);
        navigate(`/organization/${organization.id}`);
    }; // NOTE: this function is now specific for closing the confirm popup after saving an organization.

    const [isErrorPopupOpen, setErrorPopupOpen] = useState<boolean>(false);
    const [cvsError, setCvsError] = useState<CVSError>(() => {
        return {
            id: 1,
            errorcode: 'E12345',
            message: "Dit is een foutmelding"
        }
    });

    const handleOrganizationInputChange = (fieldName: string, value: string) => {
        setOrganization(prevOrganization => ({
            ...prevOrganization,
            [fieldName]: value
        }));
    };

    useEffect(() => {
        const fetchOrganizationById = async () => {
            try {
              setStatus(StatusEnum.PENDING);
              const fetchedOrganization: Organization = await fetchOrganizationEditor(id!);

              setStatus(StatusEnum.SUCCESSFUL);
    
              setOrganization(fetchedOrganization);
            } catch (e) {
              // TODO: error handling
              console.error(e);
              setStatus(StatusEnum.REJECTED);
              //setError(e);
            }
          };
    }, [id]);

    return(
        <>
        <div className={`loading-spinner ${showLoadingScreen(status)}`}>
            <FontAwesomeIcon icon={faSpinner} className="fa fa-2x fa-refresh fa-spin" />
        </div>
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
                <div className="organization-create-container">
                    <div className='header'>
                        <Header text="Organisatie aanmaken" />
                    </div>

                    <div className='organization-create-fields'>
                        <LabelField text='Naam' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='Naam' 
                                value={organization.organizationname} 
                                onChange={(value) => handleOrganizationInputChange('organizationname', value)} />
                        </LabelField>

                        <LabelField text='Telefoon' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 0543123456' 
                                value={organization.phonenumber}
                                onChange={(value) => handleOrganizationInputChange('phonenumber', value)} />
                        </LabelField>

                        <LabelField text='Website' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. www.website.nl' 
                                value={organization.website}
                                onChange={(value) => handleOrganizationInputChange('website', value)} />
                        </LabelField>

                        <LabelField text='Email' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. mail@mail.com' 
                                value={organization.emailaddress}
                                onChange={(value) => handleOrganizationInputChange('emailaddress', value)} />
                        </LabelField>

                        <div className='float-left'>
                        <Label text='Bezoekadres' strong={true} className='organization-create-subheader' />

                        <LabelField text='Bezoekadres' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. adres' 
                                value={organization.visitstreetname}
                                onChange={(value) => handleOrganizationInputChange('visitstreetname', value)} />
                        </LabelField>

                        <LabelField text='Huisnummer' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 11' 
                                className='house-number' 
                                value={organization.visithousenumber}
                                onChange={(value) => handleOrganizationInputChange('visithousenumber', value)} />

                            <LabelField text='Toevoeging' required={false} className='house-number-addition'>
                                <InputField 
                                    inputfieldtype={{type:'text'}} 
                                    required={false} 
                                    placeholder='b.v. A' 
                                    className='house-number-addition-field' 
                                    value={organization.visithousenumberaddition}
                                    onChange={(value) => handleOrganizationInputChange('visithousenumberaddition', value)} />
                            </LabelField>
                        </LabelField>

                        <LabelField text='Postcode' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 1234AA' 
                                className='house-number' 
                                value={organization.visitpostalcode}
                                onChange={(value) => handleOrganizationInputChange('visitpostalcode', value)} />

                            <LabelField text='Plaats' required={true} className='house-number-addition'>
                                <InputField 
                                    inputfieldtype={{type:'text'}} 
                                    required={false} 
                                    placeholder='b.v. Arnhem' 
                                    className='house-number-addition-field' 
                                    value={organization.visitresidence}
                                    onChange={(value) => handleOrganizationInputChange('visitresidence', value)} />
                            </LabelField>
                        </LabelField>
                        </div>

                        <div className='float-right'>
                        <Label text='Factuuradres' strong={true} className='organization-create-subheader' />
                        <LabelField text='Factuuradres' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. adres' 
                                value={organization.invoicestreetname}
                                onChange={(value) => handleOrganizationInputChange('invoicestreetname', value)} />
                        </LabelField>

                        <LabelField text='Huisnummer' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 11' 
                                className='house-number' 
                                value={organization.invoicehousenumber}
                                onChange={(value) => handleOrganizationInputChange('invoicehousenumber', value)} />

                            <LabelField text='Toevoeging' required={false} className='house-number-addition'>
                                <InputField 
                                    inputfieldtype={{type:'text'}} 
                                    required={false} 
                                    placeholder='b.v. A' 
                                    className='house-number-addition-field' 
                                    value={organization.invoicehousenumberaddition}
                                    onChange={(value) => handleOrganizationInputChange('invoicehousenumberaddition', value)} />
                            </LabelField>
                        </LabelField>

                        <LabelField text='Postcode' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 1234AA' 
                                className='house-number' 
                                value={organization.invoicepostalcode}
                                onChange={(value) => handleOrganizationInputChange('invoicepostalcode', value)} />

                            <LabelField text='Plaats' required={true} className='house-number-addition'>
                                <InputField 
                                    inputfieldtype={{type:'text'}} 
                                    required={false} 
                                    placeholder='b.v. Arnhem' 
                                    className='house-number-addition-field' 
                                    value={organization.invoiceresidence}
                                    onChange={(value) => handleOrganizationInputChange('invoiceresidence', value)} />
                            </LabelField>
                        </LabelField>
                        </div>

                        <Label text='Postadres' strong={true} className='organization-create-subheader' />
                        <div className='float-left'>
                        <LabelField text='Postadres' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. adres' 
                                value={organization.poststreetname}
                                onChange={(value) => handleOrganizationInputChange('poststreetname', value)} />
                        </LabelField>

                        <LabelField text='Huisnummer' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 11' 
                                className='house-number' 
                                value={organization.posthousenumber}
                                onChange={(value) => handleOrganizationInputChange('posthousenumber', value)} />

                            <LabelField text='Toevoeging' required={false} className='house-number-addition'>
                                <InputField 
                                    inputfieldtype={{type:'text'}} 
                                    required={false} 
                                    placeholder='b.v. A' 
                                    className='house-number-addition-field' 
                                    value={organization.posthousenumberaddition}
                                    onChange={(value) => handleOrganizationInputChange('posthousenumberaddition', value)} />
                            </LabelField>
                        </LabelField>

                        <LabelField text='Postcode' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 1234AA' 
                                className='house-number' 
                                value={organization.postpostalcode}
                                onChange={(value) => handleOrganizationInputChange('postpostalcode', value)} />

                            <LabelField text='Plaats' required={true} className='house-number-addition'>
                                <InputField 
                                    inputfieldtype={{type:'text'}} 
                                    required={false} 
                                    placeholder='b.v. Arnhem' 
                                    className='house-number-addition-field' 
                                    value={organization.postresidence}
                                    onChange={(value) => handleOrganizationInputChange('postresidence', value)} />
                            </LabelField>
                        </LabelField>
                        </div>

                        <Label text='Contactpersoon' strong={true} className='organization-create-subheader' />
                        <LabelField text='Naam' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='Naam' 
                                value={organization.contactpersonname} 
                                onChange={(value) => handleOrganizationInputChange('contactpersonname', value)} />
                        </LabelField>

                        <LabelField text='Functie' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. Eigenaar' 
                                value={organization.contactpersonfunction}
                                onChange={(value) => handleOrganizationInputChange('contactpersonfunction', value)} />
                        </LabelField>

                        <LabelField text='Telefoon' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 0543123456' 
                                value={organization.contactpersontelephonenumber}
                                onChange={(value) => handleOrganizationInputChange('contactpersontelephonenumber', value)} />
                        </LabelField>

                        <LabelField text='Mobiel' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 0612345678' 
                                value={organization.contactpersonmobilephonenumber}
                                onChange={(value) => handleOrganizationInputChange('contactpersonmobilephonenumber', value)} />
                        </LabelField>

                        <LabelField text='Email' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. mail@mail.com' 
                                value={organization.contactpersonemailaddress}
                                onChange={(value) => handleOrganizationInputChange('contactpersonemailaddress', value)} />
                        </LabelField>

                        <Label text='Overige informatie' strong={true} className='organization-create-subheader' />
                        <LabelField text='KVK nummer' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 01223340' 
                                value={organization.kvknumber} 
                                onChange={(value) => handleOrganizationInputChange('kvknumber', value)} />
                        </LabelField>

                        <LabelField text='BTW nummer' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. 003456789B00' 
                                value={organization.btwnumber}
                                onChange={(value) => handleOrganizationInputChange('btwnumber', value)} />
                        </LabelField>

                        <LabelField text='IBAN' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. NL10BBAA0987654321' 
                                value={organization.ibannumber}
                                onChange={(value) => handleOrganizationInputChange('ibannumber', value)} />
                        </LabelField>

                        <LabelField text='BIC' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='b.v. Ladder3A' 
                                value={organization.bic}
                                onChange={(value) => handleOrganizationInputChange('bic', value)} />
                        </LabelField>
                    </div>
                    <div className='button-container'>
                        <SaveButton
                        buttonText= "Opslaan"
                        loadingText = "Bezig met oplaan"
                        successText = "Cliënt opgeslagen"
                        errorText = "Fout tijdens opslaan"
                        onSave={async () => {                                 
                                return await saveOrganization(organization!)
                            }
                        }
                        onResult={(apiResult) => handleSaveResult(apiResult, setConfirmMessage, setConfirmPopupOneButtonOpen, setCvsError, setErrorPopupOpen, setOrganization)} />
                    </div>
                </div>
            </div>
            <ConfirmPopup
                message={confirmMessage}
                isOpen={isConfirmPopupOneButtonOpen}
                onClose={handlePopUpConfirmOrganizationSavedClick}
                buttons={[{ text: 'Bevestigen', onClick: handlePopUpConfirmOrganizationSavedClick, buttonType: {type:"Solid"}}]} />
            <ErrorPopup 
                error={cvsError} 
                isOpen={isErrorPopupOpen}
                onClose={() => setErrorPopupOpen(false)} />  
            <Copyright />
        </div>
        </>
    );
}
export default OrganizationEditor;

function handleSaveResult(
    apiResult: ApiResult<Organization>, 
    setConfirmMessage: React.Dispatch<React.SetStateAction<string>>, 
    setConfirmPopupOneButtonOpen: React.Dispatch<React.SetStateAction<boolean>>, 
    setCvsError: React.Dispatch<React.SetStateAction<CVSError>>, 
    setErrorPopupOpen: React.Dispatch<React.SetStateAction<boolean>>, 
    setClient: React.Dispatch<React.SetStateAction<Organization>>) {
    if (apiResult.Ok) {
        setConfirmMessage('Organisatie succesvol opgeslagen');
        setConfirmPopupOneButtonOpen(true);

        setClient(apiResult.ReturnObject!);
    }
    else {
        setCvsError({
            id: 0,
            errorcode: 'E',
            message: `Er is een opgetreden tijdens het opslaan van een organisatie. Foutmelding: ${apiResult.Errors!.join(', ')}`
        });
        setErrorPopupOpen(true);
    }
}

function showLoadingScreen(status: string): string | undefined {
    return ` ${status === StatusEnum.PENDING ? 'loading-spinner-visible' : 'loading-spinner-hidden'}`;
}