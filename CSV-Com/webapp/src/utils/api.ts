import ClientQuery from "types/model/ClientQuery";
import BenefitForm from "types/model/BenefitForm";
import Diagnosis from "types/model/Diagnosis";
import MaritalStatus from "types/model/MaritalStatus";
import DriversLicence from "types/model/DriversLicence";
import Client from "types/model/Client";
import moment from 'moment';
import { isNullOrEmpty } from "./utilities";
import ApiResult from "types/common/api-result";
import { Console, error } from "console";
import Organization from "types/model/Organization";
import { ValidationErrorHash, ValidationError, parseValidationErrors } from "types/common/validation-error";
import { Type } from "typescript";
import CoachingProgramQuery from "types/model/CoachingProgramQuery";
import CoachingProgram from "types/model/CoachingProgram";
import LoginCommand from "types/model/login/login-command";
import LoginCommandDto from "types/model/login/login-command-dto";
import { BearerToken } from "types/common/bearer-token";
import RefreshTokenService from "utils/refresh-token-service";
import CoachingProgramEdit from "types/model/CoachingProgramEdit";
import GetClientFullnameDto from "types/model/GetClientFullnameDto";
import GetCoachingProgramTypesDto from "types/model/GetCoachingProgramTypesDto";
import LogoutCommand from "types/model/logout/logout-command";
import LogoutCommandDto from "types/model/logout/logout-command-dto";
import RequestResetPasswordCommand from "types/model/request-reset-password/request-reset-password-command";
import RequestResetPasswordCommandDto from "types/model/request-reset-password/request-reset-password-command-dto";
import ResetPasswordCommandDto from "types/model/reset-password/reset-password-command-dto";
import ResetPasswordCommand from "types/model/reset-password/reset-password-command";
import TwoFactorAuthenticationCommandDto from "types/model/login-2fa/login-2fa-command-dto";
import TwoFactorAuthenticationCommand from "types/model/login-2fa/login-2fa-command";
import ResendTwoFactorAuthenticationTokenCommand from "types/model/resend-2fa-token/resend-2fa-token-command";
import ResendTwoFactorAuthenticationTokenCommandDto from "types/model/resend-2fa-token/resend-2fa-token-command-dto";
import GetMenuByUserDto from "types/model/menu/get-menu-by-user-dto";
import CreateUserCommand from "types/model/user/create-user/create-user-command";
import CreateUserCommandDto from "types/model/user/create-user/create-user-command-dto";
import GetUserRolesDto from "types/model/user/get-user-roles/get-user-roles.dto";
import SearchUserQueryDto from "types/model/user/search-users/search-user-query-dto";

const apiUrl = process.env.REACT_APP_API_URL;

async function fetchAPI<T>(url: string, method: string = 'GET', body?: any, resultPattern: boolean = false): Promise<ApiResult<T>> {
    let bearerTokenJson: string | null = sessionStorage.getItem('token');
    let bearerToken: BearerToken | null = null;

    // Token expired?
    if(bearerTokenJson){
        bearerToken = BearerToken.deserialize(bearerTokenJson);

        if(bearerToken.isExpired()){
            const newToken = await RefreshTokenService.getInstance().refreshAccessToken();

            if(!newToken) {
                return Promise.reject({
                    Ok: false,
                    Errors: ['Unauthorized access']
                });
            }
        }
    }
    // Refresh token?
    else {
        const newToken = await RefreshTokenService.getInstance().refreshAccessToken();

        if(!newToken) {
            return Promise.reject({
                Ok: false,
                Errors: ['Unauthorized access']
            });
        }
    }

    const options: RequestInit = {
        method,
        headers: {
          'Content-type': 'application/json',
          ...(bearerToken ? {'Authorization': `Bearer ${bearerToken.getToken()}`} : {})
        },
        body: body ? JSON.stringify(body) : undefined,
      };
    
    const response = await fetch(url, options);
    
    return resultPattern ? handleApiResonseResult<T>(response) : handleApiResonse<T>(response);
}

//TODO: move this to global file
const DATE_FORMAT_JSON = 'yyyy-MM-DD';
    
moment.prototype.toJSON = function(){
    return moment(this).format(DATE_FORMAT_JSON);
}

Date.prototype.toJSON = function(){
    return moment(this).format(DATE_FORMAT_JSON);
}

/**
 * @deprecated This method is deprecated and will be removed. Please use handleApiResonseResult when result pattern is implemented.
 */
