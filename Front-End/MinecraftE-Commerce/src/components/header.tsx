import { useNavigate } from "react-router-dom";

function HomeMain(){
    const navigate = useNavigate();
    const pfp = localStorage.getItem("pfp");
    const token = localStorage.getItem("token");
    var notLog;
    let arr = ['Quer deixar seu mundo mais bonito?', 'Plugins legais', 'Que tal uma pesquisa', 'Gosta de criar mods?']
    const randomIndex = Math.floor(Math.random() * arr.length);
    const randomElement = arr[randomIndex];

    if (pfp == null && token == null) {
        notLog = "SignIn/SignUp"
    }

    async function loginPage() {
        navigate('/login');
    }

    async function logout() {
        localStorage.removeItem('pfp');
        localStorage.removeItem("token"); 
        location.reload();       
    }

    return(
        <div>
            <label htmlFor="inpSearch"><img src=""/>

            </label>
            <input id="inpSearch" type="search" placeholder={randomElement}/>

            <div className="links">
                <a href="#">My announcements</a>
                <a href="#">About</a>
                <a href="#">Terms of use</a>
            </div>

            <div className="menuUser">
                <p>{notLog}</p>
                <img onClick={loginPage} src={`https://localhost:7253/${pfp}`} className="pfpUser" width={50}/>
                <button onClick={logout}>Sair</button>
            </div>
        </div>  
    )
}

export default HomeMain;