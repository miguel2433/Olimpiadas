import React, { useState, useRef, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './custom.css';

export default function Login() {
    const [isLogin, setIsLogin] = useState(true);
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [email, setEmail] = useState('');
    const passwordRef = useRef(null);
    const confirmPasswordRef = useRef(null);
    const [userData, setUserData] = useState(null);
    
    const navigate = useNavigate(); // Hook para redirección

    const toggleForm = () => {
        setIsLogin(!isLogin);
    };

    const handlePassword = (ref) => {
        const icon = ref.current.nextElementSibling.querySelector('ion-icon');
        icon.setAttribute('name', ref.current.type === 'password' ? 'lock-closed' : 'lock-open-outline');
    };

    const togglePasswordVisibility = (ref) => {
        ref.current.type = ref.current.type === 'password' ? 'text' : 'password';
        handlePassword(ref);
    };

    const handleSubmit = async (e) => { // Cambiado para que la función sea async
        e.preventDefault();
        if (isLogin) {
            // Lógica para iniciar sesión
            const loginData = {
                Email: email,
                Password: password,
            };
            try { // Mover el bloque try aquí
                const response = await fetch('http://localhost:5249/api/auth/login', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(loginData),
                });
    
                if (response.ok) {
                    const data = await response.json();
                    console.log('Usuario autenticado:', data);
                    navigate('/home'); // Redirección a /home en caso de éxito
                } else {
                    console.error('Error al iniciar sesión:', response.statusText);
                }
            } catch (error) {
                console.error('Error en la solicitud:', error);
            }}
        else {
            if (password === confirmPassword) {
                const userData = {
                    Nombre: username,
                    NombreUsuario: username,
                    Apellido: username,
                    Email: email,
                    Password: password,
                    Telefono: '1233'
                };
                setUserData(userData);
            } else {
                console.error("Las contraseñas no coinciden");
            }
        }
    };

    useEffect(() => {
        const registerUser = async () => {
            if (userData) {
                try {
                    const response = await fetch('http://localhost:5249/api/usuario/__CONTRA__', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(userData),
                    });

                    if (response.ok) {
                        const data = await response.json();
                        console.log('Usuario registrado:', data);
                        navigate('/home'); // Redirección a /home en caso de éxito
                    } else {
                        console.error('Error al registrar el usuario:', response.statusText);
                    }
                } catch (error) {
                    console.error('Error en la solicitud:', error);
                }
            }
        };

        registerUser();
    }, [userData, navigate]);


    return (
        <div className="flex justify-center items-center min-h-screen p-4 bg-cyanOscuro">
            <div className="shadow-zinc-900 shadow-xl relative w-96 h-auto bg-transparent border-2 border-white rounded-2xl backdrop-blur-md p-8">
                <h1 className="text-2xl text-white text-center mb-6">{isLogin ? 'Inicia Sesión' : 'Regístrate'}</h1>
                <form method="post" onSubmit={handleSubmit}>
                    <div className="mb-4">
                        <label className="duration-300 font-medium text-base block text-white mb-1">Email</label>
                        <div className="flex items-center border-b-2 border-white">
                            <input 
                                type="email" 
                                value={email} 
                                onChange={(e) => setEmail(e.target.value)} 
                                className="w-full h-12 bg-transparent text-white focus:outline-none pl-2" 
                                required 
                            />
                            <ion-icon style={{ color: 'white' }} name="mail-outline"></ion-icon>
                        </div>
                    </div>
                    <div className="mb-4">
                        <label className="duration-300 font-medium text-base block text-white mb-1">Contraseña</label>
                        <div className="flex items-center border-b-2 border-white">
                            <input 
                                type="password" 
                                value={password} 
                                onChange={(e) => setPassword(e.target.value)} 
                                className="w-full h-12 bg-transparent text-white focus:outline-none pl-2" 
                                required 
                                ref={passwordRef}
                            />
                            <span className="icon toggle-password" onClick={() => {
                                togglePasswordVisibility(passwordRef);
                            }}>
                                <ion-icon name="lock-closed" style={{ color: 'white' }}></ion-icon>
                            </span>
                        </div>
                    </div>
                    {!isLogin && (
                        <>
                            <div className="mb-4">
                                <label className="font-medium text-base duration-300 block text-white mb-1">Usuario</label>
                                <div className="flex items-center border-b-2 border-white">
                                    <input 
                                        type="text" 
                                        value={username} 
                                        onChange={(e) => setUsername(e.target.value)} 
                                        className="w-full h-12 bg-transparent text-white focus:outline-none pl-2" 
                                        required 
                                    />
                                    <span className="icon">
                                        <ion-icon name="person" style={{ color: 'white' }}></ion-icon>
                                    </span>
                                </div>
                            </div>
                            <div className="mb-4">
                                <label className="duration-300 font-medium text-base block text-white mb-1">Verificar Contraseña</label>
                                <div className="flex items-center border-b-2 border-white">
                                    <input 
                                        type="password" 
                                        value={confirmPassword} 
                                        onChange={(e) => setConfirmPassword(e.target.value)} 
                                        className="w-full h-12 bg-transparent text-white focus:outline-none pl-2" 
                                        required 
                                        ref={confirmPasswordRef}
                                    />
                                    <span className="icon toggle-password" onClick={() => {
                                        togglePasswordVisibility(confirmPasswordRef);
                                    }}>
                                        <ion-icon name="lock-closed" style={{ color: 'white' }}></ion-icon>
                                    </span>
                                </div>
                            </div>
                        </>
                    )}
                    <button type="submit" className="font-medium text-base w-full mt-4 h-12 bg-white text-gray-800 rounded-md hover:border-2 hover:text-white hover:bg-cyanOscuro transition duration-300">
                        {isLogin ? 'Login' : 'Registrate'}
                    </button>
                </form>
                <div className="text-center text-white mt-4">
                    <p>{isLogin ? 'No tienes una cuenta?' : 'Ya tienes una cuenta?'}</p>
                    <button onClick={toggleForm} className="text-white hover:underline">
                        {isLogin ? 'Registrate' : 'Inicia Sesión'}
                    </button>
                </div>
            </div>
        </div>
    );
}
