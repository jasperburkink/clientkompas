import React, { useState, ReactNode } from 'react';
import { Collapse } from 'react-collapse';
import './slide-toggle-label.css';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleDown, faAngleRight, faAngleUp } from "@fortawesome/free-solid-svg-icons";

interface SlideToggleLabelProps extends React.HtmlHTMLAttributes<HTMLElement> {
    text: string,
    smallTextColapsed: string,
    smallTextExpanded: string,
    children: ReactNode
}
export const SlideToggleLabel = (props: SlideToggleLabelProps) => {
    const [isOpen, setIsOpen] = useState<boolean>(true);

    const toggleOpen = () => {
        setIsOpen(prev => !prev);
    };

    const icon = isOpen ? faAngleRight : faAngleDown;
    const smalltext = isOpen ? props.smallTextExpanded : props.smallTextColapsed;
    const text = props.text;

    return (
        <div className={props.className}>      
            <button className='toggle-button' onClick={toggleOpen} aria-expanded={isOpen}>
                {text} <span id='toggle-smalltext'>{smalltext}</span> <FontAwesomeIcon icon={icon}  /> 
            </button>      
            
            <Collapse isOpened={isOpen} theme={{ collapse: 'toggle-animation' }}>
                    {props.children}
            </Collapse>
        </div>
    );
};