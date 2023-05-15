import React, { useState, useEffect } from "react";
import logo from './logo.svg';
import './App.css';

function App() {
  var [ i, setI ] = useState(0);

  var [ declaratieLijst, setDeclaratieLijst ] = useState(null);

  setDeclaratieLijst(apis.declaratieApi.getAllDeclaraties());

  var changeFunc = () => {
    setI(i + 1);
  };

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p>
        <button onClick={changeFunc}>test</button>
        <p>
          het nummer is: {i}
        </p>
        {
          declaratieLijst.map(a => 
          <div>
            <h2>{a.titel}</h2>
            <p>{a.summary}</p>
          </div>
          )
        }
        <a
          className="App-link"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
    </div>
  );
}

export default App;
