import '../index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleDown, faL, faPlus } from "@fortawesome/free-solid-svg-icons";

import React, { useEffect, useState } from "react";
import { InputFieldAddition } from './inputFieldAddition';



export function InputFieldText({state, stateChanger, stateName, ...props}) {  
    const GetOptions = (props) =>{
        if (props.options === undefined) return null;
        if(props.options.length > 0){
            let allOptions = []
            for (let i = 0; i < props.options.length; i++) {
                allOptions.push(<div key={i} onClick={ () => setValueDropdown(props.options[i].name)} value={props.options[i].value} className='cursor-pointer'>{props.options[i].name}</div>)
            }
            return(allOptions)
        }else{
            return("")
        }
    }
    const [value, setValue] = useState(props.placeholder);
    
    const [extraList, setExtraList] = useState([])
    const openCloseDropDown = () => {
        const isOpen = document.getElementById("dropdown" + props.text).style.display
        if(isOpen === "block")
        {
            document.getElementById("dropdown" + props.text).style.display = "none"
        }else{
            document.getElementById("dropdown" + props.text).style.display = "block"
        }

    }
    const setValueDropdown = (newValue) =>{
        setValue(newValue)
        openCloseDropDown()
        document.getElementById("value" + props.text).style.color = "black"
        if(!stateName){
            stateChanger(newValue)
        }
    }
    const handleExtraAdd = (value) => {
        setExtraList([...extraList, {extra: [value]}])
        stateChanger([...state, {[stateName]: value}])
    }
    const handleExtraRemove = (index) => {
        const list = [...extraList]
        list.splice(index, 1)
        setExtraList(list)
    }
    useEffect(() =>{
        if(props.info){
            let arr = [...extraList];
            for(let i = 0; i < props.info.length; i++){
                arr[i] = {extra: [props.info[i].naam]}
            }
            setExtraList(arr)
        }
    }, [])
    var type = props.type
    if(type === "dropdown"){
        let isDefault = true
        if (props.placeholder != "Kies uit de lijst"){isDefault = false}
        return (
        <div className=''>
            <div className='flex'>
                <div className='w-1/5'>{props.text}</div>
                <div className="md:col-span-2 inputField w-4/5 flex justify-between cursor-pointer" onClick={openCloseDropDown}>
                {isDefault ? (
                    <div id={'value' + props.text} value={value} className='text-subGray2'>
                        {value}                    
                    </div>
                ) : (
                    <div id={'value' + props.text} value={value} className='text-black'>
                        {value}                    
                    </div>
                )}
                    <FontAwesomeIcon icon={faAngleDown} className="fa-solid fa-xl my-auto" />
                </div>
            </div>
            <div id={'dropdown' + props.text} className='w-4/5 ml-auto bg-gray-200  text-sm rounded-b-lg -mt-5 p-2.5 pt-[30px] hidden'>
                <GetOptions options={props.options} />
            </div>
        </div>
        )
    }else if(type === "dropdownPlus"){
        
        return (
            <div className=''>
                <div className='flex'>
                    <div className='w-1/5'>{props.text}</div>
                    <div className='w-4/5 flex gap-2'>
                        <div className="inputFieldDropDown flex justify-between cursor-pointer" onClick={openCloseDropDown}>
                            <div id={'value' + props.text} value={value} className='text-subGray2'>
                                {value}                   
                            </div>
                            <FontAwesomeIcon icon={faAngleDown} className="fa-solid fa-xl my-auto" />
                        </div>
                            <div onClick={() => handleExtraAdd(value)} className='inputField h-[45px] w-[45px] flex justify-center content-center flex-wrap aspect-square '>
                                <FontAwesomeIcon icon={faPlus} className=" fa-lg m-auto cursor-pointer"/>
                            </div> 
                    </div>
                </div>
                <div id={'dropdown' + props.text} className='w-4/5-45px ml-auto mr-[45px] bg-gray-200 text-sm rounded-b-lg -mt-5 p-2.5 pt-[30px] hidden'>
                    <GetOptions options={props.options} />
                </div>
                <div className='flex flex-wrap w-4/5 ml-auto'>
                    {extraList.map((singleExtra, index) => (
                        <div id='extra' key={index}>
                            {singleExtra.extra && <div><InputFieldAddition key={index} value={singleExtra.extra} onClick={handleExtraRemove}/></div>}
                        </div>
                        
                    ))}
                </div>
            </div>
        )
    }else if(type === "small"){
        return (
            <div className='flex w-2/5'>
                <div className='w-1/2'>{props.text}</div>
                <input value={props.value} onChange={props.onChange} type="text" className="inputField w-1/2 h-12" placeholder={props.placeholder} required={props.required}></input>
            </div>
            );
    }else if(type === "big"){
        return (
            <div className='flex col-span-2'>
                <div className='w-[10%]'>{props.text}</div>
                <textarea value={props.value} onChange={props.onChange} type="text" className="inputField w-[90%] h-32" placeholder={props.placeholder} required={props.required}></textarea>
            </div>
            );
    }else{
        return (
            <div className='flex'>
                <div className='w-1/5'>{props.text}</div>
                <input value={props.value} onChange={props.onChange} type="text" className="inputField w-4/5 h-12" placeholder={props.placeholder} required={props.required}></input>
            </div>
            );
    } 
}