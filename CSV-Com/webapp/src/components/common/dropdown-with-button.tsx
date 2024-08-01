import React, { useState, useEffect } from 'react';
import './dropdown-with-button.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAsterisk , faPlus, faXmark} from "@fortawesome/free-solid-svg-icons";
import { Dropdown, DropdownObject } from './dropdown';

interface IDropDownProps {
    options: DropdownObject[];
    required: boolean;
    inputfieldname: string;
    value?: number[];
    onChange?: (value: number[]) => void;
    dataTestId?: string;
    error?: string;
}

const DropdownWithButton = (props: IDropDownProps) => {
    const getSelectableOptions = (options: DropdownObject[]): DropdownObject[] => {
        return options.filter(option => !selectedOptions.includes(option.value));
    }; // Get all options that can be selected in the dropdown. So not the options that already have been selected.

    const [selectedOptions, setSelectedOptions] = useState<number[]>(() => {
        return props.value ?? [];
    }); // All options that have been selected by the user

    const [dropdownOptions, setDropdownOptions] = useState<DropdownObject[]>(props.options); // All options that are available to select in the dropdown.

    const [dropdownValue, setDropdownValue] = useState<number>(0); // Dropdown value that's currently selected.

    const addOption = (value: number) => {
        if(dropdownValue === 0) return;

        setSelectedOptions([...selectedOptions, value]);        

        setDropdownValue(0);
    }; // Add an selected option.

    const removeOption = (value: number) => {
        setSelectedOptions(oldValues => {
                return oldValues.filter(number => number !== value);
            }
        )
    }; // Remove an selected option.

    useEffect(() => {
        setDropdownOptions(getSelectableOptions(props.options));
    }, [props.value, selectedOptions]); // Update method to update dropdown options.

    useEffect(() => {
        if (props.onChange) {
            props.onChange(selectedOptions);
        }
    }, [selectedOptions]);
    
    return (
        <div className='input-field flex-col'>
            <div className='dropdown-component-container'>
                <Dropdown 
                    className='dropdown-component'
                    options={dropdownOptions}
                    required={props.required}
                    inputfieldname={props.inputfieldname}
                    value={dropdownValue}
                    onChange={(e) => setDropdownValue(e)}
                    dataTestId={props.dataTestId}
                    error={props.error} />
                <button className='add-extra-dropdown-btn' type='button' data-testid={`${props.dataTestId}.add`} onClick={() => {addOption(dropdownValue);}}>
                    <FontAwesomeIcon className='ml-[0]' icon={faPlus} size="xl" style={{color: "#000000"}} />
                </button>
            </div>
            <div className='dropdown-badge-container'>
                {selectedOptions.map((selectedOption, index) => (
                    <div key={props.inputfieldname + '_badge_' + selectedOption} className='dropdownbadge'>
                        <div>
                            <p className='mx-1'>
                                {props.options.find(option => option.value === selectedOption)?.label}
                            </p>
                            <input name={props.inputfieldname} type="hidden" value={selectedOption} />
                        </div>
                        <button type='button' className='badgeBtn' onClick={() => removeOption(selectedOption)}>
                            <FontAwesomeIcon icon={faXmark} size="sm" style={{color: "#000000"}} />
                        </button>
                    </div>      
                ))}
            </div>
        </div>
    );
};

export default DropdownWithButton;
