import '../index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleDown, faPlus } from "@fortawesome/free-solid-svg-icons";

import React, { useEffect, useState } from "react";

export function InputFieldText(props) {  
    const [value, setValue] = useState(props.placeholder);
    const openDropDown = () => {
        const test = document.getElementById("dropdown" + props.text).style.display
        if(test === "block")
        {
            document.getElementById("dropdown" + props.text).style.display = "none"
        }else{
            document.getElementById("dropdown" + props.text).style.display = "block"
        }

    }
    // useEffect(() => {
    //     setValueDropdown();
    // }, [])
    const setValueDropdown = (newValue) =>{
        setValue(newValue)
        document.getElementById("value" + props.text).style.color = "black"
    }
    var type = props.type
    if(type === "dropdown"){
        return (
        <div className='md:col-span-2'>
            <div className='flex'>
                <div className='w-1/5'>{props.text}</div>
                <div className="md:col-span-2 inputField w-4/5 flex justify-between cursor-pointer" onClick={openDropDown}>
                    <div id={'value' + props.text} className='text-subGray2'>
                        {value}                    
                    </div>
                    <FontAwesomeIcon icon={faAngleDown} className="fa-solid fa-xl my-auto" />
                </div>
            </div>
            <div id={'dropdown' + props.text} className='w-4/5 ml-auto bg-gray-200  text-sm rounded-b-lg -mt-5 p-2.5 pt-[30px] hidden'>
                {props.children}
                <div onClick={ () => setValueDropdown("Test")} value="Test" className='cursor-pointer'>Test1</div>
            </div>
        </div>
        )
    }else if(type === "dropdownPlus"){
        return (
            <div className='md:col-span-2'>
                <div className='flex'>
                    <div className='w-1/5'>{props.text}</div>
                    <div className='w-4/5 flex'>
                        <div className="inputFieldDropDown flex justify-between cursor-pointer" onClick={openDropDown}>
                            <div className='text-subGray2'>
                            {value}                   
                            </div>
                            <FontAwesomeIcon icon={faAngleDown} className="fa-solid fa-xl my-auto" />
                        </div>
                        <div className='inputField h-[45px] w-[45px] flex justify-center content-center flex-wrap aspect-square '>
                            <FontAwesomeIcon icon={faPlus} className=" fa-lg m-auto cursor-pointer"/>
                        </div>
                    </div>
                </div>
                <div id={'dropdown' + props.text} className='w-4/5-45px ml-auto mr-[45px] bg-gray-200 text-sm rounded-b-lg -mt-5 p-2.5 pt-[30px] hidden'>
                    {props.children}
                </div>
            </div>
        )
    }else if(type === "small"){
        return (
            <div className='flex w-2/5'>
                <div className='w-1/2'>{props.text}</div>
                <input value={props.value} onChange={props.onChange} type="text" className="inputField w-1/2 h-8" placeholder={props.placeholder} required={props.required}></input>
            </div>
            );
    } else{
        return (
            <div className='flex'>
                <div className='w-1/5'>{props.text}</div>
                <input value={props.value} onChange={props.onChange} type="text" className="inputField w-4/5 h-8" placeholder={props.placeholder} required={props.required}></input>
            </div>
            );
    } 
}