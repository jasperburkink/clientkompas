import React from "react";
import "./text-area.css";

export interface TextareaProps {
  value?: string,
  placeholder: string,
  className?: string,
  onChange?: (value: string) => void,
  dataTestId?: string;
}

const Textarea = (props: TextareaProps) => {
  return (
    <div style={{ flex: 1 }}>
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
    </div>
  );
};

export default Textarea;
