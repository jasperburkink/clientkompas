import React, { LabelHTMLAttributes } from 'react';
import '../../styles/common/header.css';

interface HeaderProps extends React.LabelHTMLAttributes<HTMLHeadingElement> {
    text: string
}

export const Header = (props: HeaderProps) => (
    <h1>
        {props.text}
    </h1>
  );