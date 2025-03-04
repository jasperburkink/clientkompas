import React, {useState} from 'react';
import ResultItem  from '../../types/common/ResultItem';
import UserEditor from './user-editor';

export const UserContext = React.createContext<IUserContext>({allUsers: [], setAllUsers: (x) => null});

export enum UserRoute {
    VIEW_USER,
    EDIT_USER
  }

export interface IUserContext {
    allUsers: ResultItem[];
    setAllUsers: (x: ResultItem[]) => void;
}

export interface UserContextWrapperProps {
    userRoute: UserRoute;
}

export const UserContextWrapper = (props: UserContextWrapperProps) => {
    const [allUsers, setAllUsers] = useState<ResultItem[]>([]);

    const userContext: IUserContext = {
        allUsers: allUsers, 
        setAllUsers: setAllUsers,
    }

    return (
        <UserContext.Provider value={userContext}>
            { props.userRoute === UserRoute.VIEW_USER && < />}
            { props.userRoute === UserRoute.EDIT_USER && <UserEditor />}
        </UserContext.Provider>
    );
  };