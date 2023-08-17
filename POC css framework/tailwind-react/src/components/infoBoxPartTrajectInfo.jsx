import '../index.css';

import { InputField } from './inputField';

export function InfoBoxPartTrajectInfo(props) {
    return (
        //<div className="p-3 md:p-0 md:overflow-hidden w-screen md:w-full h-fit md:h-full gap-3 flex flex-col justify-between">
            
        <div className='p-3 md:p-0 md:overflow-hidden w-screen md:w-full h-screen md:h-fit'>
            <ul className="twoSpaceUlBox">
                <li className="pieceTitle">Traject info</li>
                <InputField type="dropdown"> 
                    <ul>
                        <li>Test1</li>
                        <li>Test2</li>
                    </ul>
                </InputField>
                <li className='md:order-4'>Ordernummer: {props.ordernummer}</li>
                <li className='md:order-2'>Traject type: {props.trajectType}</li>
                <li className='md:order-4'>Opdrachtgever: {props.opdrachtgever}</li>
                <li className='md:order-1'>Begindatum: {props.begindatum}</li>
                <li className='md:order-3'>Einddatum: {props.einddatum}</li>
                <li className='md:order-4'>Budget bedrag: {props.budgetBedrag}</li>
                <li className='md:order-4'>Uurtarief: {props.uurtarief}</li>
                <li className='md:order-4'>Te besteden uren: {props.teBestedenUren}</li>
                <li className='md:order-4'>Coach werkt voor: {props.coachWerktVoor}</li>
            </ul>
        </div>
    );
}