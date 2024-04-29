import React, { useState, useEffect } from 'react';
import './dropdown.css';
import './dropdown-with-button.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAsterisk , faPlus,faXmark} from "@fortawesome/free-solid-svg-icons";

export interface IDropdownObject {
    value: number;
    label: string;
}

interface IDropDownProps {
    options: IDropdownObject[];
    required: boolean;
    inputfieldname: string;
    value?: number[];
    onChange?: (value: number[]) => void;
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

    const removeBadge = (badge: IBadge) => {
        let newcurrentOptions = currentOptions.concat(badge);
        setCurentOptions(newcurrentOptions); 
        let newBadges = badges.filter(a => a.id !== badge.id);   
        setBadges(newBadges);

        onChangeBadges(newBadges, props);
    };

    const addBadge = (item?: number) => {
        if ((!value || value === '') && !item) return;

        let valueOption = item ?? parseInt(value);

        const option = props.options.find(element => element.value === valueOption);
        const newBadge: IBadge = {
          id: option!.value,
          text: option!.label
        }
        
        const newbadges = badges.concat(newBadge);
        setBadges(newbadges);

        onChangeBadges(newbadges, props);

        setCurentOptions(currentOptions.filter(badge => badge.id !== option?.value));
        setSelect('');
    };

    // const addBadge = async (item?: number) => {
    //     if ((!value || value === '') && !item) return;
    
    //     let valueOption = item ?? parseInt(value);
    
    //     const option = props.options.find(element => element.value === valueOption);
    //     const newBadge: IBadge = {
    //         id: option!.value,
    //         text: option!.label
    //     };
        
    //     // Wacht tot de badge is toegevoegd voordat de staat wordt bijgewerkt
    //     await new Promise<void>((resolve) => {
    //         const updateState = () => {
    //             const newbadges = badges.concat(newBadge);
    //             setBadges(newbadges);
        
    //             onChangeBadges(newbadges, props);
        
    //             setCurentOptions(currentOptions.filter(badge => badge.id !== option?.value));
    //             setSelect('');
    //             resolve();
    //         };
            
    //         updateState();
    //     });
    // };

    useEffect(() => {
        const addOptions = async () => {
            const newOptions: IBadge[] = props.options.map(item => ({id: item.value, text: item.label}));
            setCurentOptions(newOptions);
        };

        addOptions();
    }, [props.options]);

    useEffect(() => {        

        const addExitingBadges = async () => {
            //setBadges([]);

            if (props.value && Array.isArray(props.value)) {
                for (const item of props.value) {
                    if (item) {
                        await addBadge(item);
                    }
                }
            }            
        };
    
        addExitingBadges();
    }, [props.value]);

    return (
        <div className='input-field flex-col '>
            <div className='flex'>
                <select id="" className='dropdown' onChange={event => {setSelect(event.target.value);}} required={props.required}>
                    <option value=''>{OPTION_TEXT}</option>
                    {currentOptions.map((currentOption, index) => (
                        <option key={props.inputfieldname + currentOption.id} value={currentOption.id}>{currentOption.text}</option>
                    ))};    
                </select>
                <button className='add-extra-dropdown-btn' type='button'  onClick={() => {addBadge();}}>
                    <FontAwesomeIcon className='ml-[0]' icon={faPlus} size="xl" style={{color: "#000000",}} />
                </button>
            </div>
            <div className='flex flex-wrap max-w-[100%]'>
                {badges.map((badge, index) => (  
                    <div key={props.inputfieldname + '_badge_' +badge.id} className='dropdownbadge'>
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

function onChangeBadges(badges: IBadge[], props: IDropDownProps) {
    let badgesIds = badges.map(badge => badge.id);
    props.onChange?.(badgesIds);
}
