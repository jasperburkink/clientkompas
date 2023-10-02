import React from 'react';
import './button.css';
import { ButtonType } from '../../types/common/ButtonComponentType';
import { getClassNameButtonType } from '../../types/common/ButtonComponentType';

interface ButtonProps extends React.HtmlHTMLAttributes<HTMLButtonElement> {
    text: string,
    buttonType: ButtonType
}

export const Button = (props: ButtonProps) => (
  <button {...props}
    className={props.className + ' ' + getClassNameButtonType(props.buttonType.type)} >
      {props.text}
  </button>
);