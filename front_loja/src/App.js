import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import BarraLateral from './components/BarraLateral';
import FormularioCategoria from './components/formularios/FormularioCategoria';
import ListaCategorias from './components/formularios/ListaCategorias';
import FormularioProduto from './components/formularios/FormularioProduto';
import ListaProdutos from './components/formularios/ListaProdutos';
import Login from './pages/Login';
import Cadastro from './pages/Cadastro';
import Painel from './pages/Painel';

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem('token');
    setIsAuthenticated(!!token);
  }, []);

  const handleLogin = () => {
    setIsAuthenticated(true); 
  };

  const handleLogout = () => {
    localStorage.removeItem('token'); 
    setIsAuthenticated(false); 
  };

  return (
    <Router>
      <div className="flex h-screen">
        {isAuthenticated && <BarraLateral onLogout={handleLogout} />}
        <div className={`flex-1 ${isAuthenticated ? 'ml-64' : 'w-full'}`}>
          <Routes>
            <Route
              path="/login"
              element={isAuthenticated ? <Navigate to="/dashboard" replace /> : <Login onLogin={handleLogin} />}
            />
            <Route path="/register" element={<Cadastro />} />
            {isAuthenticated ? (
              <>
                <Route path="/dashboard" element={<Painel />} />
                <Route path="/categorias/nova" element={<FormularioCategoria />} />
                <Route path="/categorias" element={<ListaCategorias />} />
                <Route path="/produtos/novo" element={<FormularioProduto />} />
                <Route path="/produtos" element={<ListaProdutos />} />
              </>
            ) : (
              <Route path="*" element={<Navigate to="/login" replace />} />
            )}
          </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;
