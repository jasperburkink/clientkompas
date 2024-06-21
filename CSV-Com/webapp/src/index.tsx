import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './pages/App';
import reportWebVitals from './utils/reportWebVitals';
import preval from 'preval.macro'

import { BrowserRouter, Routes, Route } from "react-router-dom";
import { ClientContextWrapper, ClientRoute } from './pages/client-context';
import { OrganizationContextWrapper, OrganizationRoute } from 'pages/organization-context';

const root = ReactDOM.createRoot(document.getElementById('root')!);
root.render(
    <>
        <React.StrictMode>
            <BrowserRouter>
                <Routes>
                    <Route path='/' element={<App />} />
                    <Route  path='/clients' element={<ClientContextWrapper clientRoute={ClientRoute.VIEW_CLIENT} />} />
                    <Route path='/clients/:id' element={<ClientContextWrapper clientRoute={ClientRoute.VIEW_CLIENT} />} />
                    <Route path='/clients/edit' element={<ClientContextWrapper clientRoute={ClientRoute.EDIT_CLIENT} />} />
                    <Route path='/clients/edit/:id' element={<ClientContextWrapper clientRoute={ClientRoute.EDIT_CLIENT} />} />
                    <Route path='/organization/:id' element={<OrganizationContextWrapper organizationRoute={OrganizationRoute.VIEW_ORGANIZATION} />} />
                    <Route path='/organization/edit' element={<OrganizationContextWrapper organizationRoute={OrganizationRoute.EDIT_ORGANIZATION} />} />
                    <Route path='/organization/edit/:id' element={<OrganizationContextWrapper organizationRoute={OrganizationRoute.EDIT_ORGANIZATION} />} />
                </Routes>
            </BrowserRouter>
        </React.StrictMode>
        {/* <span>Build Date: {preval`module.exports = new Date().toLocaleString();`}.</span> */}
         {/* TODO: Uitgezet want dit was zichtbaar op elke pagina */}
    </>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
