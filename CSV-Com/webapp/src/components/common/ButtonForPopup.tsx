import React from "react";
import { Button } from './button';
import './PopUp.css'; 

interface ButtonForPopupProps {
  isOpen: boolean;
  setIsOpen: React.Dispatch<React.SetStateAction<boolean>>;
}
const ButtonForPopup: React.FC<ButtonForPopupProps> = ({ isOpen, setIsOpen }) => {

  return (
    <div>
      <button
        className="btn-first"
        onClick={() => setIsOpen(!isOpen)}
      >
        Deactiveren cliënt
      </button>
    </div>
  );
};

export default ButtonForPopup;

