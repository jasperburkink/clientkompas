import React, {useState} from 'react';
import ResultItem  from './../types/common/ResultItem';
import OrganizationDetails from './organization-details';

export const OrganizationContext = React.createContext<IOrganizationContext>({allOrganization: [], setAllOrganizations: (x) => null});

export interface IOrganizationContext {
    allOrganization: ResultItem[];
    setAllOrganizations: (x: ResultItem[]) => void;
}

export interface OrganizationContextWrapperProps {
    organizationRoute: OrganizationRoute;
}

export const OrganizationContextWrapper = (props: OrganizationContextWrapperProps) => {
    const [allOrganizations, setAllOrganizations] = useState<ResultItem[]>([]);

    const organizationContext: IOrganizationContext = {
        allOrganization: allOrganizations, 
        setAllOrganizations: setAllOrganizations,
    }

    return (
        <OrganizationContext.Provider value={organizationContext}>
            { props.organizationRoute === OrganizationRoute.VIEW_ORGANIZATION && <OrganizationDetails />}
            { props.organizationRoute === OrganizationRoute.EDIT_ORGANIZATION && <OrganizationDetails />}
        </OrganizationContext.Provider>
    );
  };

  export enum OrganizationRoute {
    VIEW_ORGANIZATION,
    EDIT_ORGANIZATION
  }