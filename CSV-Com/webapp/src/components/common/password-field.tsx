import './password-field.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {faEyeSlash,faEye} from "@fortawesome/free-solid-svg-icons";
import React, { useState } from 'react';

interface IPasswordFieldProps extends React.HtmlHTMLAttributes<HTMLElement> {
    inputfieldname: string;
    placeholder: string;
}

const PasswordField = (props: IPasswordFieldProps) => {
    const [icon,setIcon] = useState(<FontAwesomeIcon icon={faEyeSlash} size="lg" style={{color: "#000000",}} />);
    const [type,setType] = useState('password');
    
    function visibility() {
        if (type === "password") {
            setType('text');
            setIcon(<FontAwesomeIcon icon={faEye} size="lg" style={{color: "#000000",}} />);
        } else {
            setType('password')
            setIcon(<FontAwesomeIcon icon={faEyeSlash} size="lg" style={{color: "#000000",}} />)
        }
        
    }
    return(
        <div className='input-field'>
            <input type={type} className='passwordfield' placeholder={props.placeholder} name={props.inputfieldname} />
            <button type="button" onClick={visibility} className='visibilitybtn'>{icon}</button>
        </div>
    );
};

export default PasswordField;