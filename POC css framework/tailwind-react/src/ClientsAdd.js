import './index.css';

import { Copyright } from './components/copyright';
import { Button } from './components/button';
import React, { useEffect, useState } from "react";
import { useParams } from 'react-router-dom';
import { InputFieldText } from './components/inputFieldText';
import { SidebarFull } from './components/sidebarFull';

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
    const [houseNumber, setHouseNumber] = useState();
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
    const [companyName, setCompanyName] = useState([])
    const [functionWork, setFunction] = useState('')
    const [contractType, setContractType] = useState('')
    const [fromDate, setFromDate] = useState('')
    const [toDate, setToDate] = useState('')
    const [workingContracts, setWorkingContracts] = useState([]);
    const [remarks, setRemarks] = useState('');
    const [dateOfBirth, setDateOfBirth] = useState('');
    useEffect(() => {
        fetch("https://localhost:7017/api/Client")
            .then(response => {
                return response.json();
            })
            .then(data => {
                //console.log(data);
                setClient(data);
                if(id){
                    var clientData = data.find((element) => {
                        return element.id == id
                    })
                    if(clientData){
                        setIdentificationNumber(clientData.identificationNumber)
                        setFirstName(clientData.firstName)
                        setInitials(clientData.initials)
                        setPrefixLastName(clientData.prefixLastName)
                        setLastName(clientData.lastName)
                        setSex(getNameOfValue(clientData.sex, optionsSex))
                        setStreetName(clientData.streetName)
                        setHouseNumber(clientData.houseNumber)
                        setHouseNumberAddition(clientData.houseNumberAddition)
                        setPostalCode(clientData.postalCode)
                        setResidence(clientData.residence)
                        setTelephoneNumber(clientData.telephoneNumber)
                        setDateOfBirth(clientData.dateOfBirth)
                        setEmailAddress(clientData.emailAddress)
                        setMaritalStatus(getNameOfValue(clientData.maritalStatus, optionsMaritalStatus))
                        if(clientData.driversLicences[0]){
                            setDriversLicences(getAllInfo(clientData, "driversLicences", "driversLicenceCode", optionsDriversLicences))
                        }
                        if(clientData.emergencyPeople[0]){
                            let allInfo = []
                            for (let i = 0; i < clientData.emergencyPeople.length; i++) {
                                allInfo = ([...allInfo, {name: clientData.emergencyPeople[i].name, telephoneNumber: clientData.emergencyPeople[i].telephoneNumber}])
                                setEmergencyPeople(allInfo)
                            }
                        }

                        if(clientData.diagnoses[0]){
                            setDiagnoses(getAllInfo(clientData, "diagnoses", "name"))
                        }
                        setBenefitForm(getNameOfValue(clientData.benefitForm, optionsBenefitForm))
                        if(clientData.workingContracts[0]){
                            // setCompanyName(getAllInfo(data[id], "workingContracts", "companyName"))
                            // setFunction(data[id].workingContracts[0].function)
                            // let contractTypeNumber = data[id].workingContracts[0].contractType - 1
                            // setContractType(getNameOfValue(contractTypeNumber, optionsContract))
                            // setFromDate(data[id].workingContracts[0].fromDate)
                            // setToDate(data[id].workingContracts[0].toDate)
                            setWorkingContracts([])
                        }
                        //setWorkingContracts([{companyName}, {functionWork}, {contractType}, {fromDate}, {toDate}])
                        setRemarks(clientData.remarks)
                    }else{
                        setEmergencyPeople([...emergencyPeople, {name: "", telephoneNumber: ""}])
                    }
                }else{
                    setEmergencyPeople([...emergencyPeople, {name: "", telephoneNumber: ""}])
                }
            })
            .catch(error => {
                console.error(error);
            });
    }, 
    [] 
    ); 
    const getNameOfValue = (client, options) => {
        if(client || client === 0){
            let nameOfValue = ""
            nameOfValue += options[client].name
            return(nameOfValue)
        }else{
            return("/")
        }
    }
    const getValueOfName = (name, options) => {
        if(options.find(function(element){return element.name == name})){
            let valueOfName = ""
            let element = options.find(function(element){
                return element.name == name
            })
            valueOfName += element.value
            return(valueOfName)
        }else{
            return("/")
        }
    }
    const getAllInfo = (client, soort, naam, options) =>{
        if(client[soort].length > 0){
            let allInfo = []
            for (let i = 0; i < client[soort].length; i++) {
                if(options){
                    allInfo = ([...allInfo, {"naam": getNameOfValue(client[soort][i][naam], options), [naam]: client[soort][i][naam]}])
                }else{
                    allInfo = ([...allInfo, {"naam": client[soort][i][naam], [naam]: client[soort][i][naam]}])
                }
            }
            return(allInfo)
        }else{
            return("/")
        }
    }
    const handleSubmit = async (e) => {
        e.preventDefault();
        const headers = {
            'Accept': 'application/json',
            'content-type':'application/json',
            "Content-Type" : "application/json",
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Credentials": "true",
            "Access-Control-Allow-Methods": "GET,HEAD,OPTIONS,POST,PUT",
            "Access-Control-Allow-Headers": "Access-Control-Allow-Headers, Origin,Accept, X-Requested-With, Content-Type, Access-Control-Request-Method, Access-Control-Request-Headers"
        }
        if(id){
            const newClient = {id, identificationNumber, firstName, initials, prefixLastName, lastName, streetName, houseNumber, houseNumberAddition, postalCode, residence, telephoneNumber, dateOfBirth, emailAddress, driversLicences, emergencyPeople, diagnoses, remarks}
            console.log(newClient)
            fetch("https://localhost:7017/api/Client/" + id, {
                method: 'PUT',
                mode: 'cors',
                headers: headers,
                body: JSON.stringify(newClient)
            }).then((response) =>{
                console.log("client updated")
            }).catch(error => {
                console.error(error);
            });
        }else{
            const newClient = {identificationNumber, firstName, initials, prefixLastName, lastName, streetName, houseNumber, houseNumberAddition, postalCode, residence, telephoneNumber, dateOfBirth, emailAddress, driversLicences, emergencyPeople, diagnoses, remarks}
            console.log(newClient)
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
    }

    if(client == null) {
        return "loading...";
    };
    return(
        <div className='flex flex-col lg:flex-row h-screen lg:h-auto'>
            <SidebarFull client={client} />
            <form onSubmit={handleSubmit} className='grid grid-2 h-fit w-full my-100px px-10 gap-10'>
                {id ? <div className="pieceTitle">Cliënt Aanpassen</div> : <div className="pieceTitle">Cliënt Aanmaken</div>}
                <div className='md:col-span-2 font-bold'>Client gegevens</div>
                <div className='flex flex-col gap-2'>
                    <InputFieldText value={identificationNumber} onChange={(e) => setIdentificationNumber(e.target.value)} text="BSN" placeholder="BSN nummer" required={true}/>
                    <div className='flex w-full justify-between'>
                        <InputFieldText value={initials} onChange={(e) => setInitials(e.target.value)} type="small" text="Voorletters" placeholder="b.v. A B"/>
                        <InputFieldText value={prefixLastName} onChange={(e) => setPrefixLastName(e.target.value)} type="small" text="Tussenvoegsel" placeholder="b.v. de"/>
                    </div>
                    <InputFieldText value={streetName} onChange={(e) => setStreetName(e.target.value)} text="Straatnaam" placeholder="Straatnaam"/>
                    <InputFieldText value={postalCode} onChange={(e) => setPostalCode(e.target.value)} text="Postcode" placeholder="b.v. 1234 AA"/>
                    <InputFieldText value={telephoneNumber} onChange={(e) => setTelephoneNumber(e.target.value)} text="Telefoon" placeholder="b.v. 0543-123456"/>
                    <InputFieldText value={dateOfBirth} onChange={(e) => setDateOfBirth(e.target.value)} text="Geboortedatum" placeholder="b.v. 01-01-2001"/>
                    <InputFieldText value={maritalStatus} onChange={(e) => setMaritalStatus(e.target.value)} stateChanger={setMaritalStatus} options={optionsMaritalStatus} type="dropdown" text="Burgelijke staat" placeholder={maritalStatus ? maritalStatus : "Kies uit de lijst"}/>

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
                    <InputFieldText value={sex} onChange={(e) => setSex(e.target.value)} stateChanger={setSex} options={optionsSex} type="dropdown" text="Geslacht" placeholder={sex ? sex : "Kies uit de lijst"}/>
                    <InputFieldText info={driversLicences} value={driversLicences} state={driversLicences} stateChanger={setDriversLicences} stateName="driversLicenceCode" options={optionsDriversLicences} type="dropdownPlus" text="Rijbewijs" placeholder="Kies uit de lijst" />

                </div>
                <div className='md:col-span-2 font-bold'>In geval van nood</div>
                <div className='grid grid-cols-1 md:col-span-2 gap-2'>
                    {emergencyPeople.map((singleEmergencyPeople, index) => (
                        <div id={index} key={index}>
                            {singleEmergencyPeople.name ? <div className='grid grid-cols-2 md:col-span-2 gap-2'>
                                <InputFieldText value={emergencyPeople[index].name} onChange={(e) => setEmergencyPeople((prevState) => {
                                    let newEmergencyPeople = [...prevState]
                                    newEmergencyPeople[index].name = e.target.value
                                    return newEmergencyPeople
                                })} text="Naam" placeholder="Voor en/of Achternaam"/> 
                                <InputFieldText value={emergencyPeople[index].telephoneNumber} onChange={(e) => setEmergencyPeople((prevState) => {
                                    let newEmergencyPeople = [...prevState]
                                    newEmergencyPeople[index].telephoneNumber = e.target.value
                                    return newEmergencyPeople
                                })} text="Telefoon" placeholder="b.v. 06-78912345"/>
                            </div> 
                            : 
                            <div className='grid grid-cols-2 md:col-span-2 gap-2'>
                                <InputFieldText value={emergencyPeople[index].name} onChange={(e) => setEmergencyPeople((prevState) => {
                                    let newEmergencyPeople = [...prevState]
                                    newEmergencyPeople[index].name = e.target.value
                                    return newEmergencyPeople
                                })} text="Naam" placeholder="Voor en/of Achternaam"/> 
                                <InputFieldText onChange={(e) => setEmergencyPeople((prevState) => {
                                    let newEmergencyPeople = [...prevState]
                                    newEmergencyPeople[index].telephoneNumber = e.target.value
                                    return newEmergencyPeople
                                })} text="Telefoon" placeholder="b.v. 06-78912345"/>
                            </div> 
                        }
                        </div>
                        
                    ))}
                    <div className='text-blue-400 cursor-pointer' onClick={() => setEmergencyPeople([...emergencyPeople, {name: "", telephoneNumber: ""}])}>Voeg nog een persoon toe</div>
                </div>
                <div className='md:col-span-2 font-bold'>Overige informatie</div>
                <div className='grid grid-cols-2 gap-2 col-span-2'>
                    <InputFieldText info={diagnoses} value={diagnoses} state={diagnoses} stateChanger={setDiagnoses} stateName="name" options={optionsDiagnoses} type="dropdownPlus" text="Diagnose(s)" placeholder="Kies uit de lijst"/> 
                    <InputFieldText value={benefitForm} onChange={(e) => setBenefitForm(e.target.value)} stateChanger={setBenefitForm} options={optionsBenefitForm} type="dropdown" text="Uitkeringsvorm" placeholder={benefitForm ? benefitForm : "Kies uit de lijst"} />
                </div>
                <div className='grid grid-cols-2 gap-2 col-span-2'>
                    <InputFieldText info={companyName} value={companyName} state={companyName} stateChanger={setCompanyName} stateName="companyName" options={optionsWork} type="dropdownPlus" text="Werkt bij" placeholder="Kies uit de lijst" />
                    <div></div>
                    <InputFieldText value={contractType} state={contractType} stateChanger={setContractType} onChange={(e) => setContractType(e.target.value)} options={optionsContract} type="dropdown" text="Contract" placeholder={contractType ? contractType : "Kies uit de lijst"}></InputFieldText>
                    <InputFieldText value={functionWork} onChange={(e) => setFunction(e.target.value)} text="Functie" placeholder="b.v. Administratie"/>
                    <InputFieldText value={fromDate} onChange={(e) => setFromDate(e.target.value)} text="Van" placeholder="b.v. 01-01-2022"/>
                    <InputFieldText value={toDate} onChange={(e) => setToDate(e.target.value)} text="Tot" placeholder="b.v. 01-01-2022"/>
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