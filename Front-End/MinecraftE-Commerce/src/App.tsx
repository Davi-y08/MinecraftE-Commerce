import HeaderMain from "./components/header"
import Login from "./components/login";
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from 'react-query';

function App() {

  const queryClient = new QueryClient();

  return (
    <>
    <QueryClientProvider client={queryClient}>
      <Router>
        <Routes>
          <Route path="/" element = {<HeaderMain></HeaderMain>} />
          <Route path="/login" element = {<Login></Login>}/>
        </Routes>
      </Router>
      </QueryClientProvider>
    </>
  )
}

export default App
