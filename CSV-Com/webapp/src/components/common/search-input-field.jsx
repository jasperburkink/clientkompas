import '../../index.css';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faMagnifyingGlass } from "@fortawesome/free-solid-svg-icons";

export function SearchInputField(props) {
    return (
        <form className="py-3">   
            <div className="relative">
              <input className="inputField" placeholder="Zoeken"/>
              <button type="submit" className="absolute right-2.5 bottom-0 top-0">
                <FontAwesomeIcon icon={faMagnifyingGlass} />
              </button>
            </div>
        </form>
    )
}