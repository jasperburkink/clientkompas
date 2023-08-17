import '../index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowRight } from "@fortawesome/free-solid-svg-icons";


export function NavButtonGray(props) {
    return (
        <div onClick={event =>  window.location.href=[props.path]} className="navBtnGray group ">
            <div className="w-fit max-w-[90%]">{props.text}</div>
            <FontAwesomeIcon icon={faArrowRight} className="navBtnArrow" />
        </div>
    );
}