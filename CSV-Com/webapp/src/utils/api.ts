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
import CoachingProgramEdit from "types/model/CoachingProgramEdit";

const apiUrl = process.env.REACT_APP_API_URL;

async function fetchAPI<T>(url: string, method: string = 'GET', body?: any): Promise<T> {
    const options: RequestInit = {
        method,
        headers: {
          'Content-Type': 'application/json'
        },
        body: body ? JSON.stringify(body) : undefined,
      };
    
    const response = await fetch(url, options);
    
    if (!response.ok) {
        throw new Error(`An error has occured while executing an API operation to '${url}'.`);
    }
    
    return response.json() as Promise<T>;
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
    if(response.ok){
        let ObjectReturn: T = await response.json();

        return {
            Ok: response.ok,
            ReturnObject: ObjectReturn
        }
    }

    // Error
    try {
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
    catch (err) {
        console.log(`Error while parsing api response. Error:${err}`);
        return {
            Ok: response.ok,
            Errors: [response.statusText]
        }
    }
}

export const fetchClient = async (clientId: string): Promise<ClientQuery> => {
    return fetchAPI<ClientQuery>(`${apiUrl}Client/${clientId}`);
}

export const fetchClientEditor = async (clientId: string): Promise<Client> => {
    let client = await fetchAPI<Client>(`${apiUrl}Client/GetClientEditor/${clientId}`);

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
    return fetchAPI<ClientQuery[]>(`${apiUrl}Client/SearchClients?SearchTerm=${searchTerm}`);
}

export const fetchBenefitForms = async (): Promise<BenefitForm[]> => {
    return fetchAPI<BenefitForm[]>(`${apiUrl}BenefitForm`);
}

export const fetchDiagnosis = async (): Promise<Diagnosis[]> => {
    return fetchAPI<Diagnosis[]>(`${apiUrl}Diagnosis`);
}

export const fetchMaritalStatuses = async (): Promise<MaritalStatus[]> => {
    return fetchAPI<MaritalStatus[]>(`${apiUrl}MaritalStatus`);
}

export const fetchDriversLicences = async (): Promise<DriversLicence[]> => {
    return fetchAPI<DriversLicence[]>(`${apiUrl}DriversLicence`);
}

export const fetchOrganizations = async (): Promise<Organization[]> => {
    return fetchAPI<Organization[]>(`${apiUrl}Organization`);
}

export const deactivateClient = async (clientId: number): Promise<ClientQuery> => {
    return fetchAPI<ClientQuery>(`${apiUrl}Client/DeactivateClient`, 'PUT', { id: clientId });
}
    
moment.prototype.toJSON = function(){
    return moment(this).format(DATE_FORMAT_JSON);
}
Date.prototype.toJSON = function(){
    return moment(this).format(DATE_FORMAT_JSON);
}

export const saveClient = async (client: Client): Promise<ApiResult<Client>> => {
    let method = client.id > 0  ? 'PUT' : 'POST';

    console.log(JSON.stringify(client));


    const requestOptions: RequestInit = {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(client)
    };

    const response = await fetch(`${apiUrl}Client`, requestOptions);     

    return handleApiResonse<Client>(response);
}

export const fetchOrganization = async (organizationId: string): Promise<Organization> => {
    return fetchAPI<Organization>(`${apiUrl}organization/${organizationId}`);
}

export const fetchOrganizationEditor = async (organizationId: string): Promise<Organization> => {
    let organization = await fetchAPI<Organization>(`${apiUrl}Organization/${organizationId}`);
    return organization;
}

export const saveOrganization = async (organization: Organization): Promise<ApiResult<Organization>> => {
    let method = organization.id > 0  ? 'PUT' : 'POST';

    const requestOptions: RequestInit = {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(organization)
    };

    const response = await fetch(`${apiUrl}Organization`, requestOptions);     
    
    return handleApiResonse<Organization>(response);
}

export const fetchCoachingProgramsByClient = async (clientId: string): Promise<CoachingProgramQuery[]> => {
    return await fetchAPI<CoachingProgramQuery[]>(`${apiUrl}CoachingProgram/GetCoachingProgramsByClient/${clientId}`);
}

export const fetchCoachingProgram = async (id: number): Promise<CoachingProgram> => {
let tempco = await fetchAPI<CoachingProgram>(`${apiUrl}CoachingProgram/${id}`);

    return await fetchAPI<CoachingProgram>(`${apiUrl}CoachingProgram/${id}`);
}

export const saveCoachingProgram = async (coachingProgram: CoachingProgramEdit): Promise<ApiResult<CoachingProgramEdit>> => {
    let method = coachingProgram.id > 0  ? 'PUT' : 'POST';

    console.log(JSON.stringify(coachingProgram));


    const requestOptions: RequestInit = {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(coachingProgram)
    };

    const response = await fetch(`${apiUrl}CoachingProgram`, requestOptions);     

    return handleApiResonse<CoachingProgramEdit>(response);
}

function processErrors(errors: { [key: string]: string[] }): string[] {

    // Verzamel alle foutmeldingen in een enkele array
    const allErrors: string[] = Object.values(errors).flat();

    return allErrors;
}