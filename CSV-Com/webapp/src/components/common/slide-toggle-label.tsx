import React, { useState, ReactNode } from 'react';
import { Collapse } from 'react-collapse';
import './slide-toggle-label.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleDown, faAngleUp } from "@fortawesome/free-solid-svg-icons";

interface SlideToggleLabelProps extends React.HtmlHTMLAttributes<HTMLElement> {
    textColapsed: string,
    textExpanded: string,
    children: ReactNode
}
export const SlideToggleLabel = (props: SlideToggleLabelProps) => {
    const [isOpen, setIsOpen] = useState<boolean>(true);

    const toggleOpen = () => {
        setIsOpen(prev => !prev);
    };

    const icon = isOpen ? faAngleUp : faAngleDown;
    const text = isOpen ? props.textExpanded : props.textColapsed;

    return (
        <div className={props.className}>            
            <Collapse isOpened={isOpen} theme={{ collapse: 'toggle-animation' }}>
                    {props.children}
            </Collapse>
            <button className='toggle-button' onClick={toggleOpen} aria-expanded={isOpen}>
                {text} <FontAwesomeIcon icon={icon}  /> 
                <span className="toggle-button-underline"></span>
            </button>
        </div>
    );
};