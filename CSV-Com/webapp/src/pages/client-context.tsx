import React, {useState} from 'react';
import Clients from './Clients';
import ResultItem  from './../types/common/ResultItem';
import ClientEditor from './client-editor';
import CoachingProgramEditor from './coachingprogram-editor';

export const ClientContext = React.createContext<IClientContext>({allClients: [], setAllClients: (x) => null});

export interface IClientContext {
    allClients: ResultItem[];
    setAllClients: (x: ResultItem[]) => void;
}

export interface ClientContextWrapperProps {
    clientRoute: ClientRoute;
}

export const ClientContextWrapper = (props: ClientContextWrapperProps) => {
    const [allClients, setAllClients] = useState<ResultItem[]>([]);

    const clientContext: IClientContext = {
        allClients, 
        setAllClients,
    }

    return (
        <ClientContext.Provider value={clientContext}>
            { props.clientRoute === ClientRoute.VIEW_CLIENT && <Clients />}
            { props.clientRoute === ClientRoute.EDIT_CLIENT && <ClientEditor />}
            { props.clientRoute === ClientRoute.EDIT_CLIENT_COACHINGPROGRAM && <CoachingProgramEditor />}
        </ClientContext.Provider>
    );
  };

  export enum ClientRoute {
    VIEW_CLIENT,
    EDIT_CLIENT,
    EDIT_CLIENT_COACHINGPROGRAM,
  }