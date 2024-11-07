import React from 'react';
import { Link } from 'react-router-dom';
import './custom.css';

export default function Nav() {
  return (
    <>
      <header className='transition-all duration-300 sticky top-0 flex bg-cyanOscuro text-white justify-center items-center w-full h-10vh'>
        <nav className='flex w-full justify-around items-center '>
          <h1 className='font-bold text-2xl'><Link to="/home">Home</Link></h1>
          <ul className='flex justify-center items-center'>
            <li><Link className='mr-4' to="/miscompras">Mis Compras</Link></li>
            <li><Link to="/login" className='hover:bg-white hover:text-black cursor-pointer duration-300 transition-all border-2 border-white py-4 px-8 rounded-md'>Login</Link></li>
          </ul>
        </nav>
      </header>
    </>
  );
}
    