import React, { LabelHTMLAttributes } from 'react';
import { useState } from "react";
import '../../styles/common/profile-picture.css';
import { Button } from '../common/button';
import { Picture } from '../common/picture';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCircleUser } from "@fortawesome/free-solid-svg-icons";



interface ProfilePictureProps extends React.HtmlHTMLAttributes<HTMLElement> {
    
    pictureUrl?: string,
    // onRemoveProfilePicture: void
}

export const ProfilePicture = (props: ProfilePictureProps) => (
    // const [pictureUrl, setPictureUrl] = useState("red");

    <div>
        <div className="profile-picture-box">
            {
                props.pictureUrl === undefined &&
                <FontAwesomeIcon icon={faCircleUser} className="fa-sollid fa-10x text-white"/>
            }
            {
                props.pictureUrl !== null &&
                <Picture source={props.pictureUrl!} className='rounded-3xl' />
            }
        </div>
        <div className='flex justify-between py-4'>
            <Button buttonType={{type:"Underline"}} text="Uploaden" onClick={()=> {alert('Uploaden');}} />
            <Button buttonType={{type:"Underline"}} text="Verwijderen" onClick={RemoveProfilePicture} />
        </div>
    </div>

    
    
);

const RemoveProfilePicture = () =>
    {
        alert('woot!');
    }
