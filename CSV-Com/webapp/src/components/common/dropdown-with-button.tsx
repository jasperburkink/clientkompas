import React, { useState, useEffect } from 'react';
import './dropdown-with-button.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAsterisk , faPlus,faXmark,faAngleDown} from "@fortawesome/free-solid-svg-icons";

interface DropdownObject {
    Value: number;
    Label: string;

}

interface IDropDownProps extends React.HTMLProps<HTMLSelectElement> {
    options: DropdownObject[];
    required: boolean;
    inputfieldname: string;
   
}

interface IBadge{
    id: number;
    text: string;
}

const DropdownWithButton = (props: IDropDownProps) => {
    
    const [badges, setBadges] = useState<IBadge[]>([]);
    const [currentoptions, setcurentoptions] = useState<IBadge[]>([]);
    const [value,setselect] = useState('');

    const removebadge = (badge: IBadge) => {
        setcurentoptions(currentoptions.concat(badge));    
        setBadges(badges.filter(a => a.id !== badge.id));
    };

    const addbadge = () => {
        const val = parseInt(value);
        let id = 0;
        let text = ""

        props.options.forEach((item, index) => {
            if (item.Value === val) {
                id = item.Value;
                text = item.Label;
            }
        });

        if (value !== '') {
            const newBadge: IBadge = {
                id,
                text,
            }
            setBadges(badges.concat(newBadge)); 
            setcurentoptions(currentoptions.filter(badge => badge.id !== id));
            setselect('');
        }
    };

    useEffect(() => {
        let newoptions: IBadge[] = [];
        props.options.forEach((item, index) => {
            const id = item.Value;
            const text = item.Label;
            newoptions.push({id: id, text: text});
        });
        setcurentoptions(newoptions);
      }, [props.options]);

    return (
    <div className='input-field flex-col '>
        <div className='flex'>
                    <select id="" className='dropdown' onChange={event => {setselect(event.target.value)}} required={props.required}>
                        <option value="">Kies uit de lijst</option>
                        {currentoptions.map((currentoption, index) => (
                            <option key={currentoption.id} value={currentoption.id}>{currentoption.text}</option>
                        ))};    
                        <FontAwesomeIcon icon={faAngleDown} size="sm" style={{color: "#000000",}} />
                    </select>
                <button className='add-extra-dropdown-btn' type='button'  onClick={addbadge}>
                <FontAwesomeIcon className='ml-[0]' icon={faPlus} size="xl" style={{color: "#000000",}} />
                </button>
                {props.required === true && <FontAwesomeIcon icon={faAsterisk} className="fa-solid fa-1x"/>}
        </div>
            <div className='flex flex-wrap max-w-[100%]'>
                {badges.map((badge, index) => (  
                    <div key={badge.id} className='dropdownbadge'>
                            <div>
                                <p className='mx-1'>{badge.text}</p>
                                <input name={props.inputfieldname} type="hidden" value={badge.id} />
                            </div>
                        <button type='button' className='badgeBtn' onClick={() => removebadge(badge)}><FontAwesomeIcon icon={faXmark} size="sm" style={{color: "#000000",}} /></button>
                    </div>      
                ))}
            </div>
    </div>
    );
};

export default DropdownWithButton;

