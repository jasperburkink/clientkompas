import React, { useState } from 'react';
import Spinner from './Spinner';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCheck, faXmark } from '@fortawesome/free-solid-svg-icons'; 
import SaveButton2 from './SaveButton2';
import SaveButton3 from './SaveButton3';

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
        }, 4000); 
      }
    }, 1500); 
  };

  return (
  <div>
    <div className='w-10'>
       <SaveButton3/>
       <SaveButton2/>      
      </div>

    <button
      onClick={handleClick}
      className={`error success ${error ? 'bg-red-500' : success ? 'bg-mainBlue' : 'bg-mainBlue'} w-48 h-12 rounded-lg`}
      disabled={loading}
      data-testid="button.safe" 
    >
      {loading && <Spinner data-testid="spinner"/>}

      {success && <FontAwesomeIcon className='fa-lg'
      icon={faCheck} style={{ color: 'white' }}
      data-testid="success-icon" />}

      {error && <FontAwesomeIcon className='fa-lg'
      icon={faXmark} style={{color: 'white'}}/>}

      {showText && !loading && !success && !error && (
      <span style={{ color: 'white' }}>Opslaan</span>)}  
    </button>

    </div>
  );
}; 

export default SaveButton;