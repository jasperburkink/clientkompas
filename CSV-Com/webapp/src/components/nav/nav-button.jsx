import '../../index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUsers, faClock, faBuilding, faUser, faArrowRightFromBracket } from "@fortawesome/free-solid-svg-icons";
import { faArrowRight } from "@fortawesome/free-solid-svg-icons";

function GetIcon(props){
    let icon;
    switch (props.icon) {
        case "Gebruikers":
            icon = faUsers;
            break;
        case "Klok":
            icon = faClock;
            break;
        case "Gebouw":
            icon = faBuilding;
            break;
        case "Gebruiker":
            icon = faUser;
            break;
        case "Uitloggen":
            icon = faArrowRightFromBracket;
            break;
    }

    return <FontAwesomeIcon icon={icon} className="leading-none h-6" />
}

export function NavButton(props) {
    return (
        <div className="navBtn group ">
            <GetIcon icon={props.icon} />
            <div className="w-1/2">{props.text}</div>
            <FontAwesomeIcon icon={faArrowRight} className='navBtnArrow'/>
        </div>
    );
}