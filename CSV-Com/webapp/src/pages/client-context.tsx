import React, {useState} from 'react';
import Clients from './Clients';
import ResultItem  from './../types/common/ResultItem';

export const ClientContext = React.createContext<IClientContext>({allClients: [], setAllClients: (x) => null});

export interface IClientContext {
    allClients: ResultItem[];
    setAllClients: (x: ResultItem[]) => void;
}

export const ClientContextWrapper = () => {
    const [allClients, setAllClients] = useState<ResultItem[]>([]);

    const clientContext: IClientContext = {
        allClients, 
        setAllClients,
    }

    return (
        <ClientContext.Provider value={clientContext}>
            <Clients />
        </ClientContext.Provider>
    );
  };