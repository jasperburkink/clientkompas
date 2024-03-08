import React, { Children, ReactNode } from 'react';
import './label-field.css';

interface LabelFieldProps{
    text: string,
    children: ReactNode,
    required?: boolean,
    className?: string
}

const LabelField = (props: LabelFieldProps) => (
    <div className={'label-field ' + props.className}>
        <label htmlFor={props.text}>{`${props.text}${props.required ? '*' : ''}`}</label>
        <div className='field-container'>
            {props.children}
        </div>
    </div>
);

export default LabelField