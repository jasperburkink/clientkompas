import React, { useState, useEffect } from 'react';
import SearchForm from '../common/search-form';
import ResultItem  from '../../types/common/ResultItem';
import ResultsList from '../common/results-list';
import { searchClients } from '../../utils/api';
import Client, { getCompleteClientName } from '../../types/model/Client';
import { Console } from 'console';

const NO_RESULT_TEXT = 'Er zijn geen cliënten gevonden die aan de zoekcriteria voldoen.';
const TYPEING_TIMEOUT = 500;

const SearchClients: React.FC = () => {
  const [searchResults, setSearchResults] = useState<ResultItem[]>([]);

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [debounceTimeout, setDebounceTimeout] = useState<NodeJS.Timeout | null>(null);

  const handleSearchChange = async (searchTerm: string) => {      
    
    // Cancel previous timeouts
    if (debounceTimeout) {
      clearTimeout(debounceTimeout);
    }

    // Start new timeout for debouncing (don't start api call until user stops typeing)
    const timeout = setTimeout(async () => {
      updateSearchResults(searchTerm);
    }, TYPEING_TIMEOUT);

    setDebounceTimeout(timeout);
  };

  const updateSearchResults = async (searchTerm: string) => {
    try{
      setLoading(true);
      const clients = await searchClients(searchTerm);

      if(!clients){
        return;
      }

      setSearchResults(clients.map((client) => (          
      {          
        id: client.identificationnumber,
        name: getCompleteClientName(client)
      })));
    }
    catch(err) {
      // TODO: handle error
      setError(null);
    }
    finally{
      setLoading(false);
    }
  };

  useEffect(() => {
    updateSearchResults('');
  }, []);

  return (
    <div>
      <SearchForm onSearchChange={handleSearchChange} />
      <ResultsList results={searchResults} noResultsText={NO_RESULT_TEXT} loading={loading} />
    </div>
  );
};

export default SearchClients;