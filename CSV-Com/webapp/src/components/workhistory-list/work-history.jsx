import '../../index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleDown, faPlus } from "@fortawesome/free-solid-svg-icons";

import React, { useEffect, useState } from "react";
import { InputFieldAddition } from '../common/input-field-addition';

const optionsWork = [{name: "SBICT", value : 0}]
const optionsContract = [{name: "Temporary", value : 0}, {name: "Permanent", value: 1}]

const getAllWorkInfo = (props, naam) =>{
    if(props.client.workingContracts[props.id]){
        let allInfo = ""
        allInfo += props.client.workingContracts[props.id][naam] + " "
        return(allInfo)
    }else{
        return("/")
    }
}
const getNameOfValue = (client, options) => {
    client = parseInt(client)
    if(client || client === 0){
        let nameOfValue = ""
        nameOfValue += options[client].name
        return(nameOfValue)
    }else{
        return("/")
    }
}

export function ClientWork(props) {  
    return(
        <ul className="twoSpaceUlBox">
            <li>Werkt bij: {getAllWorkInfo(props, "companyName")}</li>
            <li>Contract: {getNameOfValue(getAllWorkInfo(props, "contractType"), optionsContract)}</li> 
            <li>Van: {getAllWorkInfo(props, "fromDate")}</li> 
            <li>Tot: {getAllWorkInfo(props, "toDate")}</li> 
            <li>Functie: {getAllWorkInfo(props, "function")}</li>
        </ul>
    )
}