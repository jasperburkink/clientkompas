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
            <NavTitle lijstNaam="Medewerkers" />
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

            <Header text="Medewerker gegevens" className='user-subheader' />

            <Label text={user.fullName} />
            <Label text={user.emailaddress} />
            <Label text={user.telephonenumber} />            
            <LabelField text='Rol' required={true}>
              <Label text={user.role} />
            </LabelField>

            <LabelField text='Gebruikersnaam' required={true}>
              <Label text={user.emailaddress} />
            </LabelField>

            {user.createdbyuserdescription &&
            <LabelField text='Aangemaakt door:' required={true}>
              <Label text={user.createdbyuserdescription} />
            </LabelField>
            }

            {user.deactivationdatetime &&
            <LabelField text='Gedeactiveerd op' required={true}>
              <Label text={user.deactivationdatetime.toString()} />
            </LabelField>
            }
            <div className='button-container'>
                <LinkButton buttonType={{type:"Solid"}} text="Aanpassen" href={`../user/edit/${id}`} />
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