import React from 'react';
import '../../styles/common/input-field.css';
import { InputFieldType } from '../../types/common/InputFieldType';

export interface InputFieldProps extends React.HtmlHTMLAttributes<HTMLInputElement> {
    value?: string,
    placeholder: string,
    required: boolean,
    inputFieldType: InputFieldType
}

export const InputField = (props: InputFieldProps) => (  
    <input {...props}
     value={props.value} 
     type={props.inputFieldType.type} 
     className="inputField" 
     placeholder={props.placeholder} 
     required={props.required} />
);