import '../../index.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUsers, faClock, faBuilding, faUser, faArrowRightFromBracket, faStethoscope, faRing, faCar, faCertificate, faPiggyBank, faHandsHelping } from "@fortawesome/free-solid-svg-icons";
import { faArrowRight } from "@fortawesome/free-solid-svg-icons";
import { IconDefinition } from '@fortawesome/fontawesome-svg-core';
import { Link, useNavigate } from 'react-router-dom';

interface NavButtonProps {
    text: string;
    icon: string;
    to: string;
    onClick?: (event: React.MouseEvent) => void;
  }

function GetIcon(icon: string): IconDefinition{

    switch (icon) {
        case "Licence":
            return faCertificate;
        case "Diagnosis":
            return faStethoscope;
        case "MaritalStatus":
            return faRing;
        case "PiggyBank":
            return faPiggyBank;
        case "DriversLicence":
            return faCar;
        case "HelpingHands":
            return faHandsHelping;
        case "Users":
            return faUsers;
        case "Clock":
            return faClock;
        case "Building":
            return faBuilding;
        case "User":
            return faUser;
        case "Exit":
            return faArrowRightFromBracket;
        default:
            return faUsers;
    }
}

export const NavButton: React.FC<NavButtonProps> = (props: NavButtonProps) => {
    const navigate = useNavigate();

    const handleClick = (event: React.MouseEvent) => {
        if (props.onClick) {
            event.preventDefault();
            props.onClick(event);
        } else {
            navigate(props.to);
        }
    };
    
    return (
        <Link to={props.to} onClick={handleClick} className="navBtn group ">
            <FontAwesomeIcon icon={GetIcon(props.icon)} className="leading-none h-6" />
            <div className="w-1/2">{props.text}</div>
            <FontAwesomeIcon icon={faArrowRight} className='navBtnArrow'/>
        </Link>
    );
}