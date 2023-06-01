import '../index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUsers, faClock, faBuilding, faUser, faArrowRightFromBracket } from "@fortawesome/free-solid-svg-icons";
import { faArrowRight } from "@fortawesome/free-solid-svg-icons";

function GetIcon(props){
    var icon = props.icon
    if(icon === "Gebruikers"){
        return <FontAwesomeIcon icon={faUsers} className="leading-none h-6" />
    }else if(icon === "Klok"){
        return <FontAwesomeIcon icon={faClock} className="leading-none h-6" />
    }else if(icon === "Gebouw"){
        return <FontAwesomeIcon icon={faBuilding} className="leading-none h-6" />
    }else if(icon === "Gebruiker"){
        return <FontAwesomeIcon icon={faUser} className="leading-none h-6" />
    }else if(icon === "Uitloggen"){
        return <FontAwesomeIcon icon={faArrowRightFromBracket} className="leading-none h-6" />
    }
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