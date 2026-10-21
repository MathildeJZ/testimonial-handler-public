//react core
import React from 'react';

//libraries
import { BrowserRouter } from "react-router-dom";
import ReactDOM from 'react-dom/client';

//local files
import App from './App';
import './index.css';

ReactDOM.createRoot(document.getElementById("root")).render(
  <BrowserRouter>
    <App/>
  </BrowserRouter>
);