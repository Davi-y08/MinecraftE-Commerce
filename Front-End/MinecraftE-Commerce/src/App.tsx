import Login from "./components/login";
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from 'react-query';
import HomeMain from "./components/header";
import AnnouncementPage from "./components/announcement";
import CreateAnnouncementPage from "./components/createad";

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
        </Routes>
      </Router>
      </QueryClientProvider>
    </>
  )
}

export default App
