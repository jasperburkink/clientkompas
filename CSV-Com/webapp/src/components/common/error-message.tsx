import { ValidationError } from 'types/common/validation-error';
import './error-message.css';

export interface ErrorMessageProps {
    errors?: ValidationError[];
}

export const ErrorMessage = (props: ErrorMessageProps) => {
    return(
    <span className="error-message">        
    {props.errors !== undefined && (
        <ul>
            {props.errors.map((error, index) => (
                <li key={index}>
                    {error.errormessage}
                </li>
            ))}
        </ul>
    )}
    </span>
)};