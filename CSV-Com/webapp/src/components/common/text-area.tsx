import React from "react";
import "./text-area.css";
import { ErrorMessage } from "./error-message";
import { ValidationError } from "types/common/validation-error";

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
        className="text-area"
        name="postContent"
        rows={4}
        cols={20}
        value={props.value}
        placeholder={props.placeholder} 
        onChange={(e) => {props.onChange?.(e.target.value);}}
        data-testid={props.dataTestId}
      />
      <ErrorMessage errors={props.errors} />
    </div>
  );
};

export default Textarea;
