import React, { useState } from 'react';
import Spinner from './spinner-button';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheck, faXmark } from '@fortawesome/free-solid-svg-icons'; 

const SUCCESS_TIMEOUT = 2000;
const ERROR_TIMEOUT = 3500;
const LOADING_TIMEOUT = 1500;

interface SaveButtonProps {
  buttonText: string;
  loadingText: string;
  successText: string;
  errorText: string;
}

const SaveButton: React.FC<SaveButtonProps> = ({
  buttonText,
  loadingText,
  successText,
  errorText,
}) => {
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
        }, SUCCESS_TIMEOUT); 
      } 
      else {
        setLoading(false);
        setError(true);
        setTimeout(() => {
          setError(false);
          setShowText(true);
        }, ERROR_TIMEOUT); 
      }
    }, LOADING_TIMEOUT); 
  };

  return (
    <button
      onClick={handleClick}
      className={`before button ${error ? 'error' : success ? 'success' : 'default'}`}

      disabled={loading}
      data-testid="button.safe" 
    >
        {!loading && !success && !error && (
            <span className="actionTrigger">{buttonText}</span>
        )}

        {loading && !success && !error && (
            <>
            <Spinner data-testid="spinner"/>
            <span className="actionTrigger">{loadingText}</span>
            </>
        )}

        {success && (
        <>
        <FontAwesomeIcon className='icon' icon={faCheck} data-testid="succes-icon" />
        <span className="actionTrigger">{successText}</span>
        </>
        )}

        {error && (
            <>
            <FontAwesomeIcon className='icon' icon={faXmark} />
            <span className="actionTrigger">
            {loading ? loadingText : success ? successText : errorText}
          </span>            
          </>
        )}

    </button>
  );
}; 

export default SaveButton;
