import React from 'react';
import './dropdown.css';
import { ErrorMessage } from './error-message';


export interface DropdownObject {
    label: string;
    value: number;
}

interface IDropDownProps {
    options: Array<DropdownObject>;
    className?: string;
    required: boolean;
    inputfieldname: string;
    value?: number;
    onChange?: (value: number) => void;
    dataTestId?: string;
    error? :string;
}

const OPTION_TEXT = 'Kies uit de lijst'

export const Dropdown = (props: IDropDownProps) => (  
    <div className="dropdown-container">
        <select name={props.inputfieldname} id=""  className={`dropdown ${props.className}`}  required={props.required} value={props.value}
            onChange={(e) => {
                const selectedValue = parseInt(e.target.value);
                props.onChange?.(selectedValue);
            }}
            data-testid={props.dataTestId}>
            <option key={0} value=''>{OPTION_TEXT}</option>
            {props.options.map((item) => (
                <option key={item.value} value={item.value} >{item.label}</option>
            ))}
        </select>
        {props.error && <ErrorMessage error={props.error} />}
    </div>
);
