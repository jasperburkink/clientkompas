export interface ErrorMessageProps {
    error?: string;
}

export const ErrorMessage = (props: ErrorMessageProps) => {
    return(
    <span className="error-message">{props.error}</span>
)};