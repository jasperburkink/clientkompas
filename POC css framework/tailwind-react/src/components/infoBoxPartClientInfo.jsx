import '../index.css';

export function InfoBoxPartClientInfo(props) {
    if(props.client == null) {
        return "loading...";
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
                <li className='md:order-6'>Geboortedatum: {props.geboortedatum}</li>
                <li className='mt-3 md:hidden'>Burgelijke staat: {props.client.maritalStatus}</li>
                <li className='md:hidden'>Rijbewijs: {props.client.driversLicences[0] ? props.client.driversLicences[0].driversLicenceCode : "geen"}</li>
            </ul>
            <ul className="twoSpaceUlBox">
                <li className="md:col-span-2 font-bold pt-3 md:p-0">In geval van nood</li>
                <li>{props.client.emergencyPeople[0].name}</li>
                <li className='hidden md:block'>Burgelijke staat: {props.client.maritalStatus}</li>
                <li>Mobiel: {props.client.emergencyPeople[0].telephoneNumber}</li>
                <li className='hidden md:block'>Rijbewijs: {props.client.driversLicences[0] ? props.client.driversLicences[0].driversLicenceCode : "geen"}</li>
            </ul>
            <ul className="twoSpaceUlBox">
                <li className="md:col-span-2 font-bold pt-3 md:p-0">Overige informatie</li>
                <li className='md:order-1'>Diagnose(s): {props.client.diagnoses[0].name}</li>
                <li className='md:order-3'>Uitkeringsvorm: {props.client.benefitForm}</li>
                <li className='md:order-5'>Werkt bij: {props.client.workingContracts[0].companyName}</li>
                <li className='md:order-2'>Contract: {props.client.workingContracts[0].contractType}</li>
                <li className='md:order-4'>Van: {props.client.workingContracts[0].fromDate}</li>
                <li className='md:order-6'>Tot: {props.client.workingContracts[0].toDate}</li>
                <li className='md:order-7'>Functie: {props.client.workingContracts[0].function}</li>
            </ul>
            <ul className="h-fit shrink-0">
                <li className="md:col-span-2 font-bold pt-3 md:p-0">Opmerkingen</li>
                <li className="md:col-span-2">{props.client.remarks}</li>
            </ul>
        </div>
        
    );
}