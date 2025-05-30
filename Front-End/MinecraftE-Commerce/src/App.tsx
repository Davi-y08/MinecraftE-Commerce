import Login from "./components/User/login";
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from 'react-query';
import HomeMain from "./components/Home/home";
import AnnouncementPage from "./components/Announcements/announcement";
import CreateAnnouncementPage from "./components/Announcements/createad";
import MyAnnouncementsPage from "./components/User/overview";
import HelpPage from "./components/HelperPages/helpLoginAndRegister";
import Chat from "./components/Sale/Chat";
function App() {

  const queryClient = new QueryClient();

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
          <Route path="/chatSale/:idUser/:chatId" element = {<Chat/>}/>
        </Routes>
      </Router>
      </QueryClientProvider>
    </>
  )
}

export default App
