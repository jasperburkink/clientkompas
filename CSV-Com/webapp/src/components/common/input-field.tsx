import React from 'react';
import './input-field.css';
import { InputFieldType } from '../../types/common/InputFieldComponentType';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAsterisk } from "@fortawesome/free-solid-svg-icons";

export interface InputFieldProps {
    value?: string,
    placeholder: string,
    required: boolean,
    inputfieldtype: InputFieldType,
    className?: string,
    onChange?: (value: string) => void;
}

export const InputField = (props: InputFieldProps) => (  
    <div className={'input-field ' + props.className}>
        <input
        onChange={(e) => props.onChange?.(e.target.value)}
        value={props.value} 
        type={props.inputfieldtype.type}         
        placeholder={props.placeholder} 
        required={props.required} /> 
        {props.required === true &&
        <FontAwesomeIcon icon={faAsterisk} className="fa-solid fa-1x"/>}
     </div>
);