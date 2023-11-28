import React, { useState, useRef, useEffect} from 'react';
import './button.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faXmark, faTimes  } from '@fortawesome/free-solid-svg-icons'
import { Button } from './button';
 
function PopUp() {
  const [isOpen, setIsOpen] = useState(false);
  const popupRef = useRef(null);


  useEffect(() => {
    function handleClickOutside(event) {
      if (popupRef.current && !popupRef.current.contains(event.target)) {
        setIsOpen(false);
      }
    }

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);


  return (
    <div className="rounded-lg h-40 w-80">
      
      <button className="w-64 text-lg py-3  bg-mainBlue rounded-lg text-white"  
      onClick={() =>setIsOpen(!isOpen)}>
       Deactiveren cliënt
      </button>
 
      {isOpen && (
        <div className="fixed top-0 left-0 w-full h-full bg-slate-900 bg-opacity-25 flex justify-center items-center z-50">
        <div className=" fixed top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2"ref={popupRef}>
        <div className="rounded-tl-2xl rounded-tr-2xl rounded-br-2xl rounded-bl-2xl drop-shadow-2xl bg-white  rounded-lg h-48  ">
        <div className="flex justify-between ml-2">
        <p className=" mr-2	ml-5  text-lg mt-5 ">Weet u zeker dat u de cliënt <br />  wilt deactiveren?</p>
        
            <button className="mr-3 ml-10 h-10 text-lg  justify-end" onClick={() => setIsOpen(false)}>
          <FontAwesomeIcon icon={faXmark} className="text-lg " />
        </button>
        {/* <div className="rounded-tl-3xl rounded-tr-3xl rounded-br-3xl rounded-bl-3xl">
        </div> */}

        
        </div>
         <div className="space-x-9 ml-6 py-1 ms-6 mr-5 flex justify-start mt-12 h-[3rem]  ">
            <button className="w-48 text-lg  border-4 border-mainBlue bg-mainBlue rounded-lg text-white font-medium">Deactiveren</button>
            <button className="w-48 text-lg  border-4 border-mainBlue bg-white rounded-lg text-mainBlue font-medium"onClick={() => setIsOpen(false)}>Annuleren</button>
         </div>
       </div>
       </div>
       
       </div>
       )}

       </div>

  );
}
 
export default PopUp;