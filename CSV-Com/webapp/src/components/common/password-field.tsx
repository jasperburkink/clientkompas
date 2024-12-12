import './password-field.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {faEyeSlash,faEye} from "@fortawesome/free-solid-svg-icons";
import React, { useState } from 'react';
import { ValidationError } from 'types/common/validation-error';
import { ErrorMessage } from './error-message';

interface IPasswordFieldProps {
    inputfieldname: string;    
    placeholder: string;
    value?: string;
    className?: string;
    dataTestId?: string;
    errors?: ValidationError[];
    onChange?: (value: string) => void;
}
const INPUTFIELDTYPE_PASSWORD = 'password';
const INPUTFIELDTYPE_TEXT = 'text';

const PasswordField = (props: IPasswordFieldProps) => {
    const [icon,setIcon] = useState(<FontAwesomeIcon icon={faEyeSlash} size="lg" style={{color: "#000000",}} />);
    const [type,setType] = useState(INPUTFIELDTYPE_PASSWORD);
    
    function visibility() {
        if (type === INPUTFIELDTYPE_PASSWORD) {
            setType(INPUTFIELDTYPE_TEXT);
            setIcon(<FontAwesomeIcon icon={faEye} size="lg" style={{color: "#000000",}} />);
        } else {
            setType(INPUTFIELDTYPE_PASSWORD);
            setIcon(<FontAwesomeIcon icon={faEyeSlash} size="lg" style={{color: "#000000",}} />)
        }
    }
    return(
        <div>
            <div className='input-field password-container'>
                <input 
                    type={type} 
                    className={`passwordfield ${props.className} ${props.errors ? 'error' : ''}`}
                    placeholder={props.placeholder} 
                    name={props.inputfieldname} 
                    value={props.value} 
                    onChange={(e) => {props.onChange?.(e.target.value);}}
                    data-testid={props.dataTestId} />
                <button type="button" onClick={visibility} className='visibilitybtn'>{icon}</button>                        
            </div>
            <ErrorMessage errors={props.errors} />
        </div>
    );
};

export default PasswordField;