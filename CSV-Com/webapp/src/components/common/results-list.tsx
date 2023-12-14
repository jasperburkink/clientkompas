import './results-list.css';
import ResultItem  from '../../types/common/ResultItem';
import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import { LinkButton } from '../common/link-button'

interface ResultsListProps {
  results: ResultItem[];
  noResultsText: string;
  loading: boolean;
}

const ResultsList: React.FC<ResultsListProps> = (props) => {
  
    if(props.loading){
      return (
        <div className='spinner'>
          <FontAwesomeIcon icon={faSpinner} className="fa fa-2x fa-refresh fa-spin" />
        </div>
      );
    }

    if(!props.results || props.results.length <= 0){
      return (
        <div>{props.noResultsText}</div>
      );
    }
    
    return (
    <ul>
      {props.results.map((result) => (
        <li className='result-item' key={result.id}><a href={'../clients/' + result.id}>{result.name}</a></li>
      ))}
    </ul>
  );
};

export default ResultsList;