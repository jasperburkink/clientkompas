import './index.css';

import { Copyright } from './components/copyright';
import { Button } from './components/button';
import { ProfilePicture } from './components/profilePicture';
import { InfoBox } from './components/infoBox';
import { InfoBoxPartClientInfo } from './components/infoBoxPartClientInfo';
import { InfoBoxPartTrajectInfo } from './components/infoBoxPartTrajectInfo';
import React, { useEffect, useState } from "react";
import { useParams } from 'react-router-dom';
import { SidebarFull } from './components/sidebarFull';

function Clients() {
    var [ client, setClient ] = useState(null);
    var { id } = useParams();

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

    if(client == null) {
        return "loading...";
    };
    if(id == null) {
        id = 0
    };

    return (
        <div className="flex flex-col lg:flex-row h-screen lg:h-auto">
            <SidebarFull client={client} />
            <div className="flex w-screen md:w-full overflow-scroll snap-x snap-mandatory md:overflow-visible md:grid md:grid-cols-3 md:grid-rows-infoBox md:m-5 lg:my-100px lg:mx-50px lg:gap-clienten">
                <InfoBox type="Client" buttonPrimaryText="Cliënt Aanpassen" buttonSecondaryText="Urenoverzicht" classNameMoreInfoBtns="md:bg-gradient-to-t md:from-white md:from-30% md:to-transparent md:to-30% ">
                     <InfoBoxPartClientInfo client={client[id]} geboortedatum="1-2-2000" />
                </InfoBox>
                <ProfilePicture />
                <InfoBox type="Traject" buttonPrimaryText="Traject Aanpassen" buttonSecondaryText="Nieuw Traject" classNameMoreInfoBtns="md:bg-gradient-to-t md:from-white md:from-70% md:to-transparent md:to-70% ">
                    <InfoBoxPartTrajectInfo 
                        ordernummer="1234" trajectType="Jobcoach extern" opdrachtgever="UWV" 
                        begindatum="01-01-2022" einddatum="31-12-2022" budgetBedrag="5000" uurtarief="40"
                        teBestedenUren="125" coachWerktVoor="/"
                    />
                </InfoBox>
                <div className='h-[300px] flex flex-wrap justify-end content-end'>
                    <Button type="btnSollid" text="De-activeer cliënt" className="hidden md:block"/>
                </div>
                
            </div>
            <Copyright />
        </div>
    )
}

export default Clients;