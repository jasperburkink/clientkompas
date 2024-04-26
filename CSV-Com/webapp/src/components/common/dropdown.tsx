import React from 'react';
import './dropdown.css';


export interface DropdownObject {
    label: string;
    value: number;
}

interface IDropDownProps {
    options: Array<DropdownObject>;
    required: boolean;
    inputfieldname: string;
    value?: number;
    onChange?: (value: number) => void;
}

const OPTION_TEXT = 'Kies uit de lijst'

export const Dropdown = (props: IDropDownProps) => (  
    <div className='input-field'>
        <select name={props.inputfieldname} id=""  className='dropdown'  required={props.required} value={props.value}
        onChange={(e) => {
            const selectedValue = parseInt(e.target.value);
            props.onChange?.(selectedValue);
        }}>
            <option key={0} value=''>{OPTION_TEXT}</option>
            {props.options.map((item) => (
                <option key={item.value} value={item.value} >{item.label}</option>
            ))}
        </select>
    </div>
);
