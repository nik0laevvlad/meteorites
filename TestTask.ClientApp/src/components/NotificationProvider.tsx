import { notification } from 'antd';
import { createContext, useContext } from 'react';

const NotificationContext = createContext<
  ReturnType<typeof notification.useNotification>[0] | null
>(null);

export const NotificationProvider = ({
  children,
}: {
  children: React.ReactNode;
}) => {
  const [api, contextHolder] = notification.useNotification();

  return (
    <NotificationContext.Provider value={api}>
      {contextHolder}
      {children}
    </NotificationContext.Provider>
  );
};

export const useNotify = () => {
  const api = useContext(NotificationContext);
  if (!api) {
    throw new Error('useNotify must be used within NotificationProvider');
  }
  return api;
};
