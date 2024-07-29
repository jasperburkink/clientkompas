import React from "react";
import "./text-area.css";
import { ErrorMessage } from "./error-message";

export interface TextareaProps {
  value?: string,
  placeholder: string,
  className?: string,
  onChange?: (value: string) => void,
  dataTestId?: string;
  error?: string;
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
      {props.error && <ErrorMessage error={props.error} />}
    </div>
  );
};

export default Textarea;