const handleApiResonse = async <T>(response: Response): Promise<ApiResult<T>> => {
    // Ok API response
    if(response.ok){
        let ObjectReturn: T = await response.json();

        return {
            Ok: response.ok,
            ReturnObject: ObjectReturn
        }
    }

    // TODO: Implement all HTTP status codes with the pages. https://sbict.atlassian.net/wiki/spaces/CVS/pages/35356674/Foutafhandeling#Gehele-pagina%E2%80%99s

    // Bad request --> Validation errors
    if(response.status === 400) {
        var responseData = await response.json();

        let titleResponse: string | undefined;
        let errorsResponse: string[] | undefined;
        let validationErrorsResponse: ValidationErrorHash | undefined;

        try{
            validationErrorsResponse = parseValidationErrors(responseData);
        }
        catch(err){
            console.log(`Error while parsing api validation errors. Error:${err}`);
        }

        try{
            let {title, errors} = responseData;
            titleResponse = title;
            errorsResponse = processErrors(errors);
        }
        catch(err){
            console.log(`Error while parsing api errors. Error:${err}`);
        }

        return {
            Ok: response.ok,
            Title: titleResponse,
            Errors: errorsResponse,
            ValidationErrors: validationErrorsResponse
        }  
    }    
    // Unauthorized 
    else if (response.status === 401) {
        window.location.href = '/unauthorized';
        return Promise.reject({
            Ok: false,
            Errors: ['Unauthorized access']
        });
    }
    // Forbidden 
    else if (response.status === 403) {
        window.location.href = '/forbidden';
        return Promise.reject({
            Ok: false,
            Errors: ['Forbidden access']
        });
    }
    // Internal error
    else if (response.status === 500) {
        window.location.href = '/internalerror';
        return Promise.reject({
            Ok: false,
            Errors: ['Internal Error']
        });
    }
    // Error
    else {
        let titleResponse: string | undefined;
        let errorsResponse: string[] | undefined;
        let validationErrorsResponse: ValidationErrorHash | undefined;

        try{
            let {title, errors} = responseData;
            titleResponse = title;
            errorsResponse = processErrors(errors);
        }
        catch(err){
            console.log(`Error while parsing api errors. Error:${err}`);
        }

        return {
            Ok: response.ok,
            Title: titleResponse,
            Errors: errorsResponse,
            ValidationErrors: validationErrorsResponse
        }  
    }
}

const handleApiResonseResult = async <T>(response: Response): Promise<ApiResult<T>> => {
    // Ok API response
    if(response.ok){
        let responseJson = await response.json();

        if(responseJson.succeeded)
        {
            let ObjectReturn: T = responseJson.value;

            return {
                Ok: responseJson.succeeded,
                ReturnObject: ObjectReturn
            }
        }
        else
        {
            return {
                Ok: responseJson.succeeded,
                Errors: responseJson.errors
            }
        }
    }

    // TODO: Implement all HTTP status codes with the pages. https://sbict.atlassian.net/wiki/spaces/CVS/pages/35356674/Foutafhandeling#Gehele-pagina%E2%80%99s

    // Bad request --> Validation errors
    if(response.status === 400) {
        var responseData = await response.json();

        let titleResponse: string | undefined;
        let errorsResponse: string[] | undefined;
        let validationErrorsResponse: ValidationErrorHash | undefined;

        try{
            validationErrorsResponse = parseValidationErrors(responseData);
        }
        catch(err){
            console.log(`Error while parsing api validation errors. Error:${err}`);
        }

        try{
            let {title, errors} = responseData;
            titleResponse = title;
            errorsResponse = processErrors(errors);
        }
        catch(err){
            console.log(`Error while parsing api errors. Error:${err}`);
        }

        return {
            Ok: response.ok,
            Title: titleResponse,
            Errors: errorsResponse,
            ValidationErrors: validationErrorsResponse
        }  
    }    
    // Unauthorized 
    else if (response.status === 401) {
        window.location.href = '/unauthorized';
        return Promise.reject({
            Ok: false,
            Errors: ['Unauthorized access']
        });
    }
    // Forbidden 
    else if (response.status === 403) {
        window.location.href = '/forbidden';
        return Promise.reject({
            Ok: false,
            Errors: ['Forbidden access']
        });
    }
    // Internal error
    else if (response.status === 500) {
        window.location.href = '/internalerror';
        return Promise.reject({
            Ok: false,
            Errors: ['Internal Error']
        });
    }
    // Error
    else {
        let titleResponse: string | undefined;
        let errorsResponse: string[] | undefined;
        let validationErrorsResponse: ValidationErrorHash | undefined;

        try{
            let {title, errors} = responseData;
            titleResponse = title;
            errorsResponse = processErrors(errors);
        }
        catch(err){
            console.log(`Error while parsing api errors. Error:${err}`);
        }

        return {
            Ok: response.ok,
            Title: titleResponse,
            Errors: errorsResponse,
            ValidationErrors: validationErrorsResponse
        }  
    }
}

