import React from "react";
import { Button } from './button';

const ButtonForPopup = ({ isOpen, setIsOpen }) => {

  return (
    <div style={{ flex: 1 }}>
      <button
        className="w-64 text-lg py-3 bg-mainBlue rounded-lg text-white"
        onClick={() => setIsOpen(!isOpen)}
      >
        Deactiveren cliënt
      </button>
    </div>
  );
};

export default ButtonForPopup;

