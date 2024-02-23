import React from 'react';
import './input-field-with-label.css';
import { InputField, InputFieldProps } from '../../components/common/input-field';

interface InputFieldWithLabelProps{
    text: string,
    inputFieldProps: InputFieldProps,
    className?: string
}

export const InputFieldWithLabel = (props: InputFieldWithLabelProps) => (
    <div className={'input-field-with-label ' + props.className}>
        <label htmlFor={props.text}>{props.text}</label>
        <InputField
        value={props.inputFieldProps.value}
        placeholder={props.inputFieldProps.placeholder}
        required={props.inputFieldProps.required}
        inputfieldtype={props.inputFieldProps.inputfieldtype} />
    </div>
);