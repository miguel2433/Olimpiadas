import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Nav from './Navbar.jsx';
import Home from './Home';
import Login from './Login';
import MisCompras from './MisCompras.jsx';

export default function App() {
  return (
      <Router>
        <Nav />
        <Routes>
          <Route path="/home" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route path='/miscompras' element={<MisCompras/>}/>
        </Routes>
      </Router>
  );
}