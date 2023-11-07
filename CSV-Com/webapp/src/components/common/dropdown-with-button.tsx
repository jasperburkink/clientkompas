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
    const [badge, setBadge] = useState<JSX.Element[]>([]);
    const [option, setoption] = useState<JSX.Element[]>([]);
    const [value,setselect] = useState('');
    




    const removebadge = (index: number) => {
        const removebadge = badge.filter((_, i) => i !== index);
        setBadge(removebadge);
    };

    const addbadge = () => {
        const id = parseInt(value);
        let label = ""
        newArray.map((item, index) => {
            if (item.Value === id) {
                label = item.Label;
            }
        });
        if (value !== '') {
            const newbadge = badge.slice();
            const newElement = (
                <p className='mx-1'>{label}</p>
            );
            newbadge.push(newElement);
            setBadge(newbadge);
        }
       
    };
    const ar = ['1','2'];
    return (
        <div className='input-field flex-col '>
            <div className='flex'>
            <select name="" id="" className='dropdown'defaultValue={ar} onChange={event => setselect(event.target.value)} required={props.required}>
            <option value="" disabled selected>
                Kies uit de lijst
            </option>
            {newArray.map((item, index) => (
                <option key={index} value={item.Value}>
                    {item.Label}
                </option>
                
            ))}
        </select>
        <button className='add-extra-dropdown-btn' type='button'  onClick={addbadge}></button>
        {props.required === true && <FontAwesomeIcon icon={faAsterisk} className="fa-solid fa-1x"/>}
            </div>
            <div className='flex flex-wrap max-w-[100%]'>
        {badge.map((badge, index) => (
        <div key={index} className='dropdownbadge'>
            {badge}
            <button type='button' className='badgeBtn' onClick={() => removebadge(index)}></button>
            </div>      
    ))}
    </div>
    </div>
    
    );
};

export default DropdownWithButton;