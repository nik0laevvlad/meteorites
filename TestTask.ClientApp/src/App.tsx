import './App.css';
import { NotificationProvider } from './components';
import { MeteoritesTablePanel } from './views';

function App() {
  return (
    <>
      <NotificationProvider>
        <MeteoritesTablePanel />
      </NotificationProvider>
    </>
  );
}

export default App;
