import React, { useState, useRef, useEffect} from 'react';
import './button.css';
import './popup.css'; 
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faXmark } from '@fortawesome/free-solid-svg-icons'
import PropTypes from 'prop-types';
import Buttonforpopup from './buttonforpopup';

interface PopUpProps {
  handleClick: () => void;
  handleCancelClick: () => void;
  cancelButtonText: string;
  confirmButtonText: string;
  insidePopUpText: string;
  text: string;
  buttonType: string
}

let string = "text" as const
const PopUp: React.FC<PopUpProps> = ({
  handleClick,
  handleCancelClick,
  cancelButtonText,
  confirmButtonText,
  insidePopUpText
}) => {

  const [isOpen, setIsOpen] = useState(false);
  const popupRef = useRef<HTMLDivElement>(null);

  const handleClickOutside = (event: MouseEvent) => {
    if (
      popupRef.current &&
      !popupRef.current.contains(event.target as Node) &&
      event.target instanceof HTMLElement &&
      !event.target.closest('.popup-buttonX')
    ) {
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
   <div className="grid place-items-center"> 

      <Buttonforpopup
        openbutton='placeholder'
        isOpen={isOpen} 
        setIsOpen={setIsOpen} 
        onClick={() => {
        console.log('Button clicked!');
        setIsOpen(!isOpen);
        }}
      />

      {isOpen && (
        <div>
        <div className ="popup-top">
          <div className ="popup-content" ref={popupRef} >
            <div className ="popup-inner">
              <div className ="popup-justify">
                 <p className ="popup-text ">{insidePopUpText}</p>
                 <button 
                  className ="popup-buttonX"
                  onClick={() => setIsOpen(false)}
                  >
                  <FontAwesomeIcon icon={faXmark} className ="icon-size" />
                 </button>
              </div>
                
              <div className ="popup-center">
                <button 
                  onClick = {() => {
                    handleClick();
                    setIsOpen(false);
                  }}
                  className ="popUp-btnLeft">
                  {confirmButtonText}
                </button>

                <button
                  onClick = {() => {
                    handleCancelClick();
                    setIsOpen(false);
                   }}
                  className="popUp-btnRight"
                >
                    {cancelButtonText}
                 </button>
               </div>
             </div>
          </div>
        </div>
        </div>
       )}
     </div>
  );
}
PopUp.propTypes = {
  handleClick: PropTypes.func.isRequired,
  handleCancelClick: PropTypes.func.isRequired,
  confirmButtonText: PropTypes.string.isRequired,
  cancelButtonText: PropTypes.string.isRequired,
  text: PropTypes.string.isRequired,
  buttonType: PropTypes.string.isRequired,
};
 
export default PopUp;
