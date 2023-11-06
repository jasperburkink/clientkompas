import React from 'react';
import './dropdown.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAsterisk } from "@fortawesome/free-solid-svg-icons";


interface DropdownObject {
    Label: string;
    Value: number;
}

interface idropDownProps extends React.HtmlHTMLAttributes<HTMLElement> {
    array: Array<DropdownObject>,
    required: boolean
}


export const Dropdown = (props: idropDownProps) => (  
  
    <div className='input-field'>
        <select name="" id=""  className='dropdown'  required={props.required} >
            <option value='' disabled selected>Kies uit de lijst</option>


            {props.array.map((item) => (
          <option value={item.Value} >{item.Label}</option>
        ))}
                
        </select>
        {props.required === true && <FontAwesomeIcon icon={faAsterisk} className="fa-solid fa-1x"/>}
     </div>
);
