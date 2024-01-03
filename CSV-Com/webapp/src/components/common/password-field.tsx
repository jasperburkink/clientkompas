import './password-field.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import {faEyeSlash,faEye} from "@fortawesome/free-solid-svg-icons";
import React, { useState, useEffect } from 'react';



interface IPasswordFieldProps extends React.HtmlHTMLAttributes<HTMLElement> {
    inputfieldname: string;
}
let visible = false;


const PasswordField = (props: IPasswordFieldProps) => {
    const [icon,setIcon] = useState(<FontAwesomeIcon icon={faEyeSlash} size="lg" style={{color: "#000000",}} />);
    function visibility() {
        if (visible === false) {
            visible = true;
            setIcon(<FontAwesomeIcon icon={faEye} size="lg" style={{color: "#000000",}} />);
        } else {
            visible = false;
            setIcon(<FontAwesomeIcon icon={faEyeSlash} size="lg" style={{color: "#000000",}} />)
        }
        
    }
    return(
        <div className='input-field'>
            <input type="password" className='passwordfield' placeholder='Wachtwoord' name={props.inputfieldname} />
            <button type="button" onClick={visibility} className='visibilitybtn'>{icon}</button>
        </div>
    );
};

export default PasswordField;