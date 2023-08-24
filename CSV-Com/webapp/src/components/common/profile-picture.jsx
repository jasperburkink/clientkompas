import '../../index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCircleUser } from "@fortawesome/free-solid-svg-icons";

export function ProfilePicture() {
    return (
        <div className='h-fit hidden md:block'>
            <div className="profilePictureBox">
                <FontAwesomeIcon icon={faCircleUser} className="fa-sollid fa-10x text-white"/>
            </div>
            <div className='flex justify-between'>
                <div className="text-sky-500 underline">Uploaden</div>
                <div className="text-sky-500 underline">Verwijderen</div>               
            </div>
        </div>
    );
}