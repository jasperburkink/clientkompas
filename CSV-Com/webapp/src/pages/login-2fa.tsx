import React, { useEffect, useState, useContext } from "react";
import Menu from 'components/common/menu';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import { ValidationErrorHash } from "types/common/validation-error";
import StatusEnum from "types/common/StatusEnum";
import { ReactComponent as LogoLightSVG } from 'assets/CK_light_logo.svg';
import { ReactComponent as LogoDarkSVG } from 'assets/CK_dark_logo.svg';
import { Sidebar } from "components/sidebar/sidebar";
import './login-2fa.css';
import SidebarEmpty from "components/sidebar/sidebar-empty";
import { InputField } from "components/common/input-field";
import PasswordField from "components/common/password-field";
import { LinkButton } from "components/common/link-button";
import { Button } from "components/common/button";
import SaveButton from "components/common/save-button";
import { login, login2FA } from "utils/api";
import LoginCommand from "types/model/login/login-command";
import LoginCommandDto from "types/model/login/login-command-dto";
import ApiResult, { getErrorMessage } from "types/common/api-result";
import CVSError from "types/common/cvs-error";
import ErrorPopup from "components/common/error-popup";
import { Copyright } from "components/common/copyright";
import ConfirmPopup from "components/common/confirm-popup";
import { useNavigate, useParams } from "react-router-dom";
import { BearerToken } from "types/common/bearer-token";
import RefreshTokenService from "utils/refresh-token-service";
import TwoFactorAuthenticationCommand from "types/model/login-2fa/login-2fa-command";
import TwoFactorAuthenticationCommandDto from "types/model/login-2fa/login-2fa-command-dto";
import { Label } from "components/common/label";

const Login2FA = () => {
    const navigate = useNavigate();

    const { userid } = useParams();    

    const initialLogin2FACommand: TwoFactorAuthenticationCommand = {
        userid: userid!,
        token: "",
        twofactorpendingtoken: localStorage.getItem('twofactorpendingtoken')!
    };

    const [validationErrors, setValidationErrors] = useState<ValidationErrorHash>({});
    const [error, setError] = useState<string | null>(null);
    const [status, setStatus] = useState(StatusEnum.IDLE);
    const [confirmMessage, setConfirmMessage] = useState<string>('');
    const [isConfirmPopupOneButtonOpen, setConfirmPopupOneButtonOpen] = useState<boolean>(false);
    const [login2FACommand, setLogin2FACommand] = useState<TwoFactorAuthenticationCommand>(initialLogin2FACommand);
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

    const handleLogin2FACommandInputChange = (fieldName: string, value: string) => {
        setLogin2FACommand(prevOrganization => ({
            ...prevOrganization,
            [fieldName]: value
        }));
    };

    const handleLogin2FAResult = (
        apiResult: ApiResult<TwoFactorAuthenticationCommandDto>, 
        setConfirmMessage: React.Dispatch<React.SetStateAction<string>>, 
        setConfirmPopupOneButtonOpen: React.Dispatch<React.SetStateAction<boolean>>, 
        setCvsError: React.Dispatch<React.SetStateAction<CVSError>>, 
        setErrorPopupOpen: React.Dispatch<React.SetStateAction<boolean>>, 
        setBearerToken: React.Dispatch<React.SetStateAction<BearerToken | null>>) => {
        if (apiResult.Ok) {           
            if(apiResult.ReturnObject?.success === true 
                && apiResult.ReturnObject?.bearertoken
                && apiResult.ReturnObject?.refreshtoken) {

                setConfirmMessage('Inloggen succesvol.');
                setConfirmPopupOneButtonOpen(true);
                setBearerToken(new BearerToken(apiResult.ReturnObject.bearertoken));
                setRefreshToken(apiResult.ReturnObject.refreshtoken);
            }
            else{
                setCvsError({
                    id: 0,
                    errorcode: 'E',
                    message: `Er is een opgetreden tijdens het inloggen. Foutmelding: ${getErrorMessage<TwoFactorAuthenticationCommandDto>(apiResult)}.`
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
                    message: `Er is een opgetreden tijdens het inloggen. Foutmelding: ${getErrorMessage<TwoFactorAuthenticationCommandDto>(apiResult)}`
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
                        <Label text="Vul de code in die u op uw e-mail heeft ontvangen om in te loggen" />

                        <InputField 
                            inputfieldtype={{type:'text'}} 
                            required={true} 
                            placeholder='Code' 
                            className="login-token" 
                            value={login2FACommand.token}                            
                            onChange={(value) => handleLogin2FACommandInputChange('token', value)}
                            errors={validationErrors.username} 
                            dataTestId='token' />

                        <SaveButton 
                            buttonText="Inloggen" 
                            className='login-button'
                            loadingText = "Bezig met inloggen"
                            successText = "Inloggen succesvol"
                            errorText = "Fout tijdens inloggen"
                            onSave={async () => {  
                                    setStatus(StatusEnum.PENDING);

                                    return await login2FA(login2FACommand);                                    
                                }
                            }
                            onResult={(apiResult) => handleLogin2FAResult(
                                apiResult, 
                                setConfirmMessage, 
                                setConfirmPopupOneButtonOpen, 
                                setCvsError, 
                                setErrorPopupOpen, 
                                setBearerToken)}
                            dataTestId='button.login'
                        />

                        <Button buttonType={{type:"NotSolid"}} text="Button2" className='w-200px h-50px' onClick={()=> {
                            
                        }} />
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

export default Login2FA;

function showLoadingScreen(status: string): string | undefined {
    return ` ${status === StatusEnum.PENDING ? 'loading-spinner-visible' : 'loading-spinner-hidden'}`;
}