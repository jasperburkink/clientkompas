import SidebarEmpty from 'components/sidebar/sidebar-empty';
import './unauthorized.css';
import Menu from "components/common/menu";


const Unauthorized = () => {

    return(
        <div className="flex flex-col lg:flex-row h-screen lg:h-auto">
            <div className='lg:flex w-full'>
                <SidebarEmpty />
                
                <section className="statuscode-container">
                    <h1>401</h1>
                    <h2>Unauthorized - Niet ingelogd</h2>                    
                    <p>Het lijkt erop dat u niet ingelogd bent. U kunt opnieuw inloggen door <a href="/login">hier</a> te klikken.</p>
                </section>
            </div>
        </div>
    );
}

export default Unauthorized;