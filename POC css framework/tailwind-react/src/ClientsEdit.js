import './index.css';

import { Copyright } from './components/copyright';
import { Sidebar } from './components/sidebar';
import { SidebarGray } from './components/sidebarGray';
import { NavButton } from './components/navButton';
import { NavButtonGray } from './components/navButtonGray';
import { SearchInputField } from './components/searchInputField';
import { NavTitle } from './components/navTitle';
import { InputField } from './components/inputField';
import { Button } from './components/button';
import { ProfilePicture } from './components/profilePicture';
import { InfoBox } from './components/infoBox';
import { InfoBoxPartClientInfo } from './components/infoBoxPartClientInfo';
import { InfoBoxPartTrajectInfo } from './components/infoBoxPartTrajectInfo';
import React, { useEffect, useState } from "react";
import { useParams } from 'react-router-dom';
import { InputFieldText } from './components/inputFieldText';

function ClientsAdd() {var [ client, setClient ] = useState(null);
    const { id } = useParams();

    const [bsnNumber, setBsnNumber] = useState('');
    const [displayName, setDisplayName] = useState('');
    const [initials, setInitials] = useState('');
    const [infix, setInfix] = useState('');
    const [lastName, setLastName] = useState('');
    const [streetName, setStreetName] = useState('');
    const [houseNumber, setHouseNumber] = useState('');
    const [houseNumberAddition, setHouseNumberAddition] = useState('');
    const [postalCode, setPostalCode] = useState('');
    const [residence, setResidence] = useState('');
    const [telephoneNumber, setTelephoneNumber] = useState('');
    const [mobileNumber, setMobileNumber] = useState('');
    const [emailAddress, setEmailAddress] = useState('');

    useEffect(() => {
        fetch("https://localhost:7017/api/Client", {mode: 'cors'})
            .then(response => {
                return response.json();
            })
            .then(data => {
                console.log(data);
                setClient(data);
                setBsnNumber(data[id].bsnNumber)
                setDisplayName(data[id].displayName)
                setInitials(data[id].initials)
                setInfix(data[id].infix)
                setLastName(data[id].lastName)
                setStreetName(data[id].streetName)
                setHouseNumber(data[id].houseNumber)
                setHouseNumberAddition(data[id].houseNumberAddition)
                setPostalCode(data[id].postalCode)
                setResidence(data[id].residence)
                setTelephoneNumber(data[id].telephoneNumber)
                setEmailAddress(data[id].emailAddress)
            })
            .catch(error => {
                console.error(error);
            });
    }, 
    [] // Voert nu maar 1 keer uit als het component gerenderd wordt
    ); 
    /*const handleSubmit = (e) => {
        e.preventDefault();
        const newClient = {bsnNumber, displayName}
        console.log(newClient)

        fetch("https://localhost:7017/api/Client", {mode: 'cors'}, {
            method: 'POST',
            headers: { 
                //'Accept': 'application/json',
                "content-Type" : "application/json"
            },
            body: JSON.stringify(newClient)
        }).then(() =>{
            console.log("new client added")
        }).catch(error => {
            console.error(error);
        });
    }*/

    if(client == null) {
        return "loading...";
    };
    return(
        <div className='flex flex-col lg:flex-row h-screen lg:h-auto'>
            <div className="lg:flex">
                <Sidebar>
                    <NavButton text="Cliënten" icon="Gebruikers"/>
                    <NavButton text="Uren registratie" icon="Klok"/>
                    <NavButton text="Organistatie" icon="Gebouw"/>
                    <NavButton text="Gebruiker" icon="Gebruiker"/>
                    <NavButton text="Uitloggen" icon="Uitloggen"/>
                </Sidebar>
                <SidebarGray>
                    <NavTitle lijstNaam="Cliëntenlijst" path="/ClientsAdd"/>
                    <SearchInputField />
                    <div className="h-fit">
                        {client.map((infoClient, pathNumber) => {
                            return (
                                <NavButtonGray key={pathNumber} path={"/Clients/" + pathNumber} text={infoClient.displayName+ " " + infoClient.infix + " " + infoClient.lastName} />
                            )
                        })}
                    </div>
                </SidebarGray>
            </div>
            <form className='grid grid-cols-halfhalf w-full'>
                <div className="pieceTitle">Cliënt Aanmaken</div>
                <div className='col-span-2'>Client gegevens</div>
                <div className="">
                    <InputFieldText value={bsnNumber} onChange={(e) => setBsnNumber(e.target.value)} text="BSN" placeholder="BSN nummer" required={true}/>
                    <div className='flex w-full justify-between'>
                        <InputFieldText value={initials} onChange={(e) => setInitials(e.target.value)} type="small" text="Voorletters" placeholder="b.v. A B"/>
                        <InputFieldText value={infix} onChange={(e) => setInfix(e.target.value)} type="small" text="Tussenvoegsel" placeholder="b.v. de"/>
                    </div>
                    <InputFieldText value={streetName} onChange={(e) => setStreetName(e.target.value)} text="Adres" placeholder="Adres en huisnummer"/>
                    <InputFieldText value={residence} onChange={(e) => setResidence(e.target.value)} text="Woonplaats" placeholder="Woonplaats"/>
                    <InputFieldText value={telephoneNumber} onChange={(e) => setTelephoneNumber(e.target.value)} text="Telefoon" placeholder="b.v. 0543-123456"/>
                    <InputFieldText text="Geboortedatum" placeholder="b.v. 01-01-2001"/>
                    <InputFieldText type="dropdown" text="Burgelijke staat" placeholder="Kies uit de lijst">
                        <div className='cursor-pointer'>Test1</div>
                        <div>Test2</div>
                    </InputFieldText>

                </div>
                <div className="">
                    <InputFieldText value={displayName} onChange={(e) => setDisplayName(e.target.value)} text="RoepNaam" placeholder="RoepNaam"/>
                    <InputFieldText value={lastName} onChange={(e) => setLastName(e.target.value)} text="AchterNaam" placeholder="Achternaam"/>
                    <InputFieldText value={postalCode} onChange={(e) => setPostalCode(e.target.value)} text="Postcode" placeholder="b.v. 1234 AA"/>
                    <InputFieldText value={mobileNumber} onChange={(e) => setMobileNumber(e.target.value)} text="Mobiel" placeholder="b.v. 06-12345678"/>
                    <InputFieldText value={emailAddress} onChange={(e) => setEmailAddress(e.target.value)} text="Email" placeholder="b.v.mail@mailbox.com"/>
                    <InputFieldText type="dropdown" text="Rijbewijs" placeholder="Kies uit de lijst">
                        <div>Test1</div>
                        <div>Test2</div>
                    </InputFieldText>

                </div>
                <div className='col-span-2'>In geval van nood</div>
                <div className="">
                    <InputFieldText text="Naam" placeholder="Voor en/of Achternaam"/>
                </div>
                <div className="">
                    <InputFieldText text="Telefoon" placeholder="b.v. 06-78912345"/>
                </div>
                <div className='col-span-2'>Overige informatie</div>
                <div className="">
                    <InputFieldText type="dropdownPlus" text="Diagnose(s)" placeholder="Kies uit de lijst">
                        <div>Test1</div>
                        <div>Test2</div>
                    </InputFieldText>
                    <InputFieldText type="dropdownPlus" text="Werkt bij" placeholder="Kies uit de lijst">
                        <div>Test1</div>
                        <div>Test2</div>
                    </InputFieldText>
                    <InputFieldText type="dropdown" text="Contract" placeholder="Kies uit de lijst">
                        <div>Test1</div>
                        <div>Test2</div>
                    </InputFieldText>
                    <InputFieldText text="Van" placeholder="b.v. 01-01-2022"/>
                </div>
                <div className="">
                    <InputFieldText type="dropdownPlus" text="Uitkeringsvorm" placeholder="Kies uit de lijst">
                        <div>Test1</div>
                        <div>Test2</div>
                    </InputFieldText>
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