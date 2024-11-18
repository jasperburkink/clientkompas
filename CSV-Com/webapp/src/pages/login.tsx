import React, { useEffect, useState, useContext } from "react";
import Menu from 'components/common/menu';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import { ValidationErrorHash } from "types/common/validation-error";
import StatusEnum from "types/common/StatusEnum";
import { ReactComponent as LogoLightSVG } from 'assets/CK_light_logo.svg';
import { ReactComponent as LogoDarkSVG } from 'assets/CK_dark_logo.svg';
import { Sidebar } from "components/sidebar/sidebar";
import './login.css';
import SidebarEmpty from "components/sidebar/sidebar-empty";
import { InputField } from "components/common/input-field";
import PasswordField from "components/common/password-field";
import { LinkButton } from "components/common/link-button";
import { Button } from "components/common/button";
import SaveButton from "components/common/save-button";
import { login } from "utils/api";
import LoginCommand from "types/model/login/login-command";
import LoginCommandDto from "types/model/login/login-command-dto";
import ApiResult, { getErrorMessage } from "types/common/api-result";
import CVSError from "types/common/cvs-error";
import ErrorPopup from "components/common/error-popup";
import { Copyright } from "components/common/copyright";
import ConfirmPopup from "components/common/confirm-popup";
import { useNavigate } from "react-router-dom";
import { BearerToken } from "types/common/bearer-token";
import RefreshTokenService from "utils/refresh-token-service";

