import React from 'react';
import './dropdown.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAsterisk } from "@fortawesome/free-solid-svg-icons";

interface DropdownObject {
    label: string;
    value: number;
}

interface IDropDownProps extends React.HtmlHTMLAttributes<HTMLElement> {
    options: Array<DropdownObject>;
    required: boolean;
    inputfieldname: string;
}

const OPTION_TEXT = 'Kies uit de lijst'

export const Dropdown = (props: IDropDownProps) => (  
    <div className='input-field'>
        <select name={props.inputfieldname} id=""  className='dropdown'  required={props.required} >
            <option key={0} value=''>{OPTION_TEXT}</option>
            {props.options.map((item) => (
                <option key={item.value} value={item.value} >{item.label}</option>
            ))}
        </select>
        {props.required === true && <FontAwesomeIcon icon={faAsterisk} className="fa-solid fa-1x"/>}
    </div>
);
