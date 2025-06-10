import React, { useState, useEffect, useRef, useContext } from 'react';
import SearchForm from 'components/common/search-form';
import ResultsList from 'components/common/results-list';
import { searchUsers } from 'utils/api';
import StatusEnum from 'types/common/StatusEnum';
import { UserContext } from '../../pages/user-context';

const NO_RESULT_TEXT = 'Er zijn geen gebruikers gevonden die aan de zoekcriteria voldoen.';
const TYPEING_TIMEOUT = 500;

const Searchusers: React.FC = () => {
  const userContext = useContext(UserContext);
  const [error, setError] = useState<string|null>(null);      
  const [status, setStatus] = useState(StatusEnum.IDLE);

  const debounceTimeoutRef = useRef<NodeJS.Timeout | null>(null);

  const handleSearchChange = async (searchTerm: string) => {
    // Cancel previous timeouts
    if (debounceTimeoutRef.current) {
      clearTimeout(debounceTimeoutRef.current);
    }

    // Start new timeout for debouncing (don't start api call until user stops typeing)
    const timeout = setTimeout(async () => {
      updateSearchResults(searchTerm);
    }, TYPEING_TIMEOUT);

    debounceTimeoutRef.current = timeout;
  };

  const updateSearchResults = async (searchTerm: string) => {
    try{
      setStatus(StatusEnum.PENDING);
      const users = await searchUsers(searchTerm);
      setStatus(StatusEnum.SUCCESSFUL);
      
      if(!users){
        return;
      }

      userContext.setAllUsers(users.map((user) => (          
      {          
        id: user.id,
        name: user.fullname,        
        isdeactivated: user.deactivateddatetime !== null
      })));      
    }
    catch(err) {
      setStatus(StatusEnum.REJECTED);

      // TODO: handle error
      setError(null);
    }
  };

  useEffect(() => {
    return () => {
      if (debounceTimeoutRef.current) {
        clearTimeout(debounceTimeoutRef.current);
      }
    };
  }, []);

  useEffect(() => {
    updateSearchResults('');
  }, []);

  return (
    <div>
      <SearchForm onSearchChange={handleSearchChange} />
      <ResultsList href='users' results={userContext.allUsers} noResultsText={NO_RESULT_TEXT} loading={status === StatusEnum.IDLE || status === StatusEnum.PENDING} />
    </div>
  );
};

export default Searchusers;