export const login = async (loginCommand: LoginCommand): Promise<ApiResult<LoginCommandDto>> => {
    let method = 'POST';

    const requestOptions: RequestInit = {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginCommand)
    };

    const response = await fetch(`${apiUrl}Authentication`, requestOptions);     
    
    return handleApiResonse<LoginCommandDto>(response);
}

export const logout = async (logoutCommand: LogoutCommand): Promise<ApiResult<LogoutCommandDto>> => {
    let method = 'POST';

    const requestOptions: RequestInit = {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(logoutCommand)
    };

    const response = await fetch(`${apiUrl}Authentication/Logout`, requestOptions);     
    
    return handleApiResonse<LoginCommandDto>(response);
}

export const login2FA = async (loginCommand: TwoFactorAuthenticationCommand): Promise<ApiResult<TwoFactorAuthenticationCommandDto>> => {
    let method = 'POST';

    const requestOptions: RequestInit = {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginCommand)
    };

    const response = await fetch(`${apiUrl}Authentication/TwoFactorLogin`, requestOptions);     
    
    return handleApiResonse<TwoFactorAuthenticationCommandDto>(response);
}

export const resend2FAToken = async (loginCommand: ResendTwoFactorAuthenticationTokenCommand): Promise<ApiResult<ResendTwoFactorAuthenticationTokenCommandDto>> => {
    let method = 'POST';

    const requestOptions: RequestInit = {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(loginCommand)
    };

    const response = await fetch(`${apiUrl}Authentication/ResendTwoFactorToken`, requestOptions);     
    
    return handleApiResonse<ResendTwoFactorAuthenticationTokenCommandDto>(response);
}

export const fetchMenuByUserId = async (userId: string): Promise<GetMenuByUserDto> => {
    return (await fetchAPI<GetMenuByUserDto>(`${apiUrl}Menu?UserId=${userId}`)).ReturnObject!;
}

export const fetchClient = async (clientId: string): Promise<ClientQuery> => {
    return (await fetchAPI<ClientQuery>(`${apiUrl}Client/${clientId}`)).ReturnObject!;
}

export const fetchClientEditor = async (clientId: string): Promise<Client> => {
    let client = (await fetchAPI<Client>(`${apiUrl}Client/GetClientEditor/${clientId}`)).ReturnObject!;

    // Parse dates in client object
    client.dateofbirth = client.dateofbirth ? new Date(client.dateofbirth) : undefined;
    if (client.workingcontracts) {
        client.workingcontracts.forEach(contract => {
            contract.fromdate = contract.fromdate ? new Date(contract.fromdate) : undefined;
            contract.todate = contract.todate ? new Date(contract.todate) : undefined;
        });
    }

    return client;
}

export const searchClients = async (searchTerm: string): Promise<ClientQuery[]> => {
    return (await fetchAPI<ClientQuery[]>(`${apiUrl}Client/SearchClients?SearchTerm=${searchTerm}`)).ReturnObject!;
}

export const fetchClientFullname = async (clientId: string): Promise<GetClientFullnameDto> => {
    return (await (fetchAPI<GetClientFullnameDto>(`${apiUrl}Client/GetClientFullname/${clientId}`))).ReturnObject!;
}

export const fetchBenefitForms = async (): Promise<BenefitForm[]> => {
    return (await (fetchAPI<BenefitForm[]>(`${apiUrl}BenefitForm`))).ReturnObject!;
}

export const fetchDiagnosis = async (): Promise<Diagnosis[]> => {
    return (await (fetchAPI<Diagnosis[]>(`${apiUrl}Diagnosis`))).ReturnObject!;
}

export const fetchMaritalStatuses = async (): Promise<MaritalStatus[]> => {
    return (await (fetchAPI<MaritalStatus[]>(`${apiUrl}MaritalStatus`))).ReturnObject!;
}

export const fetchDriversLicences = async (): Promise<DriversLicence[]> => {
    return (await fetchAPI<DriversLicence[]>(`${apiUrl}DriversLicence`)).ReturnObject!;
}

export const fetchOrganizations = async (): Promise<Organization[]> => {
    return (await fetchAPI<Organization[]>(`${apiUrl}Organization`)).ReturnObject!;
}

