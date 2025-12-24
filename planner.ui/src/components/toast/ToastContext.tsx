import React, { createContext, useContext, useState } from 'react';

type Toast = { id: string; message: string };

const ToastContext = createContext({ show: (m: string) => {} });

export const useToast = () => useContext(ToastContext);

export const ToastProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [toasts, setToasts] = useState<Toast[]>([]);

  function show(message: string) {
    const id = `${Date.now()}-${Math.random().toString(36).slice(2, 9)}`;
    setToasts((s) => [...s, { id, message }]);
    setTimeout(() => setToasts((s) => s.filter((t) => t.id !== id)), 4000);
  }

  return (
    <ToastContext.Provider value={{ show }}>
      {children}
      <div style={{ position: 'fixed', right: 16, bottom: 16, zIndex: 9999 }}>
        {toasts.map((t) => (
          <div key={t.id} className="toast">{t.message}</div>
        ))}
      </div>
    </ToastContext.Provider>
  );
};

export default ToastProvider;
