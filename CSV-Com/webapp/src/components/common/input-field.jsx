import '../../index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSortDown } from "@fortawesome/free-solid-svg-icons";

export function InputField(props) {  
    const openDropDown = () => {
        const test = document.getElementById("dropdown").style.visibility
        if(test === "visible")
        {
            document.getElementById("dropdown").style.visibility = "hidden"
        }else{
            document.getElementById("dropdown").style.visibility = "visible"
        }

    }
    var type = props.type
    if(type === "dropdown"){
        return (
        <div className='md:col-span-2'>
            <button type="text" className="md:col-span-2 inputField flex justify-between" placeholder={props.placeholder} onClick={openDropDown}>
                Traject Naam <FontAwesomeIcon icon={faSortDown} className="fa-solid fa-xl my-auto" />
                </button>
                
                    <div id='dropdown' className='absolute bg-mainLightGray w-full p-2.5 invisible'>
                        {props.children}
                    </div>
                
            </div>
        )
    }else{
        return (<input type="text" className="inputField w-32 h-10" placeholder={props.placeholder} required></input> );        
    } 
}