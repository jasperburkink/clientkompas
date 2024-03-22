import React from 'react';
import './link-button.css';
import { ButtonType } from 'types/common/ButtonComponentType';
import { getClassNameButtonType } from 'types/common/ButtonComponentType';

interface LinkButtonProps extends React.AnchorHTMLAttributes<HTMLAnchorElement> {
    href: string;
    text: string,
    buttonType: ButtonType
}

export const LinkButton = (props: LinkButtonProps) => (
  <a href={props.href}
  className={props.className + ' ' + getClassNameButtonType(props.buttonType.type)} >
    <p className='button'>{props.text}</p>
  </a>
);