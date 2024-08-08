import React from 'react';
import './dropdown-boolean.css';
import {objectToBoolean, booleanToNumber, numberToBoolean} from '../../utils/utilities';


export interface DropdownBooleanObject {
    label: string;
    value: number;
}

interface IDropDownBooleanProps {
    required: boolean;
    inputfieldname: string;
    value?: boolean;
    onChange?: (value: boolean) => void;
}

const OPTION_TEXT = 'Kies uit de lijst';
const OPTION_YES = 'Ja';
const OPTION_NO = 'Nee';

const DropdownBoolean = (props: IDropDownBooleanProps) => (  
    <div className='input-field'>
        <select name={props.inputfieldname} id=""  className='dropdown'  required={props.required} value={booleanToNumber(props.value!)}
        onChange={(e) => props.onChange?.(numberToBoolean(parseInt(e.target.value, 10)))}>
            {props.required && <option key={0} value=''>{OPTION_TEXT}</option>}
            <option key={1} value={1}>{OPTION_YES}</option>
            <option key={2} value={0}>{OPTION_NO}</option>
        </select>
    </div>
);

export default DropdownBoolean;