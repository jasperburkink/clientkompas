import React from 'react';
import './input-field.css';
import { InputFieldType } from '../../types/common/InputFieldType';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAsterisk } from "@fortawesome/free-solid-svg-icons";

export interface InputFieldProps extends React.HtmlHTMLAttributes<HTMLInputElement> {
    value?: string,
    placeholder: string,
    required: boolean,
    inputFieldType: InputFieldType
}

export const InputField = (props: InputFieldProps) => (  
    <div className='input-field'>
        <input {...props}
        value={props.value} 
        type={props.inputFieldType.type}         
        placeholder={props.placeholder} 
        required={props.required} /> 
        {props.required === true &&
        <FontAwesomeIcon icon={faAsterisk} className="fa-solid fa-1x"/>}
     </div>
);