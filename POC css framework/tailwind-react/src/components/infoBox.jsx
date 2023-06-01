import '../index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleDown } from "@fortawesome/free-solid-svg-icons";

export function InfoBox(props) {
    const moreInformation = (moreInfo) => {
        console.log(moreInfo)
        
        //var icon = (moreInfo.childNodes[1])
        var icon = document.getElementById('moreInfo').childNodes[1]
        console.log(icon)

        if(icon.classList.contains('fa-angle-up'))
        {
            document.getElementById("info").style.maxHeight = "300px";
            icon.classList.remove("fa-angle-up");
            icon.classList.add("fa-angle-down");
            document.getElementById("clientBtns").style.marginTop = "-150px";
        }
        else
        {
            document.getElementById("info").style.maxHeight = "873px";
            icon.classList.remove("fa-angle-down");
            icon.classList.add("fa-angle-up");
            document.getElementById("clientBtns").style.marginTop = "0";
        }
    }
    if(props.type === "moreInfo"){
        return(
            <div id="info" className="infoBox col-span-2 snap-center">
                {props.children}
                <div id="clientBtns" className="flex flex-wrap content-center sticky p-3 md:static bottom-0 w-screen md:w-full h-100px bg-white md:h-[175px] md:-mt-[150px] z-30 ease-in-out duration-500 md:bg-gradient-to-t md:from-white md:from-30% md:to-transparent md:to-30%">
                    <div className="bg-white h-fit w-full md:h-full md:w-1/2 flex justify-between md:flex-col md:content-end md:flex-wrap md:gap-3 md:ml-auto">
                        <button className="btnSollid">{props.button1}</button>
                        <button className="btnNotSollid">{props.button2}</button>
                        <div id='moreInfo' onClick={moreInformation} className="hidden md:flex mt-auto text-sky-500 underline ml-auto cursor-pointer">
                            meer informatie
                            <FontAwesomeIcon icon={faAngleDown} textDecoration="underline" underlinePosition={2} id="moreInfoArrow" className="underline h-4" />
                        </div>
                    </div>
                </div>
            </div>
        )
    }else{
        return (
            <div id="info" className="infoBox col-span-2 snap-center">
                {props.children}
                <div id="clientBtns" className="md:h-[175px] w-screen md:w-full z-30">
                    <div className="h-full flex flex-col content-end flex-wrap gap-3 ml-auto">
                        <button className="btnSollid">{props.button1}</button>
                        <button className="btnNotSollid">{props.button2}</button>
                    </div>
                </div>
            </div>
        );
    }
    
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