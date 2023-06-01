import '../index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleRight } from "@fortawesome/free-solid-svg-icons";
import { faSortDown } from "@fortawesome/free-solid-svg-icons";

export function SidebarGray(props) {
  const openCloseGraySideBar = () => {
      var rotatedeg = document.getElementById("sidebarArrow").style.rotate;

      if(rotatedeg === "180deg")
        {
            document.getElementById("sidebarGray").style.marginLeft = "-430px"
            document.getElementById("sidebarArrow").style.rotate = "0deg";
        }
        else
        {
            document.getElementById("sidebarGray").style.marginLeft = "-100px";
            document.getElementById("sidebarArrow").style.rotate = "180deg";
        }
  }
  const openCloseGraySideBarMobile = () => {
    var rotatedeg = document.getElementById("sidebarArrowMobile").style.rotate;

        if(rotatedeg === "180deg")
        {
            document.getElementById("sidebarGray").style.maxHeight = "40px"
            document.getElementById("sidebarArrowMobile").style.paddingBottom = "8px";
            document.getElementById("sidebarArrowMobile").style.rotate = "0deg";
        }
        else
        {
            document.getElementById("sidebarGray").style.maxHeight = "50vh"
            document.getElementById("sidebarArrowMobile").style.paddingBottom = "0px";
            document.getElementById("sidebarArrowMobile").style.rotate = "180deg";
        }
}
  return (
    <div id="sidebarGray" className="sidebarGray">
      <div className="navBtnsContainerGray"> 
        {props.children}
      </div>
      <div className="sidebarLineGray"></div>
      <div className="sidebarDarkLineGray"></div>

      <FontAwesomeIcon id="sidebarArrow" icon={faAngleRight} className="sidebarArrow fa-solid  fa-3x hidden lg:block" onClick={openCloseGraySideBar} />
      <FontAwesomeIcon id="sidebarArrowMobile" icon={faSortDown} className="sidebarArrow fa-solid  fa-2x lg:hidden pb-2" onClick={openCloseGraySideBarMobile} />
    </div>
  );
}