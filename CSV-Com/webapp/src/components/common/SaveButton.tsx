import React, { useState } from 'react';
import Spinner from './Spinner';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheck, faTimes } from '@fortawesome/free-solid-svg-icons';

const SUCCESS_TIMEOUT = 2000;
const ERROR_TIMEOUT = 4000;
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
    <div>
    

      <button
        onClick={handleClick}
        className={`before button ${error ? 'error' : success ? 'succes-stil' : 'default'}`}
        disabled={loading}
        data-testid="button.safe"
      >
        {loading && <Spinner data-testid="spinner"/>}
        {success && (
          <FontAwesomeIcon
            className='icon'
            icon={faCheck}
            data-testid="icon"
          />
        )}
        {error && (
          <FontAwesomeIcon
            className='icon'
            icon={faTimes}
          />
        )}
        {showText && !loading && !success && !error && (
          <span className="actionTrigger">
            {loading ? loadingText : success ? successText : buttonText}
          </span>
        )}
      </button>
    </div>
  );
};

export default SaveButton;

