import React, { useState } from 'react';
import './input-field.css';
import { InputFieldType } from 'types/common/InputFieldComponentType';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTriangleExclamation } from "@fortawesome/free-solid-svg-icons";
import { ErrorMessage } from './error-message';
import { ValidationError } from 'types/common/validation-error';
import Decimal from 'decimal.js-light';

export interface DecimalInputFieldProps {
    value?: string;
    placeholder: string;
    required: boolean;
    inputfieldtype: InputFieldType;
    className?: string;
    onChange?: (value: string) => void;
    dataTestId?: string;
    errors?: ValidationError[];
    readOnly?: boolean;
}

export const DecimalInputField = (props: DecimalInputFieldProps) => {
    
    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (props.readOnly) return;
    
        let inputValue = e.target.value;

        // Regex to allow only numbers, commas, dots, and a minus sign at the start
        const validInputRegex = /^-?\d*[.,]?\d*$/;

        if (validInputRegex.test(inputValue)) {
            // Replace comma with dot for internal Decimal parsing, but not for display
            const normalizedValue = inputValue.replace(',', '.');
            
            try {
                // If the input can be parsed as a valid Decimal, propagate it
                if (inputValue.endsWith('.') || inputValue.endsWith(',') || inputValue === '-') {
                    // Allow incomplete decimals (e.g., "123.", "123,", or "-")
                    props.onChange?.(inputValue);
                } else {
                    const decimal = new Decimal(normalizedValue);
                    props.onChange?.(decimal.toString().replace('.', ','));
                }
            } catch (err) {
                console.error('Invalid decimal value:', err);
            }
        } else {
            console.error('Invalid character entered');
        }
    };

    return (
        <div className={`input-field ${props.className}`}>
            <div className={`input-field-container`}>
                <input
                    onChange={handleChange}
                    value={props.value}
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
