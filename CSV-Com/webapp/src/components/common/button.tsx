import React from 'react';
import './button.css';
import { ButtonType } from 'types/common/ButtonComponentType';
import { getClassNameButtonType } from 'types/common/ButtonComponentType';

export interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
    text: string;
    buttonType: ButtonType;
    isOpen?: boolean;
    setIsOpen?: React.Dispatch<React.SetStateAction<boolean>>;
    dataTestId?: string;
}

export const Button = (props: ButtonProps) => {
    const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
        if (props.setIsOpen) {
            props.setIsOpen((prevIsOpen) => !prevIsOpen);
        }

        if (props.onClick) {
            props.onClick(event);
        }
    };

    const combinedClassName = getClassNameButtonType(props.buttonType.type) + (props.className ? ` ${props.className}` : '');

    return (
        <button
            onClick={handleClick}
            className={combinedClassName}
            data-testid={props.dataTestId}
        >
            {props.text}
        </button>
    );
};
