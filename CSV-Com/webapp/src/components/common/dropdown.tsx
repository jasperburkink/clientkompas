import React from 'react';
import './dropdown.css';
import { ErrorMessage } from './error-message';
import { ValidationError } from 'types/common/validation-error';

export interface DropdownObject {
    label: string;
    value: any;
}

interface IDropDownProps {
    options: Array<DropdownObject>;
    className?: string;
    required: boolean;
    inputfieldname: string;
    value?: any;
    onChange?: (value: any) => void;
    dataTestId?: string;
    errors? :ValidationError[];
}

const OPTION_TEXT = 'Kies uit de lijst';

export const Dropdown = (props: IDropDownProps) => (  
    <div className={`dropdown-container ${props.className}`} >
        <select name={props.inputfieldname} className={`dropdown ${props.errors ? 'error' : ''}`} required={props.required} value={props.value}
            onChange={(e) => {
                const selectedValue = e.target.value;
                props.onChange?.(selectedValue);
            }}
            data-testid={props.dataTestId}>
            {props.required === false && <option key={0} value='0'>{OPTION_TEXT}</option>}
            {props.options.map((item) => (
                <option key={item.value} value={item.value} >{item.label}</option>
            ))}
        </select>
        <ErrorMessage errors={props.errors} />
    </div>
);
