import ClientQuery from "types/model/ClientQuery";
import BenefitForm from "types/model/BenefitForm";
import Diagnosis from "types/model/Diagnosis";
import MaritalStatus from "types/model/MaritalStatus";
import DriversLicence from "types/model/DriversLicence";
import Client from "types/model/Client";
import moment from 'moment';
import { isNullOrEmpty } from "./utilities";
import ApiResult from "types/common/api-result";
import { error } from "console";
import Organization from "types/model/Organization";

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

    const requestOptions: RequestInit = {
        method: method,
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(client)
    };

    const response = await fetch(`${apiUrl}Client`, requestOptions);     
    
    if(!response.ok){
        try {
            let {title, errors} = await response.json();
        
            return {
                Ok: response.ok,
                Errors: errors
            }
        }
        catch (err) {
            console.log(`An error has occured while reading errors from API while saving a client. Error:${err}.`);
        }

        return {
            Ok: response.ok,
            Errors: [response.statusText]
        }
    }

    let clientReturn: Client = await response.json();

    return {
        Ok: response.ok,
        ReturnObject: clientReturn
    }
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
    
    if(!response.ok){
        try {
            let {title, errors} = await response.json();
        
            return {
                Ok: response.ok,
                Errors: errors
            }
        }
        catch (err) {
            console.log(`An error has occured while reading errors from API while saving an organization. Error:${err}.`);
        }

        return {
            Ok: response.ok,
            Errors: [response.statusText]
        }
    }

    let organizationReturn: Organization = await response.json();

    return {
        Ok: response.ok,
        ReturnObject: organizationReturn
    }
}