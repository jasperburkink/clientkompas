import '../index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faArrowRight } from "@fortawesome/free-solid-svg-icons";

export function NavButtonGray(props) {
    return (
        <div className="navBtnGray group ">
            <div className="w-1/2">{props.text}</div>
            <FontAwesomeIcon icon={faArrowRight} className="navBtnArrow" />
        </div>
    );
}