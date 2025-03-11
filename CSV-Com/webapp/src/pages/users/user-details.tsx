import './user-details.css'
import Menu from "components/common/menu";
import { NavTitle } from "components/nav/nav-title";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import ApiResultOld from "types/common/api-result-old";
import StatusEnum from "types/common/StatusEnum";
import GetUserDto from "types/model/user/get-user/get-user-query-dto";
import { fetchUser } from "utils/api";
import Searchusers from "./search-users";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSpinner } from "@fortawesome/free-solid-svg-icons";
import { Header } from "components/common/header";
import LabelField from "components/common/label-field";
import { Label } from "components/common/label";
import { LinkButton } from "components/common/link-button";
import { Button } from "components/common/button";
import ApiResult from "types/common/api-result";


export default function UserDetails() {
    var { id } = useParams();    

    const [user, setUser] = useState<GetUserDto | null>(null);
    const [status, setStatus] = useState(StatusEnum.IDLE);

    useEffect(() => {
      if(!id) {
          setStatus(StatusEnum.REJECTED);
          return;
      }

      const fetchUserById = async () => {
        try {
          setStatus(StatusEnum.PENDING);
          const fetchUserResult: ApiResult<GetUserDto> = await fetchUser(id!);

          if(fetchUserResult.succeeded && fetchUserResult.value){
            setUser(fetchUserResult.value);
          }
          else{
            // TODO: Set error
          }
          
          setStatus(StatusEnum.SUCCESSFUL);
        } catch (e) {
          // TODO: error handling
          console.error(e);
          setStatus(StatusEnum.REJECTED);
          //setError(e);
        }
      };

      fetchUserById();        

  }, [id]);

  function deactivateUserClick(user: GetUserDto): void {
    throw new Error("Function not implemented.");
  }

  function removeUserClick(user: GetUserDto): void {
    throw new Error("Function not implemented.");
  }

  return (
    <div className="flex flex-col lg:flex-row h-screen lg:h-auto">
      <div className='lg:flex w-full'>
        <div id='staticSidebar' className='sidebarContentPush'></div>

        <Menu>
            <NavTitle lijstNaam="Medewerkerslijst" />
            <Searchusers />
        </Menu>

        {status === StatusEnum.REJECTED && id && !user &&
            // TODO: Client should be shown, but is not found. Show error message.
            <div>User with id '{id}' not found!</div>
        }

        {(status === StatusEnum.IDLE || status === StatusEnum.PENDING) &&
            <div className='clients-spinner' data-testid="clients-spinner">
            <FontAwesomeIcon icon={faSpinner} className="fa fa-3x fa-refresh fa-spin" />
            </div>
        }

        {status === StatusEnum.SUCCESSFUL && user &&
        <div className="user">        
            <Header text="Medewerker" className='user-header' />

            <Header text="Medewerkergegevens" className='user-subheader' />

            <Label className='user-label' text={user.fullname} />
            <Label className='user-label' text={user.emailaddress} />
            <Label className='user-label' text={user.telephonenumber} />
            <Label className='user-label' text={`Rol: ${user.role}`}  />

            <span className='user-whitespace' />

            <Label className='user-label' text={`Gebruikersnaam: ${user.emailaddress}`}  />

            {user.createdbyuserdescription &&
            <div>
              <span className='user-whitespace' />
              <Label className='user-label' text={`Aangemaakt door: ${user.createdbyuserdescription}`}  />
            </div>
            }

            {user.deactivationdatetime &&
            <div>
              <Label className='user-label' text={`Gedeactiveerd op: ${user.deactivationdatetime.toString()}`}  />
              <span className='user-whitespace' />
            </div>
            }

            <span className='user-whitespace' />

            <div className='button-container'>
                <LinkButton buttonType={{type:"Solid"}} text="Aanpassen" href={`../users/edit/${id}`} />
                {!user.deactivationdatetime &&
                <Button buttonType={{type:"NotSolid"}} text="Non actief" onClick={() => deactivateUserClick(user)} dataTestId="button.deactivate" />
                }
                {user.deactivationdatetime &&
                <Button buttonType={{type:"NotSolid"}} text="Verwijderen" onClick={() => removeUserClick(user)} dataTestId="button.remove" />
                }
            </div>
        </div>
        }

      </div>
    </div>
  );
}