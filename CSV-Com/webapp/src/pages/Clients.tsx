import '../index.css';

import { Copyright } from '../components/common/copyright';
import { Button } from '../components/common/button';
import { ProfilePicture } from '../components/common/profile-picture';
import React, { useEffect, useState } from "react";
import { useParams } from 'react-router-dom';
import { SidebarFull } from '../components/sidebar/sidebar-full';
import { Sidebar } from '../components/sidebar/sidebar';

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
    []
    ); 

    return (
        <div className="flex flex-col lg:flex-row h-screen lg:h-auto">
            <SidebarFull client={client} />
            <div className="flex w-screen md:w-full overflow-scroll snap-x snap-mandatory md:overflow-visible md:grid md:grid-cols-3 md:grid-rows-infoBox md:m-5 lg:my-100px lg:mx-50px lg:gap-clienten">
               
                <ProfilePicture />

                <div className='h-[300px] flex flex-wrap justify-end content-end'>
                    <Button buttonType={{type:'Solid'}} text="De-activeer cliënt" className="hidden md:block" onClick={()=>{}} />
                </div>
                
            </div>
            <Copyright />
        </div>
    )
}

export default Clients;