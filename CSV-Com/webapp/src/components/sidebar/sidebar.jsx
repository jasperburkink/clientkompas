import '../../index.css';

import logo from "../../assets/sprong_logo_svg.svg";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBars } from "@fortawesome/free-solid-svg-icons";

export function Sidebar(props) {
  const openCloseBlueSideBar = () => {
    var rotatedeg = document.getElementById("hamburger").style.rotate;

    if(rotatedeg === "90deg")
    {
      document.getElementById("sidebar").style.height ="0px";
      document.getElementById("hamburger").style.rotate = "0deg";
      document.body.style.overflow ="hidden";
    }
    else
    {
      document.getElementById("sidebar").style.height ="100vh";
      document.getElementById("hamburger").style.rotate = "90deg";
      document.body.style.overflow ="hidden";
    }
  }
    return (
      <div className=' z-20'>
      <div className="logoBar">
        <img src={logo} className="w-150px lg:w-[60%]" alt='Logo'/>
        <FontAwesomeIcon icon={faBars} id="hamburger" className="lg:hidden h-12 text-white ease-in-out duration-500 my-auto" onClick={openCloseBlueSideBar} />
      </div>
      <div id="sidebar" className="sidebar">
        <div className="navBtnsContainer"> 
          {props.children}
        </div>
        <div className="sidebarLine"></div>
        <div className="sidebarDarkLine"></div>
      </div>
      </div>
    );
  }