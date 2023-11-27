import React, { LabelHTMLAttributes } from 'react';
import './label.css';

interface LabelProps extends React.LabelHTMLAttributes<HTMLParagraphElement> {
    text: string,
    strong?: boolean,
    underline?: boolean,
    cursive?: boolean    
}

export const Label = (props: LabelProps) => (
    <div className={"label " + props.className}>
        <p className={getLabelClassName(props) }>
            {props.text}
        </p>
    </div>
  );

function getLabelClassName(props: LabelProps): string | undefined {
    return (props.strong === true ?
        "label-strong " : "") +
        (props.underline === true ? "label-underline " : "") +
        (props.cursive === true ? "label-cursive " : "");
}
