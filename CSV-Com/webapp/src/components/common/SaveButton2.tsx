import React, { useState, useEffect } from 'react';
import Spinner from './Spinner';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheck, faTimes } from '@fortawesome/free-solid-svg-icons';

const SaveButton = () => {
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState(false);
  const [showText, setShowText] = useState(true);
  const [buttonStep, setButtonStep] = useState(0);

  const handleClick = () => {
    setLoading(true);
    setShowText(false);

    setTimeout(() => {
      const dataIsValid = true; 

      if (dataIsValid) {
        setLoading(false);
        setSuccess(true);
        setButtonStep(1);
        setTimeout(() => {
          setSuccess(false);
          setShowText(true);
          setButtonStep(0);
        }, 3000);
      } else {
        setLoading(false);
        setError(true);
        setButtonStep(2);
        setTimeout(() => {
          setError(false);
          setShowText(true);
          setButtonStep(0);
        }, 3500);
      }
    }, 1500);
  };

  return (
    <button
      onClick={handleClick}
      className={`error success ${error ? 'bg-red-500' : success ? 'bg-green-600' : 'bg-mainBlue'} w-48 h-12 rounded-lg`}
      disabled={loading}
      data-testid="button.safe"
    >
      {buttonStep === 0 && !loading && !success && !error && (
        <span style={{ color: 'white' }}>Opslaan 2</span>
      )}

      {buttonStep === 0 && loading && !success && !error && (
        <>
          <Spinner data-testid="spinner" />
          <span style={{ color: 'white' }}></span>
        </>
      )}

      {buttonStep === 1 && success && (
        <>
          <FontAwesomeIcon className='fa-lg' icon={faCheck} style={{ color: 'white' }}
            data-testid="success-icon"
          />
        </>
      )}

      {buttonStep === 1 && success && showText && (
        <span style={{ color: 'white' }}>Succes</span>
      )}

      {buttonStep === 2 && error && (
        <>
          <FontAwesomeIcon className='fa-lg' icon={faTimes} style={{ color: 'white' }} />
        </>
      )}

      {buttonStep === 2 && error && showText && (
        <span style={{ color: 'white' }}>Fout</span>
      )}
    </button>
  );
};

export default SaveButton;
