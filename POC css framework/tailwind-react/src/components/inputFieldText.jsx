import '../index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleDown, faPlus } from "@fortawesome/free-solid-svg-icons";

import React, { useEffect, useState } from "react";
import { InputFieldAddition } from './inputFieldAddition';

export function InputFieldText(props) {  
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
    // useEffect(() => {
    //     setValueDropdown();
    // }, [])
    const setValueDropdown = (newValue) =>{
        setValue(newValue)
        openCloseDropDown()
        document.getElementById("value" + props.text).style.color = "black"
    }
    const handleExtraAdd = (value) => {
        console.log(value)
        setExtraList([...extraList, {extra: [value]}])
    }
    const handleExtraRemove = (index) => {
        const list = [...extraList]
        list.splice(index, 1)
        setExtraList(list)
    }
    // const handleTestChange = (e, index) => {
    //     console.log(index)
    //     const {name, value} = e.target;
    //     const list = [...testList];
    //     list[index][name] = value;
    //     setTestList(list);
    // }
    var type = props.type
    if(type === "dropdown"){
        return (
        <div className='md:col-span-2'>
            <div className='flex'>
                <div className='w-1/5'>{props.text}</div>
                <div className="md:col-span-2 inputField w-4/5 flex justify-between cursor-pointer" onClick={openCloseDropDown}>
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
                        <div className="inputFieldDropDown flex justify-between cursor-pointer" onClick={openCloseDropDown}>
                            <div id={'value' + props.text} className='text-subGray2'>
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
                    {props.children}
                    <div onClick={ () => setValueDropdown("Test")} value="Test" className='cursor-pointer'>Test1</div>
                </div>
                <div className='flex flex-wrap w-4/5 ml-auto'>
                    {extraList.map((singleExtra, index) => (
                        <div id='test' key={index}>
                            {singleExtra.extra && <div><InputFieldAddition key={index} value={singleExtra.extra} onClick={handleExtraRemove}/></div>}
                        </div>
                        
                    ))}
                </div>
                    {/* {testList.map((singleTest, index) => (
                        <div key={index} id='test'>
                            <input name='test' type="text" placeholder='Test' className='' id='test'
                            value={singleTest.test}
                            onChange = {(e) => handleTestChange(e, index)}
                            />
                            Test
                            <div>
                                {testList.length > 1 && (
                                    <div onClick={() => handleTestRemove(index)}>X</div>
                                )}
                            </div>
                            {testList.length - 1 === index && testList.length < 4 && 
                                (<button className='btnSollid' onClick={handleTestAdd}>+</button>)
                            }
                        </div>
                    ))} */}
                
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