import React from 'react';
import './input-field-with-label.css';
import { InputField, InputFieldProps } from '../../components/common/input-field';

interface InputFieldWithLabelProps extends React.HtmlHTMLAttributes<HTMLElement> {
    text: string,
    inputFieldProps: InputFieldProps
}

export const InputFieldWithLabel = (props: InputFieldWithLabelProps) => (
    <div className='input-field-with-label'>
        <label htmlFor={props.id}>{props.text}</label>
        <InputField {...props}
        value={props.inputFieldProps.value}
        placeholder={props.inputFieldProps.placeholder}
        required={props.inputFieldProps.required}
        inputfieldtype={props.inputFieldProps.inputfieldtype} />
    </div>
);