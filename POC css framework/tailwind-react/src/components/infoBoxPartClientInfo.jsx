import '../index.css';
import React, { useEffect, useState } from "react";
import { ClientWork } from './clientWork';

export function InfoBoxPartClientInfo(props) {
    const [werkLijst, setwerkLijst] = useState([])
    
    if(props.client == null) {
        return "loading...";
    } 

    const getAllInfo = (soort, naam) =>{
            if(props.client[soort].length > 0){
            let allInfo = []
            for (let i = 0; i < props.client[soort].length; i++) {
                allInfo += props.client[soort][i][naam] + " "
            }
            return(allInfo)
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
        console.log(allInfo)
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
                <li className='mt-3 md:hidden'>Burgelijke staat: {props.client.maritalStatus}</li>
                <li className='md:hidden'>Rijbewijs: {props.client.driversLicences[0] ? props.client.driversLicences[0].driversLicenceCode : "geen"}</li>
            </ul>
            <ul className="twoSpaceUlBox">
                <li className="md:col-span-2 font-bold pt-3 md:p-0">In geval van nood</li>
                <li>{getAllInfo("emergencyPeople", "name")}</li>
                <li className='hidden md:block'>Burgelijke staat: {props.client.maritalStatus}</li>
                <li>Mobiel: {getAllInfo("emergencyPeople", "telephoneNumber")}</li>
                <li className='hidden md:block'>Rijbewijs: {getAllInfo("driversLicences", "driversLicenceCode")}</li>
            </ul>
            <ul className="twoSpaceUlBox">
                <li className="md:col-span-2 font-bold pt-3 md:p-0">Overige informatie</li>
                <li>Diagnose(s): {getAllInfo("diagnoses", "name")}</li>
                <li>Uitkeringsvorm: {props.client.benefitForm}</li>
            </ul>
            <GetWork/>
                
            <ul className="h-fit shrink-0">
                <li className="md:col-span-2 font-bold pt-3 md:p-0">Opmerkingen</li>
                <li className="md:col-span-2">{props.client.remarks}</li>
            </ul>
        </div>
        
    );
}