import { useNavigate } from "react-router-dom";
import '../../styles/home.css';

export default function UserMenu(){
    const pfp = localStorage.getItem("pfp");
    const token = localStorage.getItem("token")
    const navigate = useNavigate();
    var notLog;

    async function loginPage() {
        navigate('/login');
    }

    function exibirUserMenu(){
        const displayMenuLateral = document.getElementById('menuLateral');
        if (displayMenuLateral) {
            displayMenuLateral.classList.add('show');
        }
    }

    if (pfp == null && token == null) {
        notLog = "SignIn/SignUp"    
    }

    function verifyLog(){
        if(token == null){
            loginPage();
        }
        else{   
            exibirUserMenu();
        }
    }

    async function overViewPages() {
        if(localStorage.getItem('token') === null){
            navigate('/notLogged');
        }
        else {
            navigate('/myAnnouncements');
        }   
    }

    function navToCreateAd(){
        navigate('/createad');
    }

    const isLogged = token !== null;

    function logout() {
        localStorage.removeItem('pfp');
        localStorage.removeItem("token"); 
        location.reload();       
    }

    return<>
        <div className="menuUser">
                <p>{notLog}</p>
                <img id="pfpUserHead" onClick={verifyLog} src={`https://localhost:7253/${pfp}`} className="pfpUser" width={50}/>
            </div>
            
            <div id="menuLateral" className="MenuLateral">
                <img className="pfpInMenu" src={`https://localhost:7253/${pfp}`} width={45}/>
                <button onClick={overViewPages} className="myProfile">My Profile</button>
                <button  className="myAds">My ads</button>
                <button className="darkTheme">Dark theme</button>
                <button className="configs">Configs</button>
                <button onClick={navToCreateAd} className="createAd">Create ad</button>
                {
                     isLogged && <button onClick={logout}>Sair</button>
                }
            </div>
    </>
}