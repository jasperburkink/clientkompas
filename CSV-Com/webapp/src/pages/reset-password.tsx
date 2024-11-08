import React, { useEffect, useState } from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import { ValidationErrorHash } from "types/common/validation-error";
import StatusEnum from "types/common/StatusEnum";
import { ReactComponent as LogoLightSVG } from 'assets/CK_light_logo.svg';
import { ReactComponent as LogoDarkSVG } from 'assets/CK_dark_logo.svg';
import './reset-password.css';
import SidebarEmpty from "components/sidebar/sidebar-empty";
import { InputField } from "components/common/input-field";
import SaveButton from "components/common/save-button";
import ResetPasswordCommand from "types/model/reset-password/reset-password-command";
import ResetPasswordCommandDto from "types/model/reset-password/reset-password-command-dto";
import ApiResult, { getErrorMessage } from "types/common/api-result";
import CVSError from "types/common/cvs-error";
import ErrorPopup from "components/common/error-popup";
import { Copyright } from "components/common/copyright";
import ConfirmPopup from "components/common/confirm-popup";
import { useNavigate, useParams } from "react-router-dom";
import { resetPassword } from "utils/api";
import { Label } from "components/common/label";
import PasswordField from "components/common/password-field";

const RequestResetPassword = () => {
    const navigate = useNavigate();

    const { emailaddress, token } = useParams();

    const initialResetPasswordCommand: ResetPasswordCommand = { 
        emailaddress: '',
        token: '',
        newpassword: '',
        newpasswordrepeat: ''
    };

    const [validationErrors, setValidationErrors] = useState<ValidationErrorHash>({});
    const [error, setError] = useState<string | null>(null);
    const [status, setStatus] = useState(StatusEnum.IDLE);
    const [confirmMessage, setConfirmMessage] = useState<string>('');
    const [isConfirmPopupOneButtonOpen, setConfirmPopupOneButtonOpen] = useState<boolean>(false);
    const [resetPasswordCommand, setResetPasswordCommand] = useState<ResetPasswordCommand>(initialResetPasswordCommand);
   
    const [isErrorPopupOpen, setErrorPopupOpen] = useState<boolean>(false);
    const [cvsError, setCvsError] = useState<CVSError>(() => {
        return {
            id: 1,
            errorcode: 'E12345',
            message: "Dit is een foutmelding"
        }
    });

    useEffect(() => {
        if(!emailaddress){
            setCvsError({
                id: 0,
                errorcode: 'E',
                message: `Bij het resetten van het wachtwoord is het emailadres verplicht.`
            });
            setErrorPopupOpen(true);
            return;
        }

        if(!token){            
            setCvsError({
                id: 0,
                errorcode: 'E',
                message: `Bij het resetten van het wachtwoord is een token verplicht.`
            });
            setErrorPopupOpen(true);
            return;
        }

        const decodedToken = decodeURIComponent(token);
        setResetPasswordCommand(prevCommand => ({
            ...prevCommand,
            emailaddress,
            token: decodedToken
        }));
    }, [emailaddress, token]);

    const handleResetPasswordCommandInputChange = (fieldName: string, value: string) => {
        setResetPasswordCommand(prevCommand => ({
            ...prevCommand,
            [fieldName]: value
        }));
    };

    const handleResetPasswordResult = (
        apiResult: ApiResult<ResetPasswordCommandDto>, 
        setConfirmMessage: React.Dispatch<React.SetStateAction<string>>, 
        setConfirmPopupOneButtonOpen: React.Dispatch<React.SetStateAction<boolean>>, 
        setCvsError: React.Dispatch<React.SetStateAction<CVSError>>, 
        setErrorPopupOpen: React.Dispatch<React.SetStateAction<boolean>>) => {
        if (apiResult.Ok) {
            if(apiResult.ReturnObject?.success === true) {                
                setConfirmMessage('Uw wachtwoord is opnieuw ingesteld. U kun uw nieuwe wachtwoord gebruiken om in te loggen.');
                setConfirmPopupOneButtonOpen(true);
            }
            else {
                setCvsError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens het opnieuw instellen van een nieuw wachtwoord. Foutmelding: ${getErrorMessage<ResetPasswordCommandDto>(apiResult)}.`
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
                    message: `Er is een opgetreden tijdens het opnieuw instellen van een nieuw wachtwoord. Foutmelding: ${getErrorMessage<ResetPasswordCommandDto>(apiResult)}.`
                });
                setErrorPopupOpen(true);  

                setValidationErrors({});
            }
        }

        setStatus(StatusEnum.SUCCESSFUL);
    }

    const handleResetPasswordConfirmedClick = () => {
        setConfirmPopupOneButtonOpen(false);
        navigate(`/login`);
    };    

    return(
        <>
        <div className={`loading-spinner ${showLoadingScreen(status)}`}>
            <FontAwesomeIcon icon={faSpinner} className="fa fa-2x fa-refresh fa-spin" />
        </div>

        <div className="flex flex-col lg:flex-row h-screen lg:h-auto reset-password-page">
            <div className='lg:flex w-full'>        
                <SidebarEmpty>
                    <LogoDarkSVG className="logo-dark" />
                </SidebarEmpty>

                <div className="reset-password-container">
                    <div className="reset-password-logo-container">
                        <LogoLightSVG className="logo-light" />
                    </div>

                    <div className="reset-password-fields-container">
                        <Label 
                            className="reset-password-label"
                            text="Vul een nieuw wachtwoord in. Deze moet voldoen aan">
                            <li>
                                <ul>Minimaal acht tekens lang</ul>
                                <ul>Maximaal 255 tekens lang</ul>
                                <ul>Minimaal één hoofdletter</ul>
                                <ul>Minimaal één kleine letter</ul>
                                <ul>Minimaal één cijfer</ul>
                                <ul>Minimaal één speciaal teken (! # - _)</ul>
                            </li>
                        </Label>

                        <PasswordField 
                            inputfieldname='newpassword'
                            placeholder='Nieuw wachtwoord' 
                            className="reset-password-newpassword" 
                            value={resetPasswordCommand.newpassword}                            
                            onChange={(value: any) => handleResetPasswordCommandInputChange('newpassword', value)}
                            errors={validationErrors.newpassword} 
                            dataTestId='newpassword' />

                        <PasswordField 
                            inputfieldname='newpasswordrepeat'
                            placeholder='Herhaal nieuw wachtwoord' 
                            className="reset-password-newpasswordrepeat" 
                            value={resetPasswordCommand.newpasswordrepeat}                            
                            onChange={(value) => handleResetPasswordCommandInputChange('newpasswordrepeat', value)}
                            errors={validationErrors.newpasswordrepeat} 
                            dataTestId='newpasswordrepeat' />


                        <SaveButton 
                            buttonText="Reset wachtwoord" 
                            className='reset-password-button'
                            loadingText = "Bezig met aanvragen"
                            successText = "Aanvraag succesvol"
                            errorText = "Fout tijdens aanvragen"
                            onSave={async () => {  
                                    setStatus(StatusEnum.PENDING);

                                    return await resetPassword(resetPasswordCommand);                                    
                                }
                            }
                            onResult={(apiResult) => handleResetPasswordResult(
                                apiResult, 
                                setConfirmMessage, 
                                setConfirmPopupOneButtonOpen, 
                                setCvsError, 
                                setErrorPopupOpen)}
                            dataTestId='button.reset-password'
                        />
                    </div>
                </div>
            </div>

            <ConfirmPopup
                data-testid='confirm-popup'
                message={confirmMessage}
                isOpen={isConfirmPopupOneButtonOpen}
                onClose={handleResetPasswordConfirmedClick}
                buttons={[{ text: 'Bevestigen', dataTestId: 'button.confirm', onClick: handleResetPasswordConfirmedClick, buttonType: {type:"Solid"}}]} />

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