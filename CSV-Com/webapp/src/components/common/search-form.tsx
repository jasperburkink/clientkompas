import './search-form.css';

import React, { useState } from "react";
import { Button } from './button';
import { InputField } from './input-field';

interface SearchFormProps {
    onSearchSubmit: (searchTerm: string) => void;
  }

const SearchForm: React.FC<SearchFormProps> = ({ onSearchSubmit }) => {
    const [searchTerm, setSearchTerm] = useState<string>('');
  
    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setSearchTerm(e.target.value);
      };
  
      const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        onSearchSubmit(searchTerm);
      };
  
      return (
        <form onSubmit={handleSubmit}>
            <InputField placeholder='Zoeken' />
          <input type="text" value={searchTerm} onChange={handleInputChange} />
          <button type="submit">Search</button>
        </form>
      );
  };
  
  export default SearchForm;