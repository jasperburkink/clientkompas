import React from 'react';
import './input-field.css';
import { InputFieldType } from 'types/common/InputFieldComponentType';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTriangleExclamation } from "@fortawesome/free-solid-svg-icons";
import { ErrorMessage } from './error-message';
import { ValidationError } from 'types/common/validation-error';
import Decimal from 'decimal.js-light';

export interface DecimalInputFieldProps {
    value?: Decimal;
    valuestring?: string;    
    placeholder: string;
    required: boolean;
    inputfieldtype: InputFieldType;
    className?: string;
    onChange?: (value: Decimal) => void;
    dataTestId?: string;
    errors?: ValidationError[];
    readOnly?: boolean; 
}

export const DecimalInputField = (props: DecimalInputFieldProps) => {

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (props.readOnly) return;
    
        let inputValue = e.target.value;

        try
        {
            if(inputValue.endsWith(','))
            {
                inputValue += '0';
            }

            inputValue = inputValue.replace(',', '.');

            const decimalValue = new Decimal(inputValue);

            // props.value = decimalValue;
            props.onChange?.(decimalValue);

            
        }
        catch (err)
        {
            console.error('Invalid decimal value:', err);
        }
    };    

    return (
        <div className={`input-field ${props.className}`}>
            <div className={`input-field-container`}>
                <input
                    onChange={handleChange}
                    value={props.valuestring}
                    type={props.inputfieldtype.type}
                    placeholder={props.placeholder}
                    required={props.required}
                    data-testid={props.dataTestId}
                    className={`${props.errors ? 'error' : ''}`}
                    readOnly={props.readOnly} />
                <FontAwesomeIcon icon={faTriangleExclamation} className={`error-icon fa-lg ${props.errors ? 'visible' : 'hidden'}`} />
            </div>
            <ErrorMessage errors={props.errors} />
        </div>
    );
};
