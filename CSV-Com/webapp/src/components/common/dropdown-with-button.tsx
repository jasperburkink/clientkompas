import React, { useState } from 'react';
import ReactDOM from 'react-dom';
import './dropdown-with-button.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAsterisk } from "@fortawesome/free-solid-svg-icons";

interface DropdownObject {
    Value: number;
    Label: string;
}


interface IDropDownProps extends React.HTMLProps<HTMLSelectElement> {
    array: DropdownObject[];
    required: boolean;
   
}
 
const DropdownWithButton = (props: IDropDownProps) => {
    const newArray = props.array;
    const [badge, setDropdowns] = useState<JSX.Element[]>([]);

    const removerExtraDropdown = () => {
        const newDropdowns = badge.slice();
        const newElement = (
          <p></p>
        );
        newDropdowns.push(newElement);
        setDropdowns(newDropdowns);
    };

    const addExtraDropdown = () => {
        const newbadge = badge.slice();
        const newElement = (
           <p>text</p>
        );
        newbadge.push(newElement);
        setDropdowns(newbadge);
    };

    return (
        <div className='input-field flex-col '>
            <div className='flex'>
            <select name="" id="" className='dropdown' required={props.required}>
            <option value="" disabled selected>
                Kies uit de lijst
            </option>
            {newArray.map((item, index) => (
                <option key={index} value={item.Value}>
                    {item.Label}
                </option>
            ))}
        </select>
        <button className='add-extra-dropdown-btn' type='button'  onClick={addExtraDropdown}></button>
        {props.required === true && <FontAwesomeIcon icon={faAsterisk} className="fa-solid fa-1x"/>}
            </div>
            <div className='flex flex-wrap max-w-[100%]'>
        {badge.map((badge, index) => (
        <div key={index} className='flex'>
            {badge}
            <button type='button'>X</button>
            </div>      
    ))}
    </div>
    </div>
    
    );
};

export default DropdownWithButton;