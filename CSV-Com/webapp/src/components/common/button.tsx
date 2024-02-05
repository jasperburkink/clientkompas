import React from 'react';
import './button.css';
import { ButtonType } from '../../types/common/ButtonComponentType';
import { getClassNameButtonType } from '../../types/common/ButtonComponentType';

interface ButtonProps {
    text: string;
    buttonType: ButtonType;
    onClick: () => void;
    className?: string;
}

export const Button = (props: ButtonProps) => (
  <button data-testid='button_test' onClick={props.onClick} className={getClassNameButtonType(props.buttonType.type) + (props.className ? ` ${props.className}` : '')}>
    {props.text}
  </button>
);