const Login = () => {
    const navigate = useNavigate();

    const initialLoginCommand: LoginCommand = { 
        username: '',
        password: ''
    };

    const [validationErrors, setValidationErrors] = useState<ValidationErrorHash>({});
    const [error, setError] = useState<string | null>(null);
    const [status, setStatus] = useState(StatusEnum.IDLE);
    const [confirmMessage, setConfirmMessage] = useState<string>('');
    const [isConfirmPopupOneButtonOpen, setConfirmPopupOneButtonOpen] = useState<boolean>(false);
    const [loginCommand, setLoginCommand] = useState<LoginCommand>(initialLoginCommand);
    const [bearertoken, setBearerToken] = useState<BearerToken | null>(sessionStorage.getItem('token') ? BearerToken.deserialize(sessionStorage.getItem('token')!) : null);
    const [refreshtoken, setRefreshToken] = useState<string | null>(RefreshTokenService.getInstance().getRefreshToken() ? RefreshTokenService.getInstance().getRefreshToken() : null);
    
    useEffect(() => {         
        if(bearertoken) {
            sessionStorage.setItem('token', BearerToken.serialize(bearertoken));
        }

        if(refreshtoken) {
            RefreshTokenService.getInstance().setRefreshToken(refreshtoken);
        }
    }, [bearertoken, refreshtoken]);

    const [isErrorPopupOpen, setErrorPopupOpen] = useState<boolean>(false);
    const [cvsError, setCvsError] = useState<CVSError>(() => {
        return {
            id: 1,
            errorcode: 'E12345',
            message: "Dit is een foutmelding"
        }
    });

    const handleLoginCommandInputChange = (fieldName: string, value: string) => {
        setLoginCommand(prevOrganization => ({
            ...prevOrganization,
            [fieldName]: value
        }));
    };

    const handleLoginResult = (
        apiResult: ApiResult<LoginCommandDto>, 
        setConfirmMessage: React.Dispatch<React.SetStateAction<string>>, 
        setConfirmPopupOneButtonOpen: React.Dispatch<React.SetStateAction<boolean>>, 
        setCvsError: React.Dispatch<React.SetStateAction<CVSError>>, 
        setErrorPopupOpen: React.Dispatch<React.SetStateAction<boolean>>, 
        setBearerToken: React.Dispatch<React.SetStateAction<BearerToken | null>>) => {
        if (apiResult.Ok) {            
            if(apiResult.ReturnObject?.success === true){
                // Check if the user needs to supply a 2FA token to login
                if(apiResult.ReturnObject?.twofactorpendingtoken && apiResult.ReturnObject?.userid) {                    
                    navigate(`/login-2fa/${apiResult.ReturnObject?.userid}`); // TODO: waar bewaar ik de expiredat? apiResult.ReturnObject?.expiresat
                }
                // User logged in successfully
                else if (apiResult.ReturnObject?.bearertoken && apiResult.ReturnObject?.refreshtoken) {
                    setConfirmMessage('Inloggen succesvol.');
                    setConfirmPopupOneButtonOpen(true);
                    setBearerToken(new BearerToken(apiResult.ReturnObject.bearertoken));
                    setRefreshToken(apiResult.ReturnObject.refreshtoken);
                }
                else {
                    setCvsError({
                        id: 0,
                        errorcode: 'E',
                        message: `Er is een opgetreden tijdens het inloggen. Foutmelding: Not a valid loginresult.`
                    });
                    setErrorPopupOpen(true);
    
                    setValidationErrors({});
                }
            }
            else{
                setCvsError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens het inloggen. Foutmelding: ${getErrorMessage<LoginCommandDto>(apiResult)}.`
                });
                setErrorPopupOpen(true);

                setValidationErrors({});
            }
        }
        else {
            if(apiResult.ValidationErrors) {
                setValidationErrors(apiResult.ValidationErrors);
            }

            if(apiResult.Title) {
                setCvsError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens het inloggen. Foutmelding: ${getErrorMessage<LoginCommandDto>(apiResult)}`
                });
                setErrorPopupOpen(true);  

                setValidationErrors({});
            }
        }

        setStatus(StatusEnum.SUCCESSFUL);
    }

    const handleLoginConfirmedClick = () => {
        setConfirmPopupOneButtonOpen(false);
        navigate(`/clients/`); // TODO: navigate to homepage
    };    

    return(
        <>
        <div className={`loading-spinner ${showLoadingScreen(status)}`}>
            <FontAwesomeIcon icon={faSpinner} className="fa fa-2x fa-refresh fa-spin" />
        </div>

        <div className="flex flex-col lg:flex-row h-screen lg:h-auto login-page">
            <div className='lg:flex w-full'>        
                <SidebarEmpty>
                    <LogoDarkSVG className="logo-dark" />
                </SidebarEmpty>

                <div className="login-container">
                    <div className="login-logo-container">
                        <LogoLightSVG className="logo-light" />                    
                    </div>

                    <div className="login-fields-container">
                        <InputField 
                            inputfieldtype={{type:'text'}} 
                            required={true} 
                            placeholder='Email' 
                            className="login-username" 
                            value={loginCommand.username}                            
                            onChange={(value) => handleLoginCommandInputChange('username', value)}
                            errors={validationErrors.username} 
                            dataTestId='username' />

                        <PasswordField 
                            inputfieldname='password' 
                            placeholder='Wachtwoord' 
                            className="login-password" 
                            value={loginCommand.password}
                            onChange={(value) => handleLoginCommandInputChange('password', value)}
                            errors={validationErrors.password}
                            dataTestId='password' />

                        <LinkButton 
                            buttonType={{type:"Underline"}} 
                            text="Wachtwoord vergeten?" 
                            href="../password-forgotten" 
                            className="login-password-forgotten"
                            dataTestId='login-password-forgotten' />

                        <SaveButton 
                            buttonText="Inloggen" 
                            className='login-button'
                            loadingText = "Bezig met inloggen"
                            successText = "Inloggen succesvol"
                            errorText = "Fout tijdens inloggen"
                            onSave={async () => {  
                                    setStatus(StatusEnum.PENDING);

                                    return await login(loginCommand);                                    
                                }
                            }
                            onResult={(apiResult) => handleLoginResult(
                                apiResult, 
                                setConfirmMessage, 
                                setConfirmPopupOneButtonOpen, 
                                setCvsError, 
                                setErrorPopupOpen, 
                                setBearerToken)}
                            dataTestId='button.login'
                        />
                    </div>
                </div>
            </div>

            <ConfirmPopup
                data-testid='confirm-popup'
                message={confirmMessage}
                isOpen={isConfirmPopupOneButtonOpen}
                onClose={handleLoginConfirmedClick}
                buttons={[{ text: 'Bevestigen', dataTestId: 'button.confirm', onClick: handleLoginConfirmedClick, buttonType: {type:"Solid"}}]} />

            <ErrorPopup 
                error={cvsError} 
                isOpen={isErrorPopupOpen}
                onClose={() => setErrorPopupOpen(false)} />  
                
            <Copyright />

        </div>
        </>
    );
}

export default Login;

function showLoadingScreen(status: string): string | undefined {
    return ` ${status === StatusEnum.PENDING ? 'loading-spinner-visible' : 'loading-spinner-hidden'}`;
}