import React from "react";
import './PopUp.css';
import { Button } from './button';

interface ButtonForPopupProps {
  isOpen: boolean;
  openbutton: string;
  setIsOpen: React.Dispatch<React.SetStateAction<boolean>>;
}

const ButtonForPopup: React.FC<ButtonForPopupProps> = ({ isOpen, setIsOpen }) => {

  const handleButtonClick = () => {
    setIsOpen(!isOpen);
    alert('Button1 clicked!');
  };

  return (
    <div>
      <Button
        onClick={handleButtonClick}
        buttonType={{ type: "Solid" }}
        text="placeholder popup"
        className='w-200px h-50px'
      />
    </div>
  );
};

export default ButtonForPopup;
