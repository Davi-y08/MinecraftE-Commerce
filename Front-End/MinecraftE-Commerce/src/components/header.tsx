import { useNavigate } from "react-router-dom";

function HeaderMain(){
    const navigate = useNavigate();

    async function loginPage() {
        navigate('/login');
    }

    return(
        <div>
            <label htmlFor="inpSearch"><img src=""/>

            </label>
            <input id="inpSearch" type="text" placeholder="Quer deixar seu mundo mais bonito?"/>

            <div className="links">
                <a href="#">My announcements</a>
                <a href="#">About</a>
                <a href="#">Terms of use</a>
            </div>

            <div className="menuUser">
                <img onClick={loginPage} src="pfp" className="pfpUser"/>
            </div>
        </div>  
    )
}

export default HeaderMain;