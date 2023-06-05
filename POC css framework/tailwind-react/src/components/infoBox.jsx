import '../index.css';

import React, { useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleDown } from "@fortawesome/free-solid-svg-icons";
import { faAngleUp } from "@fortawesome/free-solid-svg-icons";

import { library } from "@fortawesome/fontawesome-svg-core";

library.add(faAngleDown, faAngleUp);

export function InfoBox(props) {
    const [icons, setIcons] = useState('angle-down');
    const moreInformationClient = () => {
        if(props.type === "Client"){
            if(icons === 'angle-up')
            {
                setIcons('angle-down')
                document.getElementById("infoClient").style.maxHeight = "330px";
                document.getElementById("moreInfoBtnsClient").style.marginTop = "-150px";
            }
            else
            {
                setIcons('angle-up')
                document.getElementById("infoClient").style.maxHeight = "873px";
                document.getElementById("moreInfoBtnsClient").style.marginTop = "0";
            }
        }else if(props.type === "Traject"){
            if(icons === 'angle-up')
            {
                setIcons('angle-down')
                document.getElementById("infoTraject").style.maxHeight = "300px";
                document.getElementById("moreInfoBtnsTraject").style.marginTop = "-150px";
            }
            else
            {
                setIcons('angle-up')
                document.getElementById("infoTraject").style.maxHeight = "873px";
                document.getElementById("moreInfoBtnsTraject").style.marginTop = "0";
        }
        }else{
            return(alert("error: InfoBox must have type 'Client' or 'Traject'"))
        }
    }
    return(
        <div id={"info" + props.type} className="infoBox col-span-2 snap-center">
            {props.children}
            <div id={"moreInfoBtns" + props.type} className={"flex flex-wrap content-center sticky md:static bottom-0 w-screen md:w-full h-100px md:h-[175px] p-3 z-30 md:-mt-[150px] bg-white md:bg-transparent md:ease-in-out md:duration-500 " + (props.classNameMoreInfoBtns)}>
                <div className="bg-white h-fit w-full md:h-full md:w-1/2 flex justify-between md:flex-col md:content-end md:flex-wrap md:gap-3 md:ml-auto">
                    <button className="btnSollid">{props.buttonPrimaryText}</button>
                    <button className="btnNotSollid">{props.buttonSecondaryText}</button>
                    <div id={'moreInfo' + props.type} onClick={moreInformationClient} className="hidden md:flex mt-auto text-sky-500 underline ml-auto cursor-pointer">
                        meer informatie
                        <FontAwesomeIcon icon={icons} textDecoration="underline" underlinePosition={2} id="moreInfoArrow" className="underline h-4" />
                    </div>
                </div>
            </div>
        </div>
    )
    
}
            /* <div id="clientBtns"  className="absolute h-[175px] w-full top-[150px] z-30 ease-in-out duration-500 bg-gradient-to-t from-white from-30% to-transparent to-30%">
                <div className="relative h-full">
                    <div className="bg-white h-full w-1/2 flex flex-col content-end flex-wrap gap-3 absolute right-0">
                        <button className="btnSollid">{props.button1}</button>
                        <button className="btnNotSollid">{props.button2}</button>
                        <div id='moreInfo' onClick={moreInformation} className="mt-auto text-sky-500 underline ml-auto flex">
                            meer informatie
                            <FontAwesomeIcon icon={faAngleDown} textDecoration="underline" underlinePosition={2} id="moreInfoArrow" className="underline h-4" />
                        </div>
                    </div>
                </div>
            </div> */