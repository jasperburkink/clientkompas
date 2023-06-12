import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import Clients from './Clients';
import ClientsAdd from './ClientsAdd';
import ClientsEdit from './ClientsEdit';
import reportWebVitals from './reportWebVitals';

import { BrowserRouter, Routes, Route } from "react-router-dom";

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path='/' element={<App/>} />
        <Route path='/Clients' element={<Clients/>}/>
        <Route path='/Clients/:id' element={<Clients/>}/>
        <Route path='/ClientsAdd' element={<ClientsAdd/>}/>
        <Route path='/ClientsAdd/:id' element={<ClientsAdd/>}/>
        <Route path='/ClientsEdit' element={<ClientsEdit/>}/>
        <Route path='/ClientsEdit/:id' element={<ClientsEdit/>}/>
        {/* <Route path="/user/edit/{id}" element={<EditUser mode="edit"/>}/>
        <Route path="/user/new" element={<EditUser mode="add"/>}/> */}
      </Routes>
    </BrowserRouter>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
