import React from 'react';
import './link-button.css';
import { ButtonType } from '../../types/common/ButtonType';
import { getClassNameButtonType } from '../../types/common/ButtonType';

interface LinkButtonProps extends React.AnchorHTMLAttributes<HTMLAnchorElement> {
    text: string,
    buttonType: ButtonType
}

export const LinkButton = (props: LinkButtonProps) => (
  <a {...props}
  className={props.className + ' ' + getClassNameButtonType(props.buttonType.type)} >
    <p className='button'>{props.text}</p>
  </a>
);