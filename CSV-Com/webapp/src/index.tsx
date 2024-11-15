import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './pages/App';
import reportWebVitals from './utils/reportWebVitals';

import { BrowserRouter, Routes, Route } from "react-router-dom";
import { ClientContextWrapper, ClientRoute } from './pages/client-context';
import { OrganizationContextWrapper, OrganizationRoute } from 'pages/organization-context';
import Login from 'pages/login';
import Unauthorized from 'pages/statuspages/unauthorized';
import Forbidden from 'pages/statuspages/forbidden';
import RequestResetPassword from 'pages/request-reset-password';
import ResetPassword from 'pages/reset-password';
import Login2FA from 'pages/login-2fa';

const root = ReactDOM.createRoot(document.getElementById('root')!);
root.render(
    <>
        <React.StrictMode>
            <BrowserRouter>
                <Routes>
                    <Route path='/' element={<App />} />
                    <Route path='/login' element={<Login />} />
                    <Route path='/login-2fa/:userid' element={<Login2FA />} />
                    <Route path='/password-forgotten' element={<RequestResetPassword />} />
                    <Route path='/reset-password/:emailaddress/:token' element={<ResetPassword />} />
                    <Route path='/unauthorized' element={<Unauthorized />} />
                    <Route path='/forbidden' element={<Forbidden />} />
                    <Route path='/clients' element={<ClientContextWrapper clientRoute={ClientRoute.VIEW_CLIENT} />} />
                    <Route path='/clients/:id' element={<ClientContextWrapper clientRoute={ClientRoute.VIEW_CLIENT} />} />
                    <Route path='/clients/edit' element={<ClientContextWrapper clientRoute={ClientRoute.EDIT_CLIENT} />} />
                    <Route path='/clients/edit/:id' element={<ClientContextWrapper clientRoute={ClientRoute.EDIT_CLIENT} />} />
                    <Route path='/organization/:id' element={<OrganizationContextWrapper organizationRoute={OrganizationRoute.VIEW_ORGANIZATION} />} />
                    <Route path='/organization/edit' element={<OrganizationContextWrapper organizationRoute={OrganizationRoute.EDIT_ORGANIZATION} />} />
                    <Route path='/organization/edit/:id' element={<OrganizationContextWrapper organizationRoute={OrganizationRoute.EDIT_ORGANIZATION} />} />
                    <Route path='/clients/:clientid/coachingprogram-editor' element={<ClientContextWrapper clientRoute={ClientRoute.EDIT_CLIENT_COACHINGPROGRAM} />} />
                    <Route path='/clients/:clientid/coachingprogram-editor/:id' element={<ClientContextWrapper clientRoute={ClientRoute.EDIT_CLIENT_COACHINGPROGRAM} />} />
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
