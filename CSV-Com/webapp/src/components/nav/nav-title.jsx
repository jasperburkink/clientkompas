import '../../index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus } from "@fortawesome/free-solid-svg-icons";
import React from 'react';

export function NavTitle(props) {
    return (
        <React.Fragment>
            <div className="flex justify-between px-1">
                <div className="text-lg font-semibold">{props.lijstNaam}</div>
                <FontAwesomeIcon icon={faPlus} className="fa-lg my-auto cursor-pointer" onClick={event =>  window.location.href=[props.path]}/>
            </div>
            <hr className="hidden md:block h-px bg-black border-0 -mt-1"/>
        </React.Fragment>
    )
}