import './search-form.css';

import React, { useState } from "react";
import { Button } from './button';
import { InputField } from './input-field';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch } from "@fortawesome/free-solid-svg-icons";

interface SearchFormProps {
    onSearchSubmit: (searchTerm: string) => void;
  }

const SearchForm: React.FC<SearchFormProps> = ({ onSearchSubmit }) => {
    const [searchTerm, setSearchTerm] = useState<string>('');
  
    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
      const newSearchTerm = e.target.value;
        setSearchTerm(newSearchTerm);

        onSearchSubmit(newSearchTerm);
      };  
  
      return (
        <form>
          <div className="relative">
            <InputField className='my-2' inputFieldType={{type:'text'}} required={false} placeholder='Zoeken' value={searchTerm} onChange={handleInputChange} />
            <div className="absolute inset-y-0 right-2 pl-3 
                    flex items-center
                    pointer-events-none">
              <FontAwesomeIcon icon={faSearch} className="fa fa-lg my-auto cursor-pointer search-icon" />
            </div>
          </div>
        </form>
      );
  };
  
  export default SearchForm;