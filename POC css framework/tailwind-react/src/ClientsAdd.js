import './index.css';

import { Copyright } from './components/copyright';
import { Button } from './components/button';
import React, { useEffect, useState } from "react";
import { useParams } from 'react-router-dom';
import { InputFieldText } from './components/inputFieldText';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faXmark } from "@fortawesome/free-solid-svg-icons";
import { SidebarFull } from './components/sidebarFull';
import { InputFieldAddition } from './components/inputFieldAddition';

function ClientsAdd() {
    var [ client, setClient ] = useState(null);
    const { id } = useParams();
    const optionsSex = [{name: "man", value : 0}, {name: "female", value: 1}, {name: "nonbinary", value: 2}]
    const optionsMaritalStatus = [{name: "unmarried", value : 0}, {name: "Married", value: 1}, {name: "Divorced", value: 2}, {name: "Widowed", value: 3}, {name: "RegistratedPartner", value: 4}]
    const optionsDiagnoses = [{name: "ADHD", value : 0}, {name: "Autisme", value: 1}, {name: "ADD", value: 2}]
    const optionsBenefitForm = [{name: "WW", value : 0}, {name: "Wia", value: 1}, {name: "WaJong", value: 2}, {name: "Bijstand", value: 3}, {name: "Geen", value: 5}]
    const optionsWork = [{name: "SBICT", value : 0}]
    const optionsContract = [{name: "Temporary", value : 0}, {name: "Permanent", value: 1}]
    const optionsDriversLicences = [{name: "AM", value : 0}, {name: "A1", value: 1}, {name: "A2", value: 2}, {name: "A", value: 3}, 
    {name: "B", value: 4}, {name: "C1", value: 5}, {name: "C", value: 6}, {name: "D1", value: 7}, {name: "D", value: 8}, {name: "BE", value: 9},
    {name: "C1E", value: 10}, {name: "CE", value: 11}, {name: "D1E", value: 12}, {name: "DE", value: 13}, {name: "T", value: 14}]

    const [identificationNumber, setIdentificationNumber] = useState('');
    const [firstName, setFirstName] = useState('');
    const [initials, setInitials] = useState('');
    const [prefixLastName, setPrefixLastName] = useState('');
    const [lastName, setLastName] = useState('');
    const [sex, setSex] = useState(0);
    const [streetName, setStreetName] = useState('');
    const [houseNumber, setHouseNumber] = useState(0);
    const [houseNumberAddition, setHouseNumberAddition] = useState('');
    const [postalCode, setPostalCode] = useState('');
    const [residence, setResidence] = useState('');
    const [telephoneNumber, setTelephoneNumber] = useState('');
    const [emailAddress, setEmailAddress] = useState('');
    const [maritalStatus, setMaritalStatus] = useState('');
    const [driversLicences, setDriversLicences] = useState([]);
    const [emergencyPeople, setEmergencyPeople] = useState([]);
    const [diagnoses, setDiagnoses] = useState([]);
    const [benefitForm, setBenefitForm] = useState('');
    const [workingContracts, setWorkingContracts] = useState([]);
    const [remarks, setRemarks] = useState('');
//, {mode: 'cors'}
    useEffect(() => {
        fetch("https://localhost:7017/api/Client")
            .then(response => {
                return response.json();
            })
            .then(data => {
                console.log(data);
                setClient(data);
                if(id){
                    setIdentificationNumber(data[id].identificationNumber)
                    setFirstName(data[id].firstName)
                    setInitials(data[id].initials)
                    setPrefixLastName(data[id].prefixLastName)
                    setLastName(data[id].lastName)
                    setStreetName(data[id].streetName)
                    setHouseNumber(data[id].houseNumber)
                    setHouseNumberAddition(data[id].houseNumberAddition)
                    setPostalCode(data[id].postalCode)
                    setResidence(data[id].residence)
                    setTelephoneNumber(data[id].telephoneNumber)
                    setEmailAddress(data[id].emailAddress)
                    setMaritalStatus(data[id].maritalStatus)
                    setDriversLicences(getAllInfo(data[id], "driversLicences", "driversLicenceCode"))
                }
            })
            .catch(error => {
                console.error(error);
            });
    }, 
    [] // Voert nu maar 1 keer uit als het component gerenderd wordt
    ); 
    const getAllInfo = (client, soort, naam) =>{
        if(client[soort].length > 0){
            let allInfo = ""
            for (let i = 0; i < client[soort].length; i++) {
                allInfo += client[soort][i][naam] + " "
            }
            return(allInfo)
        }else{
            return("/")
        }
    }
    
    const handleSubmit = (e) => {
        e.preventDefault();
        const newClient = {identificationNumber, firstName, initials, prefixLastName, lastName, streetName, houseNumber, houseNumberAddition, postalCode, residence, telephoneNumber, emailAddress, remarks}
        console.log(newClient)
        
//, {mode: 'cors'}
const headers = {
    'Accept': 'application/json',
    'Accept' : '*/*',
    'accept':'application/json',
    'content-type':'application/json',
    'Access-Control-Allow-Origin':'https://localhost:7017',
    'Access-Control-Allow-Origin':'https://localhost:3000',
    "Access-Control-Allow-Origin": "*",
    "Content-Type" : "application/json",

    "Access-Control-Allow-Origin": "*",
    "Access-Control-Allow-Credentials": "true",
    "Access-Control-Allow-Methods": "GET,HEAD,OPTIONS,POST,PUT",
    "Access-Control-Allow-Headers": "Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers"
    
}
        fetch("https://localhost:7017/api/Client", {
            method: 'POST',
            mode: 'cors',
            headers: headers,
            body: JSON.stringify(newClient)
        }).then((response) =>{
            console.log("new client added")
        }).catch(error => {
            console.error(error);
        });
    }

    if(client == null) {
        return "loading...";
    };
    return(
        <div className='flex flex-col lg:flex-row h-screen lg:h-auto'>
            <SidebarFull client={client} />
            <form onSubmit={handleSubmit} className='grid gris-1 h-fit w-full my-100px px-10 gap-10'>
                <div className="pieceTitle">Cliënt Aanmaken</div>
                <div className='md:col-span-2'>Client gegevens</div>
                <div className='flex flex-col gap-2'>
                    <InputFieldText value={identificationNumber} onChange={(e) => setIdentificationNumber(e.target.value)} text="BSN" placeholder="BSN nummer" required={true}/>
                    <div className='flex w-full justify-between'>
                        <InputFieldText value={initials} onChange={(e) => setInitials(e.target.value)} type="small" text="Voorletters" placeholder="b.v. A B"/>
                        <InputFieldText value={prefixLastName} onChange={(e) => setPrefixLastName(e.target.value)} type="small" text="Tussenvoegsel" placeholder="b.v. de"/>
                    </div>
                    <InputFieldText value={streetName} onChange={(e) => setStreetName(e.target.value)} text="Straatnaam" placeholder="Straatnaam"/>
                    <InputFieldText value={postalCode} onChange={(e) => setPostalCode(e.target.value)} text="Postcode" placeholder="b.v. 1234 AA"/>
                    <InputFieldText value={telephoneNumber} onChange={(e) => setTelephoneNumber(e.target.value)} text="Telefoon" placeholder="b.v. 0543-123456"/>
                    <InputFieldText text="Geboortedatum" placeholder="b.v. 01-01-2001"/>
                    <InputFieldText value={maritalStatus} onChange={(e) => setMaritalStatus(e.target.value)} options={optionsMaritalStatus} type="dropdown" text="Burgelijke staat" placeholder="Kies uit de lijst" />

                </div>
                <div className='flex flex-col gap-2'>
                    <InputFieldText value={firstName} onChange={(e) => setFirstName(e.target.value)} text="RoepNaam" placeholder="RoepNaam"/>
                    <InputFieldText value={lastName} onChange={(e) => setLastName(e.target.value)} text="AchterNaam" placeholder="Achternaam"/>
                    <div className='flex w-full justify-between'>
                        <InputFieldText value={houseNumber} onChange={(e) => setHouseNumber(e.target.value)} type="small" text="Huisnummer" placeholder="Huisnummer"/>
                        <InputFieldText value={houseNumberAddition} onChange={(e) => setHouseNumberAddition(e.target.value)} type="small" text="Toevoegingen" placeholder="b.v. a"/>
                    </div>
                    <InputFieldText value={residence} onChange={(e) => setResidence(e.target.value)} text="Woonplaats" placeholder="Woonplaats"/>
                    <InputFieldText value={emailAddress} onChange={(e) => setEmailAddress(e.target.value)}  text="Email" placeholder="b.v.mail@mailbox.com"/>
                    <InputFieldText value={sex} onChange={(e) => setSex(e.target.value)} options={optionsSex} type="dropdown" text="Geslacht" placeholder="Kies uit de lijst"/>
                    <InputFieldText value={driversLicences} onChange={(e) => setDriversLicences({})} options={optionsDriversLicences} type="dropdownPlus" text="Rijbewijs" placeholder="Kies uit de lijst" />

                </div>
                <div className='md:col-span-2'>In geval van nood</div>
                <div>
                    <InputFieldText text="Naam" placeholder="Voor en/of Achternaam"/>
                </div>
                <div>
                    <InputFieldText text="Telefoon" placeholder="b.v. 06-78912345"/>
                </div>
                <div className='md:col-span-2'>Overige informatie</div>
                <div className='flex flex-col gap-2'>
                    <InputFieldText value={diagnoses} onChange={(e) => setDiagnoses(e.target.value)} options={optionsDiagnoses} type="dropdownPlus" text="Diagnose(s)" placeholder="Kies uit de lijst"/> 
                    <InputFieldText value={workingContracts} onChange={(e) => setWorkingContracts(e.target.value)} options={optionsWork} type="dropdownPlus" text="Werkt bij" placeholder="Kies uit de lijst" />
                    <InputFieldText value={workingContracts} onChange={(e) => setWorkingContracts(e.target.value)} options={optionsContract} type="dropdown" text="Contract" placeholder="Kies uit de lijst"></InputFieldText>
                    <InputFieldText text="Van" placeholder="b.v. 01-01-2022"/>
                </div>
                <div className='flex flex-col gap-2'>
                    <InputFieldText options={optionsBenefitForm} type="dropdownPlus" text="Uitkeringsvorm" placeholder="Kies uit de lijst" />
                    <InputFieldText text="Functie" placeholder="b.v. Administratie"/>
                    <InputFieldText text="Tot" placeholder="b.v. 01-01-2022"/>
                </div>
                <InputFieldText value={remarks} onChange={(e) => setRemarks(e.target.value)} text="Opmerking" placeholder="Voeg opmerking toe" type="big" />
                <div className='flex justify-end col-span-2'>
                    <Button text="opslaan" typeOfBtn="submit"/>
                </div>
            </form>
            <Copyright />
        </div>
    )
}

export default ClientsAdd;