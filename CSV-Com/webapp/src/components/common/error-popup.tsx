import CvsError from 'types/common/cvs-error';
import './error-popup.css';
import { Button } from 'components/common/button';
import { Header } from 'components/common/header';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTriangleExclamation, faXmark} from "@fortawesome/free-solid-svg-icons";

interface IErrorPopupProps{
    error: CvsError;
    className?: string;
    isOpen: boolean;
    onClose: () => void;
}

const ErrorPopup = (props: IErrorPopupProps) => {    
    return (
        <>
        {props.isOpen && (
        <div>
            <div className='fixed inset-0 bg-gray-800 bg-opacity-50 z-50"' onClick={props.onClose} />

            <div className={`error-popup ${props.className}`}>
                <FontAwesomeIcon icon={faXmark} className="flex-none fa-solid fa-xl close-icon" onClick={props.onClose} />
                <FontAwesomeIcon icon={faTriangleExclamation} className="flex-none fa-solid fa-xl text-red-600 m-2"/>
                <Header text="Er is een fout opgetreden" className='flex-auto' />
                <p className='error-message'>{props.error.message}</p>
                <p className='error-code'>Foutcode: {props.error.errorcode}</p>
                <Button buttonType={{type:"Solid"}} text="Sluiten" className='confirm-button' onClick={props.onClose} />
            </div>
        </div>
        )}
        </>
    );
};

export default ErrorPopup;