export const deactivateClient = async (clientId: number): Promise<ClientQuery> => {
    return (await fetchAPI<ClientQuery>(`${apiUrl}Client/DeactivateClient`, 'PUT', { id: clientId })).ReturnObject!;
}
    
moment.prototype.toJSON = function(){
    return moment(this).format(DATE_FORMAT_JSON);
}

Date.prototype.toJSON = function(){
    return moment(this).format(DATE_FORMAT_JSON);
}

export const saveClient = async (client: Client): Promise<ApiResult<Client>> => {
    let method = client.id > 0  ? 'PUT' : 'POST';

    return await fetchAPI(`${apiUrl}Client`, method, client);     
}

export const fetchOrganization = async (organizationId: string): Promise<Organization> => {
    return (await fetchAPI<Organization>(`${apiUrl}organization/${organizationId}`)).ReturnObject!;
}

export const fetchOrganizationEditor = async (organizationId: string): Promise<Organization> => {
    return (await fetchAPI<Organization>(`${apiUrl}Organization/${organizationId}`)).ReturnObject!;
}

export const saveOrganization = async (organization: Organization): Promise<ApiResult<Organization>> => {
    let method = organization.id > 0  ? 'PUT' : 'POST';

    return await fetchAPI(`${apiUrl}Organization`, method, organization);
}

export const fetchCoachingProgramsByClient = async (clientId: string): Promise<CoachingProgramQuery[]> => {
    return (await fetchAPI<CoachingProgramQuery[]>(`${apiUrl}CoachingProgram/GetCoachingProgramsByClient/${clientId}`)).ReturnObject!;
}

export const fetchCoachingProgram = async (id: number): Promise<CoachingProgram> => {
    return (await (fetchAPI<CoachingProgram>(`${apiUrl}CoachingProgram/${id}`))).ReturnObject!;
}

export const fetchCoachingProgramEdit = async (id: string): Promise<CoachingProgramEdit> => {
    return (await (fetchAPI<CoachingProgramEdit>(`${apiUrl}CoachingProgram/GetCoachingProgramsEdit/${id}`))).ReturnObject!;
}

export const fetchCoachingProgramTypes = async (): Promise<GetCoachingProgramTypesDto[]> => {
    return (await (fetchAPI<GetCoachingProgramTypesDto[]>(`${apiUrl}CoachingProgram/GetCoachingProgramTypes`))).ReturnObject!;
}

export const saveCoachingProgram = async (coachingProgram: CoachingProgramEdit): Promise<ApiResult<CoachingProgramEdit>> => {
    let method = coachingProgram.id > 0  ? 'PUT' : 'POST';

    return await fetchAPI(`${apiUrl}CoachingProgram`, method, coachingProgram);
}

export const requestResetPassword = async (requestResetPasswordCommand: RequestResetPasswordCommand): Promise<ApiResult<RequestResetPasswordCommandDto>> => {
    let method = 'POST';

    const requestOptions: RequestInit = {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(requestResetPasswordCommand)
    };

    const response = await fetch(`${apiUrl}Authentication/RequestResetPassword`, requestOptions);     
    
    return handleApiResonse<RequestResetPasswordCommandDto>(response);
}

export const resetPassword = async (resetPasswordCommand: ResetPasswordCommand): Promise<ApiResult<ResetPasswordCommandDto>> => {
    let method = 'POST';

    const requestOptions: RequestInit = {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(resetPasswordCommand)
    };

    const response = await fetch(`${apiUrl}Authentication/ResetPassword`, requestOptions);
    
    return handleApiResonse<ResetPasswordCommandDto>(response);
}

export const searchUsers = async (searchTerm: string): Promise<SearchUserQueryDto[]> => {
    return (await fetchAPI<SearchUserQueryDto[]>(`${apiUrl}User/SearchUsers?SearchTerm=${searchTerm}`)).ReturnObject!;
}

export const createUser = async (user: CreateUserCommand): Promise<ApiResult<CreateUserCommandDto>> => {
    return await fetchAPI(`${apiUrl}User`, 'POST', user, true);
}

export const fetchUserRoles = async (): Promise<GetUserRolesDto[]> => {
    return (await (fetchAPI<GetUserRolesDto[]>(`${apiUrl}User/GetAvailableUserRoles`))).ReturnObject!;
}

function processErrors(errors: { [key: string]: string[] }): string[] {
    const allErrors: string[] = Object.values(errors).flat();

    return allErrors;
}