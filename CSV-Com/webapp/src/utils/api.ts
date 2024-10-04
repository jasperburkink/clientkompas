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

const apiUrl = process.env.REACT_APP_API_URL;

async function fetchAPI<T>(url: string, method: string = 'GET', body?: any): Promise<ApiResult<T>> {
    const bearerTokenJson = sessionStorage.getItem('token');    
    const refreshToken = localStorage.getItem('refreshToken');
    let bearerToken: BearerToken | null = null;

    // Token expired?
    if(bearerTokenJson){
        bearerToken = BearerToken.deserialize(bearerTokenJson);

        if(bearerToken.isExpired()){
            const newToken = await refreshAccessToken();

            if(!newToken) {
                return Promise.reject({
                    Ok: false,
                    Errors: ['Unauthorized access']
                });
            }
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
    
    return handleApiResonse<T>(response);
}

//TODO: move this to global file
const DATE_FORMAT_JSON = 'yyyy-MM-DD';
    
moment.prototype.toJSON = function(){
    return moment(this).format(DATE_FORMAT_JSON);
}
Date.prototype.toJSON = function(){
    return moment(this).format(DATE_FORMAT_JSON);
}

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

async function refreshAccessToken(): Promise<string | null> {
    const refreshToken = localStorage.getItem('refreshToken');
    if (!refreshToken) {
        window.location.href = '/unauthorized';
        return null;
    }

    const response = await fetch(`${apiUrl}/Authentication/refresh`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ refreshToken })
    });

    if (response.ok) {
        const data = await response.json();
        
        sessionStorage.setItem('token', data.accessToken);
        localStorage.setItem('refreshToken', data.refreshToken);
        return data.accessToken;
    } else {
        window.location.href = '/unauthorized';
        return null;
    }
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
    return (await fetchAPI<CoachingProgram>(`${apiUrl}CoachingProgram/${id}`)).ReturnObject!;
}

function processErrors(errors: { [key: string]: string[] }): string[] {

    // Verzamel alle foutmeldingen in een enkele array
    const allErrors: string[] = Object.values(errors).flat();

    return allErrors;
}