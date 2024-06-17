import './organization-details.css';

import React, { useContext, useEffect, useState } from "react";
import { useNavigate, useParams } from 'react-router-dom';
import { Header } from 'components/common/header';
import { Label } from 'components/common/label';
import { LinkButton } from 'components/common/link-button';
import { Button } from 'components/common/button';
import { Sidebar } from 'components/sidebar/sidebar';
import { NavButton } from 'components/nav/nav-button';
import { SidebarGray } from 'components/sidebar/sidebar-gray';
import { Copyright } from 'components/common/copyright';
import { NavTitle } from 'components/nav/nav-title';
import 'utils/utilities';
import Moment from 'moment';
import { fetchOrganization } from 'utils/api';
import SearchClients from 'components/clients/search-clients';
import StatusEnum from 'types/common/StatusEnum';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import { IOrganizationContext, OrganizationContext } from './organization-context';
import Organization from 'types/model/Organization';
import ConfirmPopup from 'components/common/confirm-popup';

const NO_INFO = 'Geen informatie beschikbaar';
const DATE_FORMAT = 'DD-MM-yyyy';
const PATH_ORGANIZATIONS = '/Organization/';

function OrganizationDetails() {

    const organizationContext = useContext(OrganizationContext)
    const [organization, setOrganization] = useState<Organization | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [status, setStatus] = useState(StatusEnum.IDLE);

    var { id } = useParams();

    // Show dates in local format
    Moment.locale('nl');

    const nav = useNavigate();        

    const fetchOrganizationById = async () => {
        try {
          setStatus(StatusEnum.PENDING);
          const fetchedOrganization: Organization = await fetchOrganization(id!);
          setStatus(StatusEnum.SUCCESSFUL);

          setOrganization(fetchedOrganization);
          
        } catch (e) {
          // TODO: error handling
          console.error(e);
          setStatus(StatusEnum.REJECTED);
          //setError(e);
        }
      };

    // Get organization by id
    useEffect(() => {
        if(!id) {
            setStatus(StatusEnum.REJECTED);
            return;
        }
    
        fetchOrganizationById();
      }, [id]);
    
    return (
        <div className="flex flex-col lg:flex-row h-screen lg:h-auto">
            <div className='lg:flex w-full'>
            <div id='staticSidebar' className='sidebarContentPush'></div>
                {/* TODO: Move menu to own control. Is already a task on the backlog. */}
                <div className='header-menu fixed'>
                    <Sidebar>
                        <NavButton text="Cliënten" icon="Gebruikers" />
                        <NavButton text="Uren registratie" icon="Klok" />
                        <NavButton text="Organistatie" icon="Gebouw" />
                        <NavButton text="Gebruiker" icon="Gebruiker" />
                        <NavButton text="Uitloggen" icon="Uitloggen" />
                    </Sidebar>
                    <SidebarGray>
                        <NavTitle lijstNaam="Organisatie" />
                        <SearchClients />
                    </SidebarGray>    
                </div>

                {status === StatusEnum.REJECTED && id && !organization &&
                    // TODO: Organization should be shown, but is not found. Show error message.
                    <div>Organization with organization id ${id} not found!</div>
                }

                {(status === StatusEnum.IDLE || status === StatusEnum.PENDING) &&
                    <div className='organizations-spinner'>
                    <FontAwesomeIcon icon={faSpinner} className="fa fa-3x fa-refresh fa-spin" />
                    </div>
                }

                {status === StatusEnum.SUCCESSFUL && organization &&
                <div className="organizations">
                {/* {Organization main info 1 */}           
                    <Header text="Organisatie details" />

                    <div className='organization-info-main-visitadres'>
                        <Label text='Bezoekadres' className='organization-subheader' strong={true} />
                            <Label text={organization.organizationname} />
                            <Label text={organization.visitstreetname + ' ' + organization.visithousenumber + (!organization.visithousenumberaddition ? '' : organization.visithousenumberaddition)} />
                            <Label text={organization.visitpostalcode + ' ' + organization.visitresidence} />
                    </div>

                    <div className='organization-info-main-contact'>
                        <Label text='Contact' className='organization-subheader' strong={true} />
                            <div className='organization-label-value'>
                                <Label text='Telefoon: ' />
                                <Label text={organization.phonenumber} />
                            </div>

                            <div className='organization-label-value'>
                                <Label text='E-mail: ' />
                                <Label text={organization.emailaddress} />
                            </div>

                            <div className='organization-label-value'>
                                <Label text='Website: ' />
                                <Label text={organization.website} />
                            </div>
                    </div>

                    <div className='organization-info-main-post_invoiceadres'>
                        <Label text='Postadres' className='organization-subheader' strong={true} />
                            <Label text={organization.poststreetname + ' ' + organization.posthousenumber + (!organization.posthousenumberaddition ? '' : organization.posthousenumberaddition)} />
                            <Label text={organization.postpostalcode + ' ' + organization.postresidence} />

                            <Label text='Factuuradres' className='organization-subheader' strong={true} />
                            <Label text={organization.invoicestreetname + ' ' + organization.invoicehousenumber + (!organization.invoicehousenumberaddition ? '' : organization.invoicehousenumberaddition)} />
                            <Label text={organization.invoicepostalcode + ' ' + organization.invoiceresidence} />
                    </div>

                    <div className='organization-info-main-contactperson'>
                        <Label text='Contactpersoon' className='organization-subheader' strong={true} />
                            <Label text={organization.contactpersonname + ' - ' + organization.contactpersonfunction} />

                            <div className='organization-label-value'>
                                <Label text='Mobiel: ' />
                                <Label text={organization.contactpersonmobilephonenumber} />
                            </div>

                            <div className='organization-label-value'>
                                <Label text='Telefoon: ' />
                                <Label text={organization.contactpersontelephonenumber} />
                            </div>

                            <div className='organization-label-value'>
                                <Label text='E-mail: ' />
                                <Label text={organization.contactpersonemailaddress} />
                            </div>
                    </div>

                    <div className='organization-info-main-organizationinfo'>
                        <Label text='Bedrijfsinformatie' className='organization-subheader' strong={true} />
                            <div className='organization-label-value'>
                                <Label text='KVK-nummer: ' />
                                <Label text={organization.kvknumber} />
                            </div>

                            <div className='organization-label-value'>
                                <Label text='BTW-nummer: ' />
                                <Label text={organization.btwnumber} />
                            </div>

                            <div className='organization-label-value'>
                                <Label text='IBAN: ' />
                                <Label text={organization.ibannumber} />
                            </div>

                            <div className='organization-label-value'>
                                <Label text='BIC: ' />
                                <Label text={organization.bic} />
                            </div>
                    </div>
                    <div className='button-container'>
                        <LinkButton buttonType={{type:"Solid"}} text="Aanpassen" href={`../organization/edit/${id}`} />
                        <Button buttonType={{type:"NotSolid"}} className='organization-deactivate-button' text="Verwijder organisatie" />
                    </div>
                </div>
                }
            </div>
            <Copyright />
        </div>
    )
}

export default OrganizationDetails;
