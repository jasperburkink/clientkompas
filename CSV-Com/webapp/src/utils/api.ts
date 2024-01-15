import Client from "../types/model/Client";

const apiUrl = 'https://localhost:7017/api/';

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