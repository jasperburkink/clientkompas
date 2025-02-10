import './user-editor.css';
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import CVSError from "types/common/cvs-error";
import StatusEnum from "types/common/StatusEnum";
import CreateUserCommand from "types/model/user/create-user/create-user-command";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import ErrorPopup from "components/common/error-popup";
import ConfirmPopup from "components/common/confirm-popup";
import Menu from "components/common/menu";
import { NavTitle } from "components/nav/nav-title";
import { Copyright } from "components/common/copyright";
import { Header } from "components/common/header";
import LabelField from "components/common/label-field";
import { InputField } from "components/common/input-field";
import { isHashEmpty, ValidationErrorHash } from "types/common/validation-error";
import { Dropdown } from "components/common/dropdown";
import SaveButton from "components/common/save-button";
import { createUser, fetchUserRoles } from "utils/api";
import ApiResult from "types/common/api-result";
import User from 'types/model/User';
import GetUserRolesDto from 'types/model/user/get-user-roles/get-user-roles.dto';

const UserEditor = () => { 
    const navigate = useNavigate();

    const [status, setStatus] = useState(StatusEnum.IDLE);

    const [user, setUser] = useState<User>({        
        firstname: '',
        prefixlastname: '',
        lastname: '',
        emailaddress: '',
        telephonenumber: '',
        rolename: 'Licensee'
    });

    const [userRoles, setUserRoles] = useState<GetUserRolesDto[]>([]);

    const [confirmMessage, setConfirmMessage] = useState<string>('');
    const [isConfirmPopupOneButtonOpen, setConfirmPopupOneButtonOpen] = useState<boolean>(false);

    const [validationErrors, setValidationErrors] = useState<ValidationErrorHash>({});
    const [error, setError] = useState<CVSError>({
        id: 1,
        errorcode: 'E12345',
        message: "Dit is een foutmelding"
    });
    const [isErrorPopupOpen, setErrorPopupOpen] = useState<boolean>(false);    

    const handlePopUpConfirmSavedClick = () => {
        setConfirmPopupOneButtonOpen(false);
        navigate(`/users/edit/${user.id}`);
    };

    const handleInputChange = (fieldName: string, value: any) => {
        setUser(createUserCmd => ({
            ...createUserCmd,
            [fieldName]: value
        }));
    };    

    useEffect (() => {
        const getUserRoles = async() => {
            setStatus(StatusEnum.PENDING);

            setUserRoles(await fetchUserRoles());

            setStatus(StatusEnum.SUCCESSFUL);
        }

        getUserRoles();
    }, []);

    const handleSaveResult = (
            apiResult: ApiResult<CreateUserCommand>, 
            setConfirmMessage: React.Dispatch<React.SetStateAction<string>>, 
            setConfirmPopupOneButtonOpen: React.Dispatch<React.SetStateAction<boolean>>, 
            setCvsError: React.Dispatch<React.SetStateAction<CVSError>>, 
            setErrorPopupOpen: React.Dispatch<React.SetStateAction<boolean>>, 
            setUser: React.Dispatch<React.SetStateAction<User>>): void => {
            if (apiResult.Ok) {
                setConfirmMessage('Medewerker opgeslagen');
                setConfirmPopupOneButtonOpen(true);
        
                setUser(apiResult.ReturnObject!);
            }
            else {
                if(apiResult.ValidationErrors && !isHashEmpty(apiResult.ValidationErrors)) {
                    setValidationErrors(apiResult.ValidationErrors);
                }
                else {
                    setCvsError({
                        id: 0,
                        errorcode: 'E',
                        message: `Er is een opgetreden tijdens het opslaan van een medewerker. Foutmelding: ${(apiResult.Errors as string[]).join(', ')}`
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
                    <NavTitle lijstNaam="Medewerkers" />
                    {/* <SearchUsers /> */}
                </Menu>

                <div className="user-container">
                    <div className='user-header'>
                        <Header text="Medewerker aanmaken" className='user-header-main' />
                        <p className='user-header-sub'> - Velden met * zijn verplicht</p>
                    </div>

                    <div className="user-field-container"> 
                        <LabelField text='Voornaam' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='Voornaam' 
                                value={user.firstname} 
                                onChange={(value) => handleInputChange('firstname', value)}
                                dataTestId='firstname'
                                errors={validationErrors.firstname} />
                        </LabelField>

                        <LabelField text='Tussenvoegsel' required={false}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={false} 
                                placeholder='bv. de' 
                                value={user.prefixlastname} 
                                onChange={(value) => handleInputChange('prefixlastname', value)}
                                dataTestId='prefixlastname'
                                errors={validationErrors.prefixlastname} />
                        </LabelField>

                        <LabelField text='Achternaam' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='Achternaam' 
                                value={user.lastname} 
                                onChange={(value) => handleInputChange('lastname', value)}
                                dataTestId='lastname'
                                errors={validationErrors.lastname} />
                        </LabelField>                        

                        <LabelField text='Mobiel' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='bv. 06-12345678' 
                                value={user.telephonenumber} 
                                onChange={(value) => handleInputChange('telephonenumber', value)}
                                dataTestId='telephonenumber'
                                errors={validationErrors.telephonenumber} />
                        </LabelField>

                        <LabelField text='E-mail' required={true}>
                            <InputField 
                                inputfieldtype={{type:'text'}} 
                                required={true} 
                                placeholder='bv. mail@mailbox.com' 
                                value={user.emailaddress} 
                                onChange={(value) => handleInputChange('emailaddress', value)}
                                dataTestId='emailaddress'
                                errors={validationErrors.emailaddress} />
                        </LabelField>

                        <LabelField text='Rol' required={true}>
                            <Dropdown 
                                options={userRoles.map((userRole) => { 
                                        return {
                                            label: userRole.name,
                                            value: userRole.value 
                                        }
                                    })
                                }
                                required={true} 
                                inputfieldname='dropdown'                                
                                value={user.rolename} 
                                onChange={(value) => handleInputChange('rolename', value)}
                                dataTestId='rolename'
                                errors={validationErrors.rolename} />
                        </LabelField>
                    </div>

                    <div className='button-container'>
                        <SaveButton
                            buttonText= "Opslaan"
                            loadingText = "Bezig met oplaan"
                            successText = "Medewerker opgeslagen"
                            errorText = "Fout tijdens opslaan"
                            onSave={async () => await createUser(user)}
                            onResult={(apiResult) => handleSaveResult(apiResult, setConfirmMessage, setConfirmPopupOneButtonOpen, setError, setErrorPopupOpen, setUser)}
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

export default UserEditor;

function showLoadingScreen(status: string): string | undefined {
    return ` ${status === StatusEnum.PENDING ? 'loading-spinner-visible' : 'loading-spinner-hidden'}`;
}