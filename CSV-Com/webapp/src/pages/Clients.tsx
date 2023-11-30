import '../styles/pages/Client.css';

import React, { useEffect, useState } from "react";
import { useParams } from 'react-router-dom';
import { Header } from '../components/common/header';
import { Label } from '../components/common/label';
import { LinkButton } from '../components/common/link-button';
import { Sidebar } from '../components/sidebar/sidebar';
import { NavButton } from '../components/nav/nav-button';
import { SidebarGray } from '../components/sidebar/sidebar-gray';
import { ProfilePicture } from '../components/common/profile-picture';
import { Copyright } from '../components/common/copyright';
import { NavButtonGray } from '../components/nav/nav-button-gray';
import { NavTitle } from '../components/nav/nav-title';
import { SlideToggleLabel } from '../components/common/slide-toggle-label';
import Client from '../types/model/Client';
import '../utils/Utilities';
import Moment from 'moment';
import { fetchClient } from '../utils/api';

let noInfo = 'Geen informatie beschikbaar';
let dateFormat = 'DD-MM-yyyy';

function Clients() {
    const profilePicRowSpanValueDefault = 4;

    const [client, setClient] = useState<Client | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [profilePicSpanValue, setProfilePicSpanValue] = useState(profilePicRowSpanValueDefault);
    
    var { id } = useParams();

    // Show dates in local format
    Moment.locale('nl');

    // Get client by id
    useEffect(() => {
        const getClient = async () => {
          try {
            const fetchedClient: Client = await fetchClient(id!);
            setClient(fetchedClient);

            var rowSpanProfilePic = fetchedClient.emergencypeople != null ? fetchedClient.emergencypeople.length + profilePicRowSpanValueDefault : profilePicRowSpanValueDefault;
            setProfilePicSpanValue(rowSpanProfilePic);
          } catch (e) {
            // TODO: error handling
            console.error(e);
            //setError(e);
          }
        };
    
        getClient();
      }, [id]);    

    if (!client) {
        // TODO:  show loading status correctly
        return <div>Loading...</div>;
      }    

    return (
        <div className="flex flex-col lg:flex-row h-screen lg:h-auto">
            <div className='lg:flex w-full'>
                {/* TODO: Move menu to own control. Is already a task on the backlog. */}
                <div className='header-menu'>
                    <Sidebar>
                        <NavButton text="Cliënten" icon="Gebruikers" />
                        <NavButton text="Uren registratie" icon="Klok" />
                        <NavButton text="Organistatie" icon="Gebouw" />
                        <NavButton text="Gebruiker" icon="Gebruiker" />
                        <NavButton text="Uitloggen" icon="Uitloggen" />
                    </Sidebar>
                    <SidebarGray>
                        <NavTitle lijstNaam="Naam Lijst" />
                        {/* <SearchInputField /> */}
                        <div className="h-fit">
                            <NavButtonGray text="Onderdeel lijst" />
                            <NavButtonGray text="Placeholder" />
                            <NavButtonGray text="HolderPlace" />
                            <NavButtonGray text="Placie Holderie" />
                        </div>
                    </SidebarGray>    
                </div>
                <div className="clients">
                {/* {Client main info 1 */}           
                    <Header text="Cliënt info" />

                    {/* Profile picture */}
                    <div className={`client-profile-picture`} style={{ 
                        gridRowStart: "span " + profilePicSpanValue,
                        gridRowEnd: "span "+profilePicSpanValue}}>                        
                        <ProfilePicture />
                    </div>

                    <div className='client-info-main'>                        
                        <Label text={client.firstname + ' ' + (!client.prefixlastname ? '' : client.prefixlastname + ' ')  + client.lastname} />
                        <Label text={client.streetname + ' ' + client.housenumber + (!client.housenumberaddition ? '' : client.housenumberaddition)} />
                        <Label text={client.postalcode + ' ' + client.residence} />                
                    </div>

                    {/* Client number */}                    
                    <div className='client-number client-label-value'>
                        <Label text='Cliëntnummer: ' />
                        <Label text={client.identificationnumber.toString()} />
                    </div>
                    
                    {/* Client main info 2 */}
                    <div className='client-info-main-2'>                        
                        <div className='client-label-value'>
                            <Label text='Telefoon: ' />
                            <Label text={client.telephonenumber} />
                        </div>

                        <div className='client-label-value'>
                            <Label text='E-mail: ' />
                            <Label text={client.emailaddress} />
                        </div>

                        <div className='client-label-value'>
                            <Label text='Geboortedatum: ' />
                            <Label text={Moment(client.dateofbirth).format(dateFormat)} />
                        </div>                        
                    </div>

                    <Label text='In geval van nood' className='client-subheader' strong={true} />
                    {client.emergencypeople &&
                        client.emergencypeople!.map((emergencyPerson, i) =>
                            <div key={i} className='client-emergencyperson'>
                                <Label text={emergencyPerson.name} className='col-span-2' />
                                <div className='client-label-value col-span-2'>
                                    <Label text='Telefoon nr.: ' />
                                    <Label text={emergencyPerson.telephonenumber} />
                                </div>
                            </div>
                        )                         
                    }                    

                    <SlideToggleLabel className='client-additional-info' textColapsed='Meer informatie' textExpanded='Minder informatie'>                       
                        <Label text='Overige informatie' className='client-subheader' strong={true} />
                        
                        <div className='client-additional-info-container'>
                            <div className='client-label-value '>
                                <Label text='Diagnose(s): ' />
                                <Label text={client.diagnoses ? client.diagnoses : noInfo} />
                            </div>

                            <div className='client-label-value'>
                                <Label text='Burgelijke staat:' />
                                <Label text={client.maritalstatus} />
                            </div>
                            
                            <div className='client-label-value'>
                                <Label text='Rijbewijs: ' />            
                                <Label text={client.driverslicences} />
                            </div>

                            <div className='client-label-value client-socialbenefit-form'>
                                <Label text='Uitkeringsvorm: ' />
                                <Label text={client.benefitform ? client.benefitform : noInfo} />
                            </div>

                            {!client.workingcontracts &&
                            <Label text={noInfo} className='col-span-2' />}
                            {client.workingcontracts &&
                                client.workingcontracts!.map((workingContract, i) =>
                                    <div key={i} className='col-span-2 lg:grid lg:grid-cols-2 mt-5'>
                                        <div className='client-label-value'>
                                            <Label text={Moment(workingContract.todate) > Moment() ? 'Werkt bij: ' : 'Werkt(e) bij: ' } />
                                            <Label text={workingContract.companyname} />
                                        </div>
                                        <div className='client-label-value'>
                                            <Label text='Functie: ' />
                                            <Label text={workingContract.function} />
                                        </div>
                                        <div className='client-label-value'>
                                            <Label text='Contract: ' />
                                            <Label text={workingContract.contracttype} />
                                        </div>
                                        <div className='client-label-value'>
                                            <Label text='Van: ' />
                                            <Label text={Moment(workingContract.fromdate).format(dateFormat)} />
                                        </div>
                                        <div className='client-label-value'>
                                            <Label text='Tot: ' />
                                            <Label text={Moment(workingContract.todate).format(dateFormat)} />
                                        </div>
                                </div>)
                            }
                        </div>

                        <div className='client-remarks'>
                            <Label text='Opmerkingen' className='client-subheader' strong={true} />
                            <Label text={client.remarks ? client.remarks : noInfo} className='col-span-2' />
                        </div>

                    </SlideToggleLabel>
                    
                    <div className='button-container'>
                        <LinkButton buttonType={{type:"Solid"}} text="Cliënt aanpassen" href="../pages/client-edit" />
                        <LinkButton buttonType={{type:"NotSolid"}} text="Urenoverzicht" href="../pages/client-hours" />
                    </div>
                </div>
            </div>
            <Copyright />
        </div>
    )
}

export default Clients;