import './forbidden.css';
import Menu from "components/common/menu";


const Forbidden = () => {

    return(
        <div className="flex flex-col lg:flex-row h-screen lg:h-auto">
            <div className='lg:flex w-full'>
                <div id='staticSidebar' className='sidebarContentPush'></div>
                
                <Menu />

                <section className="statuscode-container">
                    <h1>403</h1>
                    <h2>Forbidden - Onvoldoende rechten</h2>                    
                    <p>
                        Het lijkt erop dat u geen toegang heeft tot deze pagina. <br />
                        Is dat wel de bedoeling? Neem dan contact op met uw systeembeheerder. <br />
                        U kunt uw weg vervolgen door het menu links te gebruiken.
                    </p>
                </section>
            </div>
        </div>
    );
}

export default Forbidden;