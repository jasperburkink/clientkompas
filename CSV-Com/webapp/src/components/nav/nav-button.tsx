import '../../index.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUsers, faClock, faBuilding, faUser, faArrowRightFromBracket } from "@fortawesome/free-solid-svg-icons";
import { faArrowRight } from "@fortawesome/free-solid-svg-icons";
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { Link } from 'react-router-dom';

interface NavButtonProps {
    text: string;
    icon: string;
    to: string;
  }

function GetIcon(icon: string): IconDefinition{

    switch (icon) {
        case "Gebruikers":
            return faUsers;
        case "Klok":
            return faClock;
        case "Gebouw":
            return faBuilding;
        case "Gebruiker":
            return faUser;
        case "Uitloggen":
            return faArrowRightFromBracket;
        default:
            return faUsers;
    }
}

export const NavButton: React.FC<NavButtonProps> = (props: NavButtonProps) => {
    return (
        <Link to={props.to} className="navBtn group ">
            <FontAwesomeIcon icon={GetIcon(props.icon)} className="leading-none h-6" />
            <div className="w-1/2">{props.text}</div>
            <FontAwesomeIcon icon={faArrowRight} className='navBtnArrow'/>
        </Link>
    );
}