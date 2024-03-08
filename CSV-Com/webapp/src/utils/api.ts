import Client from "../types/model/Client";

const apiUrl = process.env.REACT_APP_API_URL;

async function fetchAPI<T>(url: string): Promise<T> {
    const response = await fetch(url);
    
    if (!response.ok) {
        throw new Error('Netwerkrespons was not ok.');
    }
    
    return response.json() as Promise<T>;
}

export const fetchClient = async (clientId: string): Promise<Client> => {
    return fetchAPI<Client>(`${apiUrl}Client/${clientId}`);
}

export const searchClients = async (searchTerm: string): Promise<Client[]> => {
    return fetchAPI<Client[]>(`${apiUrl}Client/SearchClients?SearchTerm=${searchTerm}`);
}