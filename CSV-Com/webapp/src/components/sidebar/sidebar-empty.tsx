import 'index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBars } from "@fortawesome/free-solid-svg-icons";

export default function SidebarEmpty(props: any) {  
    return (
      <div className=' z-20'>
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