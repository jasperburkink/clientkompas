import Client from "../types/model/Client";

const apiUrl = 'https://localhost:32772/api/';

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