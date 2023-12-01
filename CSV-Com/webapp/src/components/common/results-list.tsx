import './results-list.css';
import ResultItem  from '../../types/common/ResultItem';
import React from 'react';

interface ResultsListProps {
  results: ResultItem[];
}

const ResultsList: React.FC<ResultsListProps> = ({ results }) => {
  return (
    <ul>
      {results.map((result) => (
        <li key={result.id}>{result.name}</li>
      ))}
    </ul>
  );
};

export default ResultsList;