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

const OPTION_TEXT = 'Kies uit de lijst'

const DropdownWithButton = (props: IDropDownProps) => {
    const [badges, setBadges] = useState<IBadge[]>([]);
    const [currentOptions, setCurentOptions] = useState<IBadge[]>([]);
    const [value,setSelect] = useState('');

    const sortByName = (array: IBadge[]) => {
        
    };

    const removeBadge = (badge: IBadge) => {
        let arr = currentOptions.concat(badge);
        arr.sort();
        setCurentOptions(arr);    
        setBadges(badges.filter(a => a.id !== badge.id));
    };

    const addBadge = () => {
        const val = parseInt(value);
        let id = 0;
        let text = ""

        props.options.forEach((item, index) => {
            if (item.Value === val) {
                id = item.Value;
                text = item.Label;
                return;
            }
        });

        if (value !== '') {
            const newBadge: IBadge = {
                id,
                text,
            }
            setBadges(badges.concat(newBadge).sort()); 
            setCurentOptions(currentOptions.filter(badge => badge.id !== id));
            setSelect('');
        }
    };

    useEffect(() => {
        let newOptions: IBadge[] = [];
        props.options.forEach((item, index) => {
            const id = item.Value;
            const text = item.Label;
            newOptions.push({id: id, text: text});
        });
        setCurentOptions(newOptions);
      }, [props.options]);

    return (
    <div className='input-field flex-col '>
        <div className='flex'>
                    <select id="" className='dropdown' onChange={event => {setSelect(event.target.value)}} required={props.required}>
                        <option value="">{OPTION_TEXT}</option>
                        {currentOptions.map((currentOption, index) => (
                            <option key={currentOption.id} value={currentOption.id}>{currentOption.text}</option>
                        ))};    
                        <FontAwesomeIcon icon={faAngleDown} size="sm" style={{color: "#000000",}} />
                    </select>
                <button className='add-extra-dropdown-btn' type='button'  onClick={addBadge}>
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
                        <button type='button' className='badgeBtn' onClick={() => removeBadge(badge)}><FontAwesomeIcon icon={faXmark} size="sm" style={{color: "#000000",}} /></button>
                    </div>      
                ))}
            </div>
    </div>
    );
};

export default DropdownWithButton;

