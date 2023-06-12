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

    const [identificationNumber, setIdentificationNumber] = useState('');
    const [firstName, setFirstName] = useState('');
    const [initials, setInitials] = useState('');
    const [prefixLastName, setPrefixLastName] = useState('');
    const [lastName, setLastName] = useState('');
    const [sex, setSex] = useState('');
    const [streetName, setStreetName] = useState('');
    const [houseNumber, setHouseNumber] = useState('');
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
            })
            .catch(error => {
                console.error(error);
            });
    }, 
    [] // Voert nu maar 1 keer uit als het component gerenderd wordt
    ); 
    const handleSubmit = (e) => {
        e.preventDefault();
        const newClient = {identificationNumber, firstName, initials, prefixLastName, lastName, streetName, houseNumber, houseNumberAddition, postalCode, residence, telephoneNumber, emailAddress}
        console.log(newClient)
        
//, {mode: 'cors'}
const headers = {
    'Accept': 'application/json',
    'Accept' : '*/*',
    'accept':'application/json',
    'content-type':'application/json',
    'Access-Control-Allow-Origin':'http://localhost:7017',
    'Access-Control-Allow-Origin':'http://localhost:3000',
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
            body: JSON.stringify({
                identificationNumber: 7,
                firstName: "aaa",
                emailAddress: "",
                houseNumber: 0,
                houseNumberAddition: "",
                prefixLastName: "",
                initials: "",
                lastName: "",
                postalCode: "",
                residence: "",
                streetName: "",
                telephoneNumber: ""
            })
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
            <form onSubmit={handleSubmit} className='grid gris-1 md:grid-cols-halfhalf h-fit w-full mt-100px'>
                <div className="pieceTitle">Cliënt Aanmaken</div>
                <div className='md:col-span-2'>Client gegevens</div>
                <div className="">
                    <InputFieldText value={identificationNumber} onChange={(e) => setIdentificationNumber(e.target.value)} text="BSN" placeholder="BSN nummer" required={true}/>
                    <div className='flex w-full justify-between'>
                        <InputFieldText type="small" text="Voorletters" placeholder="b.v. A B"/>
                        <InputFieldText type="small" text="Tussenvoegsel" placeholder="b.v. de"/>
                    </div>
                    <InputFieldText text="Straatnaam" placeholder="Straatnaam"/>
                    <InputFieldText text="Postcode" placeholder="b.v. 1234 AA"/>
                    <InputFieldText text="Telefoon" placeholder="b.v. 0543-123456"/>
                    <InputFieldText text="Geboortedatum" placeholder="b.v. 01-01-2001"/>
                    <InputFieldText type="dropdown" text="Burgelijke staat" placeholder="Kies uit de lijst" />

                </div>
                <div className="">
                    <InputFieldText value={firstName} onChange={(e) => setFirstName(e.target.value)} text="RoepNaam" placeholder="RoepNaam"/>
                    <InputFieldText text="AchterNaam" placeholder="Achternaam"/>
                    <div className='flex w-full justify-between'>
                        <InputFieldText type="small" text="Huisnummer" placeholder="Huisnummer"/>
                        <InputFieldText type="small" text="Toevoegingen" placeholder="b.v. a"/>
                    </div>
                    <InputFieldText text="Woonplaats" placeholder="Woonplaats"/>
                    <InputFieldText text="Email" placeholder="b.v.mail@mailbox.com"/>
                    <InputFieldText type="dropdown" text="Geslacht" placeholder="Kies uit de lijst"/>
                    <InputFieldText type="dropdown" text="Rijbewijs" placeholder="Kies uit de lijst" />

                </div>
                <div className='md:col-span-2'>In geval van nood</div>
                <div className="">
                    <InputFieldText text="Naam" placeholder="Voor en/of Achternaam"/>
                </div>
                <div className="">
                    <InputFieldText text="Telefoon" placeholder="b.v. 06-78912345"/>
                </div>
                <div className='md:col-span-2'>Overige informatie</div>
                <div className="">
                    <InputFieldText type="dropdownPlus" text="Diagnose(s)" placeholder="Kies uit de lijst"> 

                    </InputFieldText>
                    <InputFieldText type="dropdownPlus" text="Werkt bij" placeholder="Kies uit de lijst" />
                    <InputFieldText dropdownInfo="Test2" type="dropdown" text="Contract" placeholder="Kies uit de lijst"></InputFieldText>
                    <InputFieldText text="Van" placeholder="b.v. 01-01-2022"/>
                </div>
                <div className="">
                    <InputFieldText type="dropdownPlus" text="Uitkeringsvorm" placeholder="Kies uit de lijst" />
                    <InputFieldText text="Functie" placeholder="b.v. Administratie"/>
                    <InputFieldText text="Tot" placeholder="b.v. 01-01-2022"/>
                </div>
                <div>
                    <Button text="opslaan" typeOfBtn="submit"/>
                </div>
            </form>
            <Copyright />
        </div>
    )
}

export default ClientsAdd;