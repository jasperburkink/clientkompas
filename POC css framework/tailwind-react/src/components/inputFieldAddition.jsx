import '../index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faXmark } from "@fortawesome/free-solid-svg-icons";

export function InputFieldAddition(props) {  
    return(
        <div className='flex w-fit gap-2 bg-gray-200 p-2'>
            <div>{props.value}</div>
            <FontAwesomeIcon onClick={props.onClick} icon={faXmark} className="h-4 w-fit m-auto" />
        </div>
    )
}