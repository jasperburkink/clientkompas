import React, { useEffect } from 'react';
import './input-field.css';
import { InputFieldType } from 'types/common/InputFieldComponentType';

export interface InputFieldProps {
    value?: string,
    placeholder: string,
    required: boolean,
    inputfieldtype: InputFieldType,
    className?: string,
    onChange?: (value: string) => void,
    dataTestId?: string;
}

export const InputField = (props: InputFieldProps) => {

    return(
    <div className={'input-field ' + props.className}>
        <input
        onChange={(e) => {props.onChange?.(e.target.value);}}        
        value={props.value}
        type={props.inputfieldtype.type}
        placeholder={props.placeholder}
        required={props.required}
        data-testid={props.dataTestId}  />
     </div>
)};