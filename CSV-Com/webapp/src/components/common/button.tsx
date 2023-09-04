import React from 'react';
import '../../styles/common/button.css';
import { ButtonType } from '../../types/common/ButtonType';
import { getClassNameButtonType } from '../../types/common/ButtonType';

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