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
        fetch("https://localhost:7017/api/Client")
            .then(response => {
                return response.json();
            })
            .then(data => {
                //console.log(data);
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
        const client = {bsnNumber, displayName}
        console.log(client)

        fetch("https://localhost:7017/api/Client", {
            method: 'POST',
            headers: { 
                //'Accept': 'application/json',
                "Content-Type" : "application/json"
            },
            body: JSON.stringify(client)
        }).then(() =>{
            console.log("new blog added")
        }).catch(error => {
            console.error(error);
        });
    }

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
            <form onSubmit={handleSubmit} className='grid  w-full'>
                <div className="pieceTitle">Cliënt Aanmaken</div>
                <div className='col-span-2'>Client gegevens</div>
                <div className="">
                    <InputFieldText value={bsnNumber} onChange={(e) => setBsnNumber(e.target.value)} text="BSN" placeholder="BSN nummer" required={true}/>
                    <div className='flex w-full justify-between'>
                        <InputFieldText type="small" text="Voorletters" placeholder="b.v. A B"/>
                        <InputFieldText type="small" text="Tussenvoegsel" placeholder="b.v. de"/>
                    </div>
                    <InputFieldText text="Adres" placeholder="Adres en huisnummer"/>
                    <InputFieldText text="Woonplaats" placeholder="Woonplaats"/>
                    <InputFieldText text="Telefoon" placeholder="b.v. 0543-123456"/>
                    <InputFieldText text="Geboortedatum" placeholder="b.v. 01-01-2001"/>
                    <InputFieldText type="dropdown" text="Burgelijke staat" placeholder="Kies uit de lijst">
                        <div className='cursor-pointer'>Test1</div>
                        <div>Test2</div>
                    </InputFieldText>

                </div>
                <div className="">
                    <InputFieldText value={displayName} onChange={(e) => setDisplayName(e.target.value)} text="RoepNaam" placeholder="RoepNaam"/>
                    <InputFieldText text="AchterNaam" placeholder="Achternaam"/>
                    <InputFieldText text="Postcode" placeholder="b.v. 1234 AA"/>
                    <InputFieldText text="Mobiel" placeholder="b.v. 06-12345678"/>
                    <InputFieldText text="Email" placeholder="b.v.mail@mailbox.com"/>
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