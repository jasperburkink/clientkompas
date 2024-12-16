import React, { useEffect, useState, useContext } from "react";
import Menu from 'components/common/menu';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import { isHashEmpty, ValidationErrorHash } from "types/common/validation-error";
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
import { login2FA, resend2FAToken } from "utils/api";
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
import { Moment } from "moment";
import moment from "moment";
import { CountdownButton } from "components/common/countdown-button";
import ResendTwoFactorAuthenticationTokenCommand from "types/model/resend-2fa-token/resend-2fa-token-command";
import AccessTokenService from "utils/access-token-service";

const Login2FA = () => {
    const COUNTDOWN_SECONDS: number = 60;

    const navigate = useNavigate();

    const { userid, remainingtimeinseconds } = useParams();

    const initialLogin2FACommand: TwoFactorAuthenticationCommand = {
        userid: userid!,
        token: "",
        twofactorpendingtoken: sessionStorage.getItem('twofactorpendingtoken')!
    };

    const [validationErrors, setValidationErrors] = useState<ValidationErrorHash>({});
    const [error, setError] = useState<string | null>(null);
    const [status, setStatus] = useState(StatusEnum.IDLE);
    const [confirmMessage, setConfirmMessage] = useState<string>('');
    const [isConfirmPopupOneButtonOpen, setConfirmPopupOneButtonOpen] = useState<boolean>(false);
    const [confirmOnClose, setConfirmOnClose] = useState<() => void>(() => {});
    const [login2FACommand, setLogin2FACommand] = useState<TwoFactorAuthenticationCommand>(initialLogin2FACommand);    
    const [remainingSecondsTokenValid, setRemainingSecondsTokenValid] = useState<number>(parseInt(remainingtimeinseconds!, 10));    

    const [isErrorPopupOpen, setErrorPopupOpen] = useState<boolean>(false);
    const [cvsError, setCvsError] = useState<CVSError>(() => {
        return {
            id: 1,
            errorcode: 'E12345',
            message: "Dit is een foutmelding"
        }
    });

    // Countdown seconds token is valid
    useEffect(() => {
        const interval = setInterval(() => {
          setRemainingSecondsTokenValid((prev) => Math.max(prev - 1, 0)); 
        }, 1000);
    
        return () => clearInterval(interval);
      }, []);

    const formatTime = (seconds: number) => {
        const duration = moment.duration(seconds, "seconds");
        return `${duration.minutes()} minuten en ${duration.seconds()} seconden`;
      };

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
        setConfirmOnClose: React.Dispatch<React.SetStateAction<() => void>>, 
        setCvsError: React.Dispatch<React.SetStateAction<CVSError>>, 
        setErrorPopupOpen: React.Dispatch<React.SetStateAction<boolean>>) => {
        if (apiResult.Ok) {           
            if(apiResult.ReturnObject?.success === true 
                && apiResult.ReturnObject?.bearertoken
                && apiResult.ReturnObject?.refreshtoken) {

                setConfirmMessage('Inloggen succesvol.');
                setConfirmPopupOneButtonOpen(true);
                setConfirmOnClose(() => handleLoginConfirmedClick);
                AccessTokenService.getInstance().setAccessToken(new BearerToken(apiResult.ReturnObject.bearertoken));
                RefreshTokenService.getInstance().setRefreshToken(apiResult.ReturnObject.refreshtoken);
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
            if(apiResult.ValidationErrors && !isHashEmpty(apiResult.ValidationErrors)) {
                setValidationErrors(apiResult.ValidationErrors);
            }
            else {
                if(apiResult.Title) {
                    setCvsError({
                        id: 0,
                        errorcode: 'E',
                        message: `Er is een opgetreden tijdens het inloggen.\n\nFoutmelding: ${getErrorMessage<TwoFactorAuthenticationCommandDto>(apiResult)}`
                    });
                    setErrorPopupOpen(true);

                    setValidationErrors({});
                }
            }
        }

        setStatus(StatusEnum.SUCCESSFUL);
    }

    const handleLoginConfirmedClick = () => {
        setConfirmPopupOneButtonOpen(false);
        navigate(`/clients/`); // TODO: navigate to homepage
    };

    const handleResend2FATokenConfirmedClick = () => {
        setConfirmPopupOneButtonOpen(false);
    };

    const resendToken = async () => {
        try {
            let command: ResendTwoFactorAuthenticationTokenCommand = {
                userid: userid!,
                twofactorpendingtoken: AccessTokenService.getInstance().getTwoFactorPendingToken()!
            }

            var apiResult = await resend2FAToken(command);

            if(apiResult.Ok && apiResult.ReturnObject){
                const expiryDate = new Date(apiResult.ReturnObject.expiresat);                    
                const remainingTimeInSeconds = Math.floor((expiryDate.getTime() - new Date().getTime()) / 1000);
                setRemainingSecondsTokenValid(remainingTimeInSeconds);
                AccessTokenService.getInstance().setTwoFactorPendingToken(apiResult.ReturnObject.twofactorpendingtoken);
                
                setConfirmMessage('Token is opnieuw verstuurd.');
                setConfirmPopupOneButtonOpen(true);
                setConfirmOnClose(() => handleResend2FATokenConfirmedClick);
            }
            else{
                if(apiResult.ValidationErrors && !isHashEmpty(apiResult.ValidationErrors)){
                    setValidationErrors(apiResult.ValidationErrors);
                }
                else{
                    setCvsError({
                        id: 0,
                        errorcode: 'E',
                        message: `Er is een opgetreden tijdens het opnieuw aanvragen van een token. Foutmelding: ${getErrorMessage<ResendTwoFactorAuthenticationTokenCommand>(apiResult)}.`
                    });
                    setErrorPopupOpen(true);
                }
        }
        }
        catch(error: any) {
            setCvsError({
                id: 0,
                errorcode: 'E',
                message: `Er is een opgetreden tijdens het opnieuw aanvragen van een token. Foutmelding: ${error.message})}`
            });
            setErrorPopupOpen(true);

            setValidationErrors({});
        }
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
                            errors={validationErrors.token} 
                            dataTestId='token' />

                        <Label 
                            text={`De verstuurde code is nog ${formatTime(remainingSecondsTokenValid)} geldig.`}
                            className="token-validity" />

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
                                setConfirmOnClose,
                                setCvsError, 
                                setErrorPopupOpen)}
                            dataTestId='button.login'
                        />

                        <CountdownButton
                            buttonType={{ type: "NotSolid" }}
                            text="Code niet ontvangen"
                            className='resend-button'
                            countdownMax={COUNTDOWN_SECONDS}
                            onClick={()=> resendToken()} />
                    </div>
                </div>
            </div>

            <ConfirmPopup
                data-testid='confirm-popup'
                message={confirmMessage}
                isOpen={isConfirmPopupOneButtonOpen}
                onClose={handleLoginConfirmedClick}
                buttons={[{ text: 'Bevestigen', dataTestId: 'button.confirm', onClick: () => {confirmOnClose();}, buttonType: {type:"Solid"}}]} />

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