import Client from "../types/model/Client";
import BenefitForm from "../types/model/BenefitForm";
import Diagnosis from "../types/model/Diagnosis";
import MaritalStatus from "../types/model/MaritalStatus";
import DriversLicence from "../types/model/DriversLicence";

const apiUrl = 'https://localhost:32768/api/';

async function fetchAPI<T>(url: string): Promise<T> {
    const response = await fetch(url);
    
    if (!response.ok) {
        throw new Error('Netwerkrespons was niet ok.');
    }
    
    return response.json() as Promise<T>;
}

export const fetchClient = async (clientId: string): Promise<Client> => {
    return fetchAPI<Client>(`${apiUrl}Client/${clientId}`);
}

export const searchClients = async (searchTerm: string): Promise<Client[]> => {
    return fetchAPI<Client[]>(`${apiUrl}Client/SearchClients?SearchTerm=${searchTerm}`);
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

export const saveClient = async (client: Client): Promise<void> => {
    const requestOptions: RequestInit = {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(client)
    };

    const response = await fetch(`${apiUrl}Client`, requestOptions);

    if (!response.ok) {
        // TODO: errors doorgeven naar pagina
        throw new Error('Er is een fout opgetreden bij het opslaan van de cliënt.');
    }
}