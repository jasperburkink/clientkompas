import React, { LabelHTMLAttributes } from 'react';
import { useState } from "react";
import './profile-picture.css';
import { Button } from '../common/button';
import { Picture } from '../common/picture';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCircleUser } from "@fortawesome/free-solid-svg-icons";

interface ProfilePictureProps extends React.HtmlHTMLAttributes<HTMLElement> {
    pictureUrl?: string
}

export function ProfilePicture(props: ProfilePictureProps){
    const [pictureUrl, setPictureUrl] = useState(props.pictureUrl);
    return (
        <div >
            <div className='profile-picture-box'>
                {
                    pictureUrl === undefined &&
                    <FontAwesomeIcon icon={faCircleUser} className="fa-sollid fa-10x text-white"/>
                }
                {
                    pictureUrl !== null &&
                    <Picture source={pictureUrl!} />
                }
            </div>
            <div className='profile-picture-buttons'>
                {/* // TODO: Create upload logic: https://codefrontend.com/file-upload-reactjs/ */}
                <Button buttonType={{type:"Underline"}} text="Uploaden" onClick={() => setPictureUrl('https://media.licdn.com/dms/image/C4D03AQGUdnfW30XcEQ/profile-displayphoto-shrink_800_800/0/1548058154952?e=2147483647&v=beta&t=vdVdQcRNraGV1oV8NhgYX6NtULqUhEfcJp_7VkcnWds') } />
                <Button buttonType={{type:"Underline"}} text="Verwijderen" onClick={() => setPictureUrl(undefined)} />
            </div>
        </div>
    );    
}