import '../styles/pages/Client.css';

import React, { useContext, useEffect, useState } from "react";
import { useNavigate, useParams } from 'react-router-dom';
import { Header } from 'components/common/header';
import { Label } from 'components/common/label';
import { LinkButton } from 'components/common/link-button';
import { Button } from 'components/common/button';
import Menu from 'components/common/menu';
import { ProfilePicture } from 'components/common/profile-picture';
import { Copyright } from 'components/common/copyright';
import { NavTitle } from 'components/nav/nav-title';
import { SlideToggleLabel } from 'components/common/slide-toggle-label';
import ClientQuery from 'types/model/ClientQuery';
import 'utils/utilities';
import Moment from 'moment';
import { fetchClient, deactivateClient, fetchCoachingProgramsByClient, fetchCoachingProgram } from 'utils/api';
import SearchClients from 'components/clients/search-clients';
import StatusEnum from 'types/common/StatusEnum';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import { DatePicker } from 'components/common/datepicker';
import { ClientContext, IClientContext } from './client-context';
import ConfirmPopup from "components/common/confirm-popup";
import Client from '../types/model/Client';
import { Dropdown } from 'components/common/dropdown';
import CoachingProgramQuery from 'types/model/CoachingProgramQuery';
import CoachingProgram from 'types/model/CoachingProgram';
import LabelField from 'components/common/label-field';
import Decimal from 'decimal.js-light';
import { formatCurrency } from 'utils/utilities';

const NO_INFO = 'Geen informatie beschikbaar';
const DATE_FORMAT = 'DD-MM-yyyy';

