import React, { useState, useEffect } from 'react';
import Spinner from './Spinner';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheck, faTimes } from '@fortawesome/free-solid-svg-icons';

const SUCCESS_TIMEOUT = 3000;
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
        }, SUCCESS_TIMEOUT);
      } 
      else {
        setLoading(false);
        setError(true);
        setButtonStep(2);
        setTimeout(() => {
          setError(false);
          setShowText(true);
          setButtonStep(0);
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
      {buttonStep === 0 && !loading && !success && !error && (
        <span className="actionTrigger">{buttonText}</span>
      )}

      {buttonStep === 0 && loading && !success && !error && (
        <>
          <Spinner data-testid="spinner" />
          <span className="actionTrigger"></span>
        </>
      )}

      {buttonStep === 1 && success && (
        <>
          <FontAwesomeIcon className='icon' icon={faCheck}
          data-testid="icon"
          />
        </>
      )}

      {buttonStep === 2 && error && (
        <>
          <FontAwesomeIcon className='icon' icon={faTimes} />
        </>
      )}

      {buttonStep === 2 && error && showText && (
     <span className="actionTrigger">
     {loading ? loadingText : success ? successText : errorText}
   </span>    
         )}
    </button>
  );
};

export default SaveButton;
