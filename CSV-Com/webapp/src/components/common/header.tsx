import React, { LabelHTMLAttributes } from 'react';
import './header.css';

interface HeaderProps extends React.LabelHTMLAttributes<HTMLHeadingElement> {
    text: string
}

export const Header = (props: HeaderProps) => (
    <div {...props} className={"header " + props.className}>
        <p>
            {props.text}
        </p>
    </div>
  );