import CvsError from '../../types/common/cvs-error';
import './error-popup.css';
import { Button } from '../../components/common/button';
import { Header } from '../../components/common/header';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTriangleExclamation, faPlus, faXmark} from "@fortawesome/free-solid-svg-icons";

interface IErrorPopupProps{
    error: CvsError;
    className?: string;
    isErrorPopupOpen: boolean;
    setErrorPopupOpen: (isOpen: boolean) => void;
}

const ErrorPopup = (props: IErrorPopupProps) => {
    return (
        <>
        {props.isErrorPopupOpen && (
        <div>
            <div className='fixed inset-0 bg-gray-800 bg-opacity-50 z-50"' onClick={() => props.setErrorPopupOpen(false)} />

            <div className={`error-popup ${props.className}`}>
                <FontAwesomeIcon icon={faXmark} className="flex-none fa-solid fa-xl close-icon" onClick={() => props.setErrorPopupOpen(false)} />                
                <FontAwesomeIcon icon={faTriangleExclamation} className="flex-none fa-solid fa-xl text-red-600 m-2"/>
                <Header text="Er is een fout opgetreden" className='flex-auto' />
                <p className='error-message'>{props.error.message}</p>
                <p className='error-code'>Foutcode: {props.error.errorcode}</p>
                <Button buttonType={{type:"Solid"}} text="Sluiten" className='ml-auto w-32 h-10' onClick={() => props.setErrorPopupOpen(false)} />
            </div>
        </div>
        )}
        </>
    );
};

export default ErrorPopup;