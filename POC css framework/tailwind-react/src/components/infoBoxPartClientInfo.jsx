import '../index.css';

export function InfoBoxPartClientInfo(props) {
    return (
        <div className="p-3 md:overflow-hidden w-screen md:w-full h-fit md:h-full gap-3 flex flex-col justify-between">
            <ul className={`twoSpaceUlBox md:h-150px`}>           
                <li className="pieceTitle">Cliënt info</li>
                <li className='md:order-1'>{props.naam}</li>
                <li className='md:order-3'>{props.straat}</li>
                <li className='md:order-5'>{props.adres}</li>
                <li className='md:order-7'></li>
                <li className='md:order-8 my-3 md:m-0'>BSN: {props.bsn}</li>
                <li className='md:order-2'>Mobiel: {props.mobiel}</li>
                <li className='md:order-4'>Email: {props.email}</li>
                <li className='md:order-6'>Geboortedatum: {props.geboortedatum}</li>
                <li className='md:order-6'>Mobiel: {props.contactMobiel}</li>
                <li className='md:order-6'>Rijbewijs: {props.contactRijbewijs}</li>
            </ul>
            <ul className="twoSpaceUlBox md:h-100px">
                <li className="md:col-span-2 font-bold">In geval van nood</li>
                <li>{props.contactNaam}</li>
                <li>Burgelijke staat: {props.contactStaat}</li>
                <li>Mobiel: {props.contactMobiel}</li>
                <li>Rijbewijs: {props.contactRijbewijs}</li>
            </ul>
            <ul className="twoSpaceUlBox md:h-150px">
                <li className="md:col-span-2 font-bold">Overige informatie</li>
                <li>Diagnose(s): {props.diagnose}</li>
                <li>Contract: {props.contract}</li>
                <li>Uitkeringsvorm: {props.uitkeringsvorm}</li>
                <li>Van: {props.van}</li>
                <li>Werkt bij: {props.werk}</li>
                <li>Tot: {props.tot}</li>
                <li>Functie: {props.functie}</li>
            </ul>
            <ul className="h-fit shrink-0">
                <li className="md:col-span-2 font-bold">Opmerkingen</li>
                <li className="md:col-span-2">{props.opmerking}</li>
            </ul>
        </div>
        
    );
}