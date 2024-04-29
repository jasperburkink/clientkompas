import React, { useState, useEffect, useRef, useContext } from 'react';
import SearchForm from 'components/common/search-form';
import ResultItem  from 'types/common/ResultItem';
import ResultsList from 'components/common/results-list';
import { searchClients } from 'utils/api';
import ClientQuery, { getCompleteClientName } from 'types/model/ClientQuery';
import StatusEnum from 'types/common/StatusEnum';
import { ClientContext } from '../../pages/client-context';

const NO_RESULT_TEXT = 'Er zijn geen cliënten gevonden die aan de zoekcriteria voldoen.';
const TYPEING_TIMEOUT = 500;

const SearchClients: React.FC = () => {
  const clientContext = useContext(ClientContext);
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
      const clients = await searchClients(searchTerm);      
      setStatus(StatusEnum.SUCCESSFUL);
      
      if(!clients){
        return;
      }

      clientContext.setAllClients(clients.map((client) => (          
      {          
        id: client.id,
        name: getCompleteClientName(client)
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
      <ResultsList results={clientContext.allClients} noResultsText={NO_RESULT_TEXT} loading={status === StatusEnum.IDLE || status === StatusEnum.PENDING} />
    </div>
  );
};

export default SearchClients;