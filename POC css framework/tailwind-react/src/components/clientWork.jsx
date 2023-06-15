import '../index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleDown, faPlus } from "@fortawesome/free-solid-svg-icons";

import React, { useEffect, useState } from "react";
import { InputFieldAddition } from './inputFieldAddition';

const getAllWorkInfo = (props, naam) =>{
    if(props.client.workingContracts[props.id]){
        let allInfo = ""
        allInfo += props.client.workingContracts[props.id][naam] + " "
        return(allInfo)
    }else{
        return("/")
    }
}

export function ClientWork(props) {  
    return(
        <ul className="twoSpaceUlBox">
            <li>Werkt bij: {getAllWorkInfo(props, "companyName")}</li>
            <li>Contract: {getAllWorkInfo(props, "contractType")}</li> 
            <li>Van: {getAllWorkInfo(props, "fromDate")}</li> 
            <li>Tot: {getAllWorkInfo(props, "toDate")}</li> 
            <li>Functie: {getAllWorkInfo(props, "function")}</li>
        </ul>
    )
}