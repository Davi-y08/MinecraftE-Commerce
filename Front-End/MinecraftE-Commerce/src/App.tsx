import Login from "./components/User/login";
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from 'react-query';
import HomeMain from "./components/Home/home";
import AnnouncementPage from "./components/Announcements/announcement";
import CreateAnnouncementPage from "./components/Announcements/createad";
import MyAnnouncementsPage from "./components/User/overview";
import HelpPage from "./components/HelperPages/helpLoginAndRegister";
import { useState, useEffect} from 'react'
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import Chat from "./components/Sale/Chat";

function App() {

  const queryClient = new QueryClient();

  const [connection, setConnection] = useState<HubConnection | null>(null);
    const [messages, setMessages] = useState([]);
    
      
      
      useEffect(() => {
        async function connectionSignal(){
          if (connection) 
            return
          
          try {
  
            const connection = new HubConnectionBuilder()
              .withUrl("https://localhost:7198/hub")
              .configureLogging(LogLevel.Information)
              .build();
     
            connection.on("ReceivedMessage", (message) => {
              setMessages((messages) => [...messages, message]);
            });

            await connection.start();
            
            setConnection(connection); 
     
            console.log("Conexão concluída com sucesso", connection);
     
          } catch (error) {
            console.log(error);
          }
        }
        connectionSignal();      
      }, [])


  return (
    <>
    <QueryClientProvider client={queryClient}>
      <Router>
        <Routes>
          <Route path="/" element = {<HomeMain></HomeMain>} />
          <Route path="/login" element = {<Login></Login>}/>
          <Route path="/announcements/:id" element = {<AnnouncementPage/>}/>
          <Route path="/createad" element = {<CreateAnnouncementPage/>}/>
          <Route path="/myAnnouncements" element = {<MyAnnouncementsPage/>}/>
          <Route path="/notLogged" element = {<HelpPage/>}/>
        </Routes>
      </Router>
      </QueryClientProvider>

      <Chat connection={connection}/> 
      <div>
        <h2>Mensagem recebidas: </h2>
        <ul>
          {messages.map((message, index) => (
            <li key={index}>
                <strong>{message.userName}</strong>: {message.message}
                --
            </li>
          ))}
        </ul>
      </div>
    </>
  )
}

export default App
