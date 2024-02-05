import React, { useState, useRef, useEffect} from 'react';
import './button.css';
import './PopUp.css'; 
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faXmark, faTimes  } from '@fortawesome/free-solid-svg-icons'
import { Button } from './button.tsx';
import PropTypes from 'prop-types';
import ButtonForPopup from './ButtonForPopup';

const PopUp = ({ handleClick, handleCancelClick, buttonText, text }) => {
  const [isOpen, setIsOpen] = useState(false);
  const popupRef = useRef(null);

  const handleToggle = () => {
    setIsOpen(prevState => !prevState);
  };

  const handleClickOutside = (event) => {
    if (popupRef.current && !popupRef.current.contains(event.target)) {
      setIsOpen(false);
    }
  };

  useEffect(() => {
    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  return (
    <div className="rounded-lg h-40 w-80">
      
      <ButtonForPopup isOpen={isOpen} setIsOpen={setIsOpen} />
      
      {isOpen && (
        <div className ="popup-top">
          <div className ="popup-content" ref={popupRef} >
          <div className ="popup-inner">
              <div className ="popup-justify">
              <p className ="popup-text ">Weet u zeker dat u de cliënt <br />  wilt deactiveren?</p>
              
              <button 
                 className = "popup-buttonX"
                 onClick={() => setIsOpen(false)}>
                <FontAwesomeIcon icon={faXmark} className ="text-lg " />
              </button>
              </div>
                
              <div className="popup-center">
              <button 
                onClick = {handleClick} className ="w-48 text-lg border-4 border-mainBlue bg-mainBlue rounded-lg text-white font-medium">
                {buttonText}
              </button>

              <button 
                onClick = {handleCancelClick} className="w-48 text-lg border-4 border-mainBlue bg-white rounded-lg text-mainBlue font-medium"
                onClick = {() => setIsOpen(false)}>
                {text}
              </button>

            </div>
          </div>
        </div>
       </div>
       )}
     </div>
  );
}
 
export default PopUp;