function Clients() {
    const PROFILE_PIC_ROW_SPAN_DEFAULT_VALUE = 4;
    const PATH_CLIENTS = '/Clients/';
    const clientContext = useContext(ClientContext)
    const [client, setClient] = useState<ClientQuery | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [profilePicSpanValue, setProfilePicSpanValue] = useState(PROFILE_PIC_ROW_SPAN_DEFAULT_VALUE);
    const [status, setStatus] = useState(StatusEnum.IDLE);
    const [isDeactivateConfirmPopupOpen, setDeactivateConfirmPopupOpen] = useState<boolean>(false);
    const [isDeactivateConfirmedPopupOpen, setDeactivateConfirmedPopupOpen] = useState<boolean>(false);
    const [deactivatedClient, setDeactivatedClient] = useState<ClientQuery | null>(null);
    const [coachingPrograms, setCoachingPrograms] = useState<CoachingProgramQuery[]>([]);
    const [currentCoachingProgram, setCurrentCoachingProgram] = useState<CoachingProgram | null>(null);
    
    var { id } = useParams();

    // Show dates in local format
    Moment.locale('nl');

    const nav = useNavigate();        

    const fetchClientById = async () => {
        try {
          setStatus(StatusEnum.PENDING);
          const fetchedClient: ClientQuery = await fetchClient(id!);
          setStatus(StatusEnum.SUCCESSFUL);
          setClient(fetchedClient);                  
          
          var rowSpanProfilePic = fetchedClient && fetchedClient.emergencypeople ? fetchedClient.emergencypeople.length + PROFILE_PIC_ROW_SPAN_DEFAULT_VALUE : PROFILE_PIC_ROW_SPAN_DEFAULT_VALUE;
          setProfilePicSpanValue(rowSpanProfilePic);
        } catch (e) {
          // TODO: error handling
          console.error(e);
          setStatus(StatusEnum.REJECTED);
          //setError(e);
        }
      };    

    function deactivateClientClick(client: ClientQuery) {
        try{
            if (!client) {
                throw new Error('Client not found!');
            }
    
            setDeactivateConfirmPopupOpen(true);
        }
        catch(error){
            if (error instanceof Error) {
            // TODO: Show errormessage in popup window
                console.error('An error has occured while confirming to deactivating an client:', error.message);
            } else {
                console.error('An unexpected error has occured.');
            }
        }
    }

    async function deactivateClientConfirmed(client: ClientQuery, context: IClientContext): Promise<ClientQuery | null> {
        try{
            const deactivatedClient: ClientQuery = await deactivateClient(client.id);
            context.setAllClients(context.allClients.filter(c => c.id !== deactivatedClient.id));

            return deactivatedClient;
        }
        catch(error){
            if (error instanceof Error) {
            // TODO: Show errormessage in popup window
                console.error('An error has occured while deactivating an client:', error.message);
            } else {
                console.error('An unexpected error has occured.');
            }
        }

        return null;
    }
    
    function cancelDeactivateClient(){
        setDeactivateConfirmPopupOpen(false);
    }

    function closeConfirmedDeactivateClientPopUp(){
        setDeactivateConfirmedPopupOpen(false);
    }

    const handleCoachingProgramChange = async (value: number) => {
        try {
            if(value === 0) {
                setCurrentCoachingProgram(null);
                return;
            }

            const fetchedCoachingProgram: CoachingProgram = await fetchCoachingProgram(value);
            setCurrentCoachingProgram(fetchedCoachingProgram);                              
          } catch (e) {
            // TODO: error handling
            console.error(e);
            //setError(e);
          }
    };

    // Get client by id
    useEffect(() => {
        if(!id) {
            setStatus(StatusEnum.REJECTED);
            return;
        }

        const fetchData = async () => {
            try {
                await fetchClientById();
    
                const programs = await fetchCoachingProgramsByClient(id!);
                setCoachingPrograms(programs);
    
            } catch (error) {
                console.error('Error fetching data:', error);
                setStatus(StatusEnum.REJECTED);
            }
        };

        fetchData();        

    }, [id]);

    useEffect(() => {
        if(coachingPrograms.length <= 0)
            return;

        handleCoachingProgramChange(coachingPrograms[0].id);
    }, [coachingPrograms]);

    
      
    return (
        <div className="flex flex-col lg:flex-row h-screen lg:h-auto">
            <div className='lg:flex w-full'>
            <div id='staticSidebar' className='sidebarContentPush'></div>

                <Menu>
                    <NavTitle lijstNaam="Cliënten" />
                    <SearchClients />
                </Menu>

                {status === StatusEnum.REJECTED && id && !client &&
                    // TODO: Client should be shown, but is not found. Show error message.
                    <div>Client with client id '{id}' not found!</div>
                }

                {(status === StatusEnum.IDLE || status === StatusEnum.PENDING) &&
                    <div className='clients-spinner' data-testid="clients-spinner">
                    <FontAwesomeIcon icon={faSpinner} className="fa fa-3x fa-refresh fa-spin" />
                    </div>
                }

                {status === StatusEnum.SUCCESSFUL && client &&
                <div className="clients">
                {/* {Client main info 1 */}           
                    <Header text="Cliënt info" className='client-header' />

                    {/* Profile picture */}
                    <div className={`client-profile-picture`} style={{ 
                        gridRowStart: "span " + profilePicSpanValue,
                        gridRowEnd: "span "+profilePicSpanValue}}>
                        {/* TODO: Future feature */}
                        {/* <ProfilePicture /> */}
                    </div>

                    <div className='client-info-main'>                        
                        <Label dataTestId='client-fullname' text={client.firstname + ' ' + (!client.prefixlastname ? '' : client.prefixlastname + ' ')  + client.lastname} />
                        <Label dataTestId='client-address-part1' text={client.streetname + ' ' + client.housenumber + (!client.housenumberaddition ? '' : client.housenumberaddition)} />
                        <Label dataTestId='client-address-part2' text={client.postalcode + ' ' + client.residence} />
                    </div>

                    {/* Client number */}                    
                    <div className='client-number client-label-value'>
                        <Label text='Cliëntnummer: ' />
                        <Label data-testid="client-number-value" dataTestId='clientid' text={client.id+''} />
                    </div>
                    
                    {/* Client main info 2 */}
                    <div className='client-info-main-2'>                        
                        <div className='client-label-value'>
                            <Label text='Telefoon: ' />
                            <Label dataTestId='client-telephonenumber' text={client.telephonenumber} />
                        </div>

                        <div className='client-label-value'>
                            <Label text='E-mail: ' />
                            <Label dataTestId='client-emailaddress' text={client.emailaddress} />
                        </div>

                        <div className='client-label-value'>
                            <Label text='Geboortedatum: ' />
                            <Label dataTestId='client-dateofbirth' text={Moment(client.dateofbirth).format(DATE_FORMAT)} />
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

                    <SlideToggleLabel className='slidetoggle client-additional-info' text='Overige cliënt informatie' smallTextColapsed=' - klap uit voor meer opties' smallTextExpanded=' - klap in voor minder opties' >
                        <Label text='Overige informatie' className='client-subheader' strong={true} />
                        
                        <div className='client-additional-info-container'>
                            <div className='client-label-value'>
                                <Label text='Diagnose(s): ' />
                                <Label text={client.diagnoses ? client.diagnoses : NO_INFO} />
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
                                <Label text={client.benefitform ? client.benefitform : NO_INFO} />
                            </div>

                            <div className='client-label-value'>
                                <Label text='Doelgroepregister: ' />
                                <Label text={client.isintargetgroupregister ? "Ja" : "Nee"} />
                            </div>

                            {!client.workingcontracts &&
                            <Label text={NO_INFO} className='col-span-2' />}
                            {client.workingcontracts &&
                                client.workingcontracts!.map((workingContract, i) =>
                                    <div key={i} className='col-span-2 lg:grid lg:grid-cols-2 mt-5'>
                                        <div className='client-label-value'>
                                            <Label text={Moment(workingContract.todate) > Moment() ? 'Werkt bij: ' : 'Werkt(e) bij: ' } />
                                            <Label text={workingContract.organizationname} />
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
                                            <Label text={Moment(workingContract.fromdate).format(DATE_FORMAT)} />
                                        </div>
                                        <div className='client-label-value'>
                                            <Label text='Tot: ' />
                                            <Label text={Moment(workingContract.todate).format(DATE_FORMAT)} />
                                        </div>
                                </div>)
                            }
                        </div>

                        <div className='client-remarks'>
                            <Label text='Opmerkingen' className='client-subheader' strong={true} />
                            <Label text={client.remarks ? client.remarks : NO_INFO} className='col-span-2' />
                        </div>

                    </SlideToggleLabel>

                    <div className='button-container'>
                        <LinkButton buttonType={{type:"Solid"}} text="Cliënt aanpassen" href={`../clients/edit/${id}`} />
                        {/* TODO: Future feature */}
                        {/* <LinkButton buttonType={{type:"NotSolid"}} text="Urenoverzicht" href="../client-hours" /> */}
                    </div>

                    <div className='traject-container'>
                        <Header text="Traject info" className='traject-header' />
                        
                        <Dropdown
                            options={coachingPrograms.map(program => ({ 
                                label: program.title, 
                                value: program.id
                            }))} 
                            required={false} 
                            inputfieldname='coaching-program'
                            onChange={(value) => handleCoachingProgramChange(value)}
                            dataTestId='coaching-program' />

                        <div className='traject-default-values'>
                            <div className='traject-label-value'>
                                <Label text='BeginDatum: ' />
                                <Label text={currentCoachingProgram ? Moment(currentCoachingProgram.begindate).format(DATE_FORMAT) : ''} />
                            </div>

                            <div className='traject-label-value'>
                                <Label text='EindDatum: ' />
                                <Label text={currentCoachingProgram ? Moment(currentCoachingProgram.enddate).format(DATE_FORMAT) : ''} />
                            </div>

                            <div className='traject-label-value'>
                                <Label text='Traject type: ' />
                                <Label text={currentCoachingProgram ? currentCoachingProgram.coachingprogramtype : ''} />
                            </div>
                        </div>

                        {currentCoachingProgram !== null && 
                        <SlideToggleLabel 
                            className='slidetoggle' 
                            text='Overige cliënt informatie' 
                            smallTextColapsed=' - klap uit voor meer opties' 
                            smallTextExpanded=' - klap in voor minder opties' >

                            <div className='traject-additional-info'>
                                <div className='traject-label-value'>
                                    <Label text='Client: ' />
                                    <Label text={currentCoachingProgram.clientfullname} />
                                </div>

                                <div className='traject-label-value'>
                                    <Label text='Titel: ' />
                                    <Label text={currentCoachingProgram.title} />
                                </div>

                                <div className='traject-label-value'>
                                    <Label text='Ordernummer: ' />
                                    <Label text={currentCoachingProgram.ordernumber ? currentCoachingProgram.ordernumber : ''} />
                                </div>

                                <div className='traject-label-value'>
                                    <Label text='Organisatie: ' />
                                    <Label text={currentCoachingProgram.organizationname ? currentCoachingProgram.organizationname : ''} />
                                </div> 

                                <div className='traject-label-value'>
                                    <Label text='Budget: ' />
                                    <Label text={currentCoachingProgram.budgetammount ? formatCurrency(currentCoachingProgram.budgetammount) : ''} />
                                </div> 

                                <div className='traject-label-value'>
                                    <Label text='Uurtarief: ' />
                                    <Label text={currentCoachingProgram.hourlyrate ? formatCurrency(currentCoachingProgram.hourlyrate) : ''} />
                                </div>

                                <div className='traject-label-value'>
                                    <Label text='Te besteden uren: ' />
                                    <Label text={currentCoachingProgram.remaininghours ? currentCoachingProgram.remaininghours.toLocaleString() : ''} />
                                </div>
                            </div>

                        </SlideToggleLabel>}

                        <div className='traject-buttons'>
                            <LinkButton buttonType={{type:"Solid"}} text="Traject aanpassen" href={`../clients/${id}/coachingprogram-editor/${currentCoachingProgram?.id}`} />
                            <LinkButton buttonType={{type:"NotSolid"}} text="Nieuw traject" href={`../clients/${id}/coachingprogram-editor/`} />
                        </div>
                    </div>

                    <Button buttonType={{type:"Solid"}} text="Deactivateer cliënt" className='client-deactivate-button' onClick={() => deactivateClientClick(client)} dataTestId="button.deactivate" />
                    <ConfirmPopup
                        message="Weet u zeker dat u de cliënt wilt deactiveren?"
                        isOpen={isDeactivateConfirmPopupOpen}
                        onClose={cancelDeactivateClient}
                        buttons={
                            [
                                { text: 'Bevestigen', dataTestId: 'button.confirmok', onClick: async () => 
                                {
                                    setDeactivatedClient(await deactivateClientConfirmed(client, clientContext));

                                    if(deactivatedClient !== null){
                                        setDeactivateConfirmedPopupOpen(true);
                                    }
                                }, buttonType: {type:"Solid"}},
                                { text: 'Annuleren', onClick: cancelDeactivateClient, buttonType: {type:"NotSolid"}},
                            ]} />                    

                    <ConfirmPopup
                        message={`Cliënt met id '${deactivatedClient ? deactivatedClient!.id : ''}' is gedeactiveerd!`}
                        isOpen={isDeactivateConfirmedPopupOpen}
                        onClose={closeConfirmedDeactivateClientPopUp}
                        buttons={
                            [
                                { text: 'Ok', dataTestId: 'button.confirmdeactivated', onClick: () => 
                                {
                                    if(deactivateClient !== null) {
                                        nav(PATH_CLIENTS);
                                    }
                                }, buttonType: {type:"Solid"}},
                            ]} />
                    
                </div>
                }
            </div>
            <Copyright />
        </div>
    )
}

export default Clients;
