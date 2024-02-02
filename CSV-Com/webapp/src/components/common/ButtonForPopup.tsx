import React from "react";
import './popup.css';
import { Button } from './button';

interface buttonforpopupProps {
  isOpen: boolean;
  openbutton: string;
  setIsOpen: React.Dispatch<React.SetStateAction<boolean>>;
  onClick: () => void;
}

const buttonforpopup: React.FC<buttonforpopupProps> = ({ isOpen, setIsOpen, onClick}) => {

  const handleButtonClick = () => {
    setIsOpen(!isOpen);
    onClick();
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

export default buttonforpopup;
