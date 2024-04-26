import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './pages/App';
import Clients from './pages/Clients';
import ClientCreate from './pages/client-create';
import reportWebVitals from './utils/reportWebVitals';
import preval from 'preval.macro'

import { BrowserRouter, Routes, Route } from "react-router-dom";
import { ClientContextWrapper } from './pages/client-context';

const root = ReactDOM.createRoot(document.getElementById('root')!);
root.render(
    <React.StrictMode>
        <BrowserRouter>
            <Routes>
                <Route path='/' element={<App />} />
                <Route path='/Clients/:id?' element={<ClientContextWrapper />} />
                <Route path='/client/new' element={<ClientCreate />} />
            </Routes>
        </BrowserRouter>
    </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
