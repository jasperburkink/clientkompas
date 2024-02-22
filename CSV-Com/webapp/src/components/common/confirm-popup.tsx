import './confirm-popup.css';
import { Button, ButtonProps } from '../../components/common/button';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faXmark} from "@fortawesome/free-solid-svg-icons";

interface IConfirmPopupProps{
    message: string;
    className?: string;
    isOpen: boolean;
    buttons: ButtonProps[];
    onClose: () => void;
}

const ConfirmPopup = (props: IConfirmPopupProps) => {    
    return (
        <>
        {props.isOpen && (
        <div>
            <div className='fixed inset-0 bg-gray-800 bg-opacity-50 z-50"' onClick={props.onClose} />

            <div className={`confirm-popup ${props.className}`}>
                <FontAwesomeIcon icon={faXmark} className="flex-none fa-solid fa-xl close-icon" onClick={props.onClose} />
                <p className='confirm-message'>{props.message}</p>
                <div className='confirm-button-container'>
                    {props.buttons.map((buttonComponent, index) => (
                        <Button key={index} {...buttonComponent} className='confirm-button'/>
                    ))}
                </div>
            </div>
        </div>
        )}
        </>
    );
};

export default ConfirmPopup;