import React from 'react';
import './link-button.css';
import { ButtonType } from 'types/common/ButtonComponentType';
import { getClassNameButtonType } from 'types/common/ButtonComponentType';

interface LinkButtonProps extends React.AnchorHTMLAttributes<HTMLAnchorElement> {
    href: string;
    text: string;
    buttonType: ButtonType;
    dataTestId?: string;
}

export const LinkButton = (props: LinkButtonProps) => (
  <a href={props.href}
  className={props.className + ' ' + getClassNameButtonType(props.buttonType.type)}
  data-testid={props.dataTestId} >
    <p className='button'>{props.text}</p>
  </a>
);