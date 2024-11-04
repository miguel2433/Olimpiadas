import React from 'react';
import { Link } from 'react-router-dom';

export default function Nav() {
  return (
    <>
      <header className='flex justify-center items-center w-full h-10vh'>
        <nav>
          <ul className='flex justify-center items-center'>
            <li><Link to="/home">Home</Link></li>
            <li><Link to="/login">Login</Link></li>
          </ul>
        </nav>
      </header>
    </>
  );
}
    