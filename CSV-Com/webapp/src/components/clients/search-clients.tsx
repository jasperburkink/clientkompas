import React, { useState } from 'react';
import SearchForm from '../common/search-form';
import ResultItem  from '../../types/common/ResultItem';
import ResultsList from '../common/results-list';
import { searchClients } from '../../utils/api';

const SearchClients: React.FC = () => {
  const [searchResults, setSearchResults] = useState<ResultItem[]>([]);

  const handleSearchSubmit = (searchTerm: string) => {
    const fetchedClient: ResultItem = await searchClients(searchTerm!);
  };

  return (
    <div>
      <SearchForm onSearchSubmit={handleSearchSubmit} />
      <ResultsList results={searchResults} />
    </div>
  );
};

export default SearchClients;