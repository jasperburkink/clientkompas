import React from 'react';
import './button.css';
import { ButtonType } from '../../types/common/ButtonComponentType';
import { getClassNameButtonType } from '../../types/common/ButtonComponentType';

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  text: string;
  buttonType: ButtonType;
  isOpen?: boolean; 
  setIsOpen?: React.Dispatch<React.SetStateAction<boolean>>;
}

export const Button = (props: ButtonProps) => {
  const handleClick = () => {
    if (props.setIsOpen) {
      props.setIsOpen((prevIsOpen) => !prevIsOpen);
    }
  };

  return (
    <button
      className={props.className + ' ' + getClassNameButtonType(props.buttonType.type)}
      onClick={handleClick} // Add the click handler
    >
      {props.text}
    </button>
  );
};