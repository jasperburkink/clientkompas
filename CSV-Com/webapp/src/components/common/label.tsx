import React, { LabelHTMLAttributes } from 'react';
import './label.css';
import { ValidationError } from 'types/common/validation-error';
import { ErrorMessage } from './error-message';

interface LabelProps extends React.LabelHTMLAttributes<HTMLParagraphElement> {
    text: string,
    strong?: boolean,
    underline?: boolean,
    cursive?: boolean,
    dataTestId?: string;
    errors?: ValidationError[];
}

export const Label = (props: LabelProps) => (
    <div className={"label " + props.className}>
        <p className={getLabelClassName(props) } data-testid={props.dataTestId}>
            {props.text}
        </p>
        <ErrorMessage errors={props.errors} />
    </div>
  );

function getLabelClassName(props: LabelProps): string | undefined {
    return (props.strong === true ?
        "label-strong " : "") +
        (props.underline === true ? "label-underline " : "") +
        (props.cursive === true ? "label-cursive " : "");
}
