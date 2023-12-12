import React, { useState } from 'react';
import Spinner from './Spinner';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheck, faXmark } from '@fortawesome/free-solid-svg-icons'; 

const SaveButton = () => {
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState(false);
  const [showText, setShowText] = useState(true); 

  const handleClick = () => {
    setLoading(true);
    setShowText(false);

    setTimeout(() => {
      const dataIsValid = true; 

      if (dataIsValid) {
        setLoading(false);
        setSuccess(true);
        setTimeout(() => {
          setSuccess(false);
          setShowText(true); 
        }, 2000); 
      } else {
        setLoading(false);
        setError(true);
        setTimeout(() => {
          setError(false);
          setShowText(true);
        }, 3500); 
      }
    }, 1500); 
  };

  return (
    <button
      onClick={handleClick}
      className={`error success ${error ? 'bg-red-500' : success ? 'bg-green-500' : 'bg-mainBlue'} w-48 h-12 rounded-lg`}
      disabled={loading}
      data-testid="button.safe" 
    >
        {!loading && !success && !error && (
            <span style={{ color: 'white' }}>Opslaan 3</span>
        )}

        {loading && !success && !error && (
            <>
            <Spinner data-testid="spinner"/>
            <span style={{ color: 'white' }}>Laden</span>
            </>
        )}

        {success && (
        <>
        <FontAwesomeIcon className='fa-lg' icon={faCheck} style={{ color: 'white' }} data-testid="success-icon" />
        <span style={{ color: 'white' }}>Succes</span>
        </>
        )}

        {error && (
            <>
            <FontAwesomeIcon className='fa-lg' icon={faXmark} style={{ color: 'white' }}/>
            <span style={{ color: 'white' }}>Fout</span>
            </>
        )}

    </button>
  );
}; 

export default SaveButton;
