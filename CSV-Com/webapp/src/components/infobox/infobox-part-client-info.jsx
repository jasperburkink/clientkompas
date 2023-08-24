import '../../index.css';
import React, { useEffect, useState } from "react";
import { ClientWork } from '../workhistory-list/work-history';

export function InfoBoxPartClientInfo(props) {
    const [werkLijst, setwerkLijst] = useState([])

    const optionsSex = [{name: "man", value : 0}, {name: "female", value: 1}, {name: "nonbinary", value: 2}]
    const optionsMaritalStatus = [{name: "unmarried", value : 0}, {name: "Married", value: 1}, {name: "Divorced", value: 2}, {name: "Widowed", value: 3}, {name: "RegistratedPartner", value: 4}]
    const optionsDiagnoses = [{name: "ADHD", value : 0}, {name: "Autisme", value: 1}, {name: "ADD", value: 2}]
    const optionsBenefitForm = [{name: "WW", value : 0}, {name: "Wia", value: 1}, {name: "WaJong", value: 2}, {name: "Bijstand", value: 3}, {name: "Geen", value: 5}]
    
    const optionsDriversLicences = [{name: "AM", value : 0}, {name: "A1", value: 1}, {name: "A2", value: 2}, {name: "A", value: 3}, 
    {name: "B", value: 4}, {name: "C1", value: 5}, {name: "C", value: 6}, {name: "D1", value: 7}, {name: "D", value: 8}, {name: "BE", value: 9},
    {name: "C1E", value: 10}, {name: "CE", value: 11}, {name: "D1E", value: 12}, {name: "DE", value: 13}, {name: "T", value: 14}]
    
    //if(props.client == null) {
    //    return "loading...";
    //} 

    const getAllInfo = (soort, naam, options) =>{
        if(props.client[soort].length > 0){
            let allInfo = []
            for (let i = 0; i < props.client[soort].length; i++) {
                if(options){
                    allInfo += getNameOfValue(props.client[soort][i][naam], options) + " "
                    //allInfo = ([...allInfo, {"naam": getNameOfValue(props.client[soort][i][naam], options), [naam]: props.client[soort][i][naam]}])
                }else{
                    allInfo += props.client[soort][i][naam] + " "
                }
            }
            return(allInfo)
        }else{
            return("/")
        }
    }
    const getNameOfValue = (client, options) => {
        if(client || client === 0){
            let nameOfValue = ""
            nameOfValue += options[client].name
            return(nameOfValue)
        }else{
            return("/")
        }
    }
    const GetWork = () =>{
        if(props.client.workingContracts.length > 0){
        let allInfo = []
        for (let i = 0; i < props.client.workingContracts.length; i++) {
            allInfo.push(<ClientWork key={i} client={props.client} id={i}/>)
        }
        return(allInfo)
    }else{
        return("/")
    }
}

    return (
        <div className="p-3 md:p-0 md:overflow-hidden w-screen md:w-full h-fit md:h-full gap-3 flex flex-col justify-between">
            <ul className={`twoSpaceUlBox`}>           
                <li className="pieceTitle">Cliënt info</li>
                <li className='md:order-1'>{props.client.firstName} {props.client.prefixLastName} {props.client.lastName}</li>
                <li className='md:order-3'>{props.client.streetName} {props.client.houseNumber}{props.client.houseNumberAddition}</li>
                <li className='md:order-5'>{props.client.postalCode} {props.client.residence}</li>
                <li className='md:order-7'></li>
                <li className='md:order-8 my-3 md:m-0'>BSN: {props.client.identificationNumber}</li>
                <li className='md:order-2'>Mobiel: {props.client.mobileNumber} {props.client.telephoneNumber}</li>
                <li className='md:order-4'>Email: {props.client.emailAddress}</li>
                <li className='md:order-6'>Geboortedatum: {props.client.dateOfBirth}</li>
                <li className='mt-3 md:hidden'>Burgelijke staat: {getNameOfValue(props.client.maritalStatus, optionsMaritalStatus)}</li>
                <li className='md:hidden'>Rijbewijs: {props.client.driversLicences[0] ? props.client.driversLicences[0].driversLicenceCode : "geen"}</li>
            </ul>
            <ul className="twoSpaceUlBox">
                <li className="md:col-span-2 font-bold pt-3 md:p-0">In geval van nood</li>
                <li>{getAllInfo("emergencyPeople", "name")}</li>
                <li className='hidden md:block'>Burgelijke staat: {getNameOfValue(props.client.maritalStatus, optionsMaritalStatus)}</li>
                <li>Mobiel: {getAllInfo("emergencyPeople", "telephoneNumber")}</li>
                <li className='hidden md:block'>Rijbewijs: {getAllInfo("driversLicences", "driversLicenceCode", optionsDriversLicences)}</li>
            </ul>
            <ul className="twoSpaceUlBox">
                <li className="md:col-span-2 font-bold pt-3 md:p-0">Overige informatie</li>
                <li>Diagnose(s): {getAllInfo("diagnoses", "name")}</li>
                <li>Uitkeringsvorm: {getNameOfValue(props.client.benefitForm, optionsBenefitForm)}</li>
            </ul>
            <GetWork/>
                
            <ul className="h-fit shrink-0">
                <li className="md:col-span-2 font-bold pt-3 md:p-0">Opmerkingen</li>
                <li className="md:col-span-2">{props.client.remarks}</li>
            </ul>
        </div>
        
    );
}