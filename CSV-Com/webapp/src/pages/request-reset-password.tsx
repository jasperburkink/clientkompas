import React, { useState } from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import { ValidationErrorHash } from "types/common/validation-error";
import StatusEnum from "types/common/StatusEnum";
import { ReactComponent as LogoLightSVG } from 'assets/CK_light_logo.svg';
import { ReactComponent as LogoDarkSVG } from 'assets/CK_dark_logo.svg';
import './request-reset-password.css';
import SidebarEmpty from "components/sidebar/sidebar-empty";
import { InputField } from "components/common/input-field";
import SaveButton from "components/common/save-button";
import RequestResetPasswordCommand from "types/model/request-reset-password/request-reset-password-command";
import RequestResetPasswordCommandDto from "types/model/request-reset-password/request-reset-password-command-dto";
import ApiResult, { getErrorMessage } from "types/common/api-result";
import CVSError from "types/common/cvs-error";
import ErrorPopup from "components/common/error-popup";
import { Copyright } from "components/common/copyright";
import ConfirmPopup from "components/common/confirm-popup";
import { useNavigate } from "react-router-dom";
import { requestResetPassword } from "utils/api";
import { Label } from "components/common/label";

const RequestResetPassword = () => {
    const navigate = useNavigate();

    const initialRequestResetPasswordCommand: RequestResetPasswordCommand = { 
        emailaddress: ''
    };

    const [validationErrors, setValidationErrors] = useState<ValidationErrorHash>({});
    const [error, setError] = useState<string | null>(null);
    const [status, setStatus] = useState(StatusEnum.IDLE);
    const [confirmMessage, setConfirmMessage] = useState<string>('');
    const [isConfirmPopupOneButtonOpen, setConfirmPopupOneButtonOpen] = useState<boolean>(false);
    const [requestResetPasswordCommand, setRequestResetPasswordCommand] = useState<RequestResetPasswordCommand>(initialRequestResetPasswordCommand);
   
    const [isErrorPopupOpen, setErrorPopupOpen] = useState<boolean>(false);
    const [cvsError, setCvsError] = useState<CVSError>(() => {
        return {
            id: 1,
            errorcode: 'E12345',
            message: "Dit is een foutmelding"
        }
    });

    const handleRequestResetPasswordCommandInputChange = (fieldName: string, value: string) => {
        setRequestResetPasswordCommand(prevCommand => ({
            ...prevCommand,
            [fieldName]: value
        }));
    };

    const handleRequestResetPasswordResult = (
        apiResult: ApiResult<RequestResetPasswordCommandDto>, 
        setConfirmMessage: React.Dispatch<React.SetStateAction<string>>, 
        setConfirmPopupOneButtonOpen: React.Dispatch<React.SetStateAction<boolean>>, 
        setCvsError: React.Dispatch<React.SetStateAction<CVSError>>, 
        setErrorPopupOpen: React.Dispatch<React.SetStateAction<boolean>>) => {
        if (apiResult.Ok) {
            if(apiResult.ReturnObject?.success === true) {                
                setConfirmMessage('Als het opegeven e-mailadres bij ons bekend is, sturen wij u een e-mail om uw wachtwoord opnieuw in te stellen.');
                setConfirmPopupOneButtonOpen(true);
            }
            else {
                setCvsError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens aanvragen van een nieuw wachtwoord. Foutmelding: ${getErrorMessage<RequestResetPasswordCommandDto>(apiResult)}.`
                });
                setErrorPopupOpen(true);

                setValidationErrors({});
            }
        }
        else {
            if(apiResult.ValidationErrors) {
                setValidationErrors(apiResult.ValidationErrors);
            }
            else {
                setCvsError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens aanvragen van een nieuw wachtwoord. Foutmelding: ${getErrorMessage<RequestResetPasswordCommandDto>(apiResult)}.`
                });
                setErrorPopupOpen(true);  

                setValidationErrors({});
            }
        }

        setStatus(StatusEnum.SUCCESSFUL);
    }

    const handlerequestresetpasswordConfirmedClick = () => {
        setConfirmPopupOneButtonOpen(false);
        navigate(`/login`);
    };    

    return(
        <>
        <div className={`loading-spinner ${showLoadingScreen(status)}`}>
            <FontAwesomeIcon icon={faSpinner} className="fa fa-2x fa-refresh fa-spin" />
        </div>

        <div className="flex flex-col lg:flex-row h-screen lg:h-auto request-reset-password-page">
            <div className='lg:flex w-full'>        
                <SidebarEmpty>
                    <LogoDarkSVG className="logo-dark" />
                </SidebarEmpty>

                <div className="request-reset-password-container">
                    <div className="request-reset-password-logo-container">
                        <LogoLightSVG className="logo-light" />                    
                    </div>

                    <div className="request-reset-password-fields-container">
                        <Label 
                            className="request-reset-password-label"
                            text="Vul uw email in om uw wachtwoord te resetten" />

                        <InputField 
                            inputfieldtype={{type:'text'}} 
                            required={true} 
                            placeholder='E-mailadres' 
                            className="request-reset-password-emailaddress" 
                            value={requestResetPasswordCommand.emailaddress}                            
                            onChange={(value) => handleRequestResetPasswordCommandInputChange('emailaddress', value)}
                            errors={validationErrors.emailaddress} 
                            dataTestId='emailaddress' />

                        <SaveButton 
                            buttonText="Versturen" 
                            className='request-reset-password-button'
                            loadingText = "Bezig met aanvragen"
                            successText = "Aanvraag succesvol"
                            errorText = "Fout tijdens aanvragen"
                            onSave={async () => {  
                                    setStatus(StatusEnum.PENDING);

                                    return await requestResetPassword(requestResetPasswordCommand);                                    
                                }
                            }
                            onResult={(apiResult) => handleRequestResetPasswordResult(
                                apiResult, 
                                setConfirmMessage, 
                                setConfirmPopupOneButtonOpen, 
                                setCvsError, 
                                setErrorPopupOpen)}
                            dataTestId='button.request-reset-password'
                        />
                    </div>
                </div>
            </div>

            <ConfirmPopup
                data-testid='confirm-popup'
                message={confirmMessage}
                isOpen={isConfirmPopupOneButtonOpen}
                onClose={handlerequestresetpasswordConfirmedClick}
                buttons={[{ text: 'Bevestigen', dataTestId: 'button.confirm', onClick: handlerequestresetpasswordConfirmedClick, buttonType: {type:"Solid"}}]} />

            <ErrorPopup 
                error={cvsError} 
                isOpen={isErrorPopupOpen}
                onClose={() => setErrorPopupOpen(false)} />  
                
            <Copyright />

        </div>
        </>
    );
}

export default RequestResetPassword;

function showLoadingScreen(status: string): string | undefined {
    return ` ${status === StatusEnum.PENDING ? 'loading-spinner-visible' : 'loading-spinner-hidden'}`;
}