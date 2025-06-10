import React from "react";
import "./text-area.css";
import { ErrorMessage } from "./error-message";
import { ValidationError } from "types/common/validation-error";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTriangleExclamation } from "@fortawesome/free-solid-svg-icons";

export interface TextareaProps {
  value?: string,
  placeholder: string,
  className?: string,
  onChange?: (value: string) => void,
  dataTestId?: string;
  errors?: ValidationError[];
}

const Textarea = (props: TextareaProps) => {
  return (
    <div className='text-area-container' style={{ flex: 1 }}>
      <textarea
        className={`text-area ${props.errors ? 'error' : ''}`}
        name="postContent"
        rows={4}
        cols={20}
        value={props.value}
        placeholder={props.placeholder} 
        onChange={(e) => {props.onChange?.(e.target.value);}}
        data-testid={props.dataTestId} />
        <FontAwesomeIcon icon={faTriangleExclamation} className={`error-icon fa-lg ${props.errors ? 'visible' : 'hidden'}`} />
      <ErrorMessage errors={props.errors} />
    </div>
  );
};

export default Textarea;
