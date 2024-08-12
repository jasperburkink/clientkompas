import React, { useEffect } from 'react';
import './input-field.css';
import { InputFieldType } from 'types/common/InputFieldComponentType';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTriangleExclamation } from "@fortawesome/free-solid-svg-icons";
import { ErrorMessage } from './error-message';
import { ValidationError } from 'types/common/validation-error';

export interface InputFieldProps {
    value?: string,
    placeholder: string,
    required: boolean,
    inputfieldtype: InputFieldType,
    className?: string,
    onChange?: (value: string) => void,
    dataTestId?: string;
    errors?: ValidationError[];
}

export const InputField = (props: InputFieldProps) => {

    return(
    <div className={`input-field ${props.className}`}>
        <div className={`input-field-container`}>
            <input
            onChange={(e) => {props.onChange?.(e.target.value);}}        
            value={props.value}
            type={props.inputfieldtype.type}
            placeholder={props.placeholder}
            required={props.required}
            data-testid={props.dataTestId}
            className={`${props.errors ? 'error' : ''}`} />        
            <FontAwesomeIcon icon={faTriangleExclamation} className={`error-icon fa-lg ${props.errors ? 'visible' : 'hidden'}`} />
        </div>
        <ErrorMessage errors={props.errors} />
     </div>
)};