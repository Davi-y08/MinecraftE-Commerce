import axios from "axios";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import '../styles/home.css'
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

    interface Announcement{
        createdAt: string,
        descripton: string,
        id: number,
        imageAnnouncement: string,
        priceService: number,
        title: string,
        userId: string,
        userName: string,
        userPfp: string
    }

    const [announcements, setAnnouncements] = useState<Announcement[]>([]);

    async function display() {
        const response = await axios.get('https://localhost:7253/api/v1/GetAll');
        const data = response.data;
        setAnnouncements(data);
    }

    useEffect(() => {
        display(); 
    }, [])


    if (announcements != null) {
        useEffect(() => {
            console.log(announcements);
        }, [announcements])
    }       

    function redirect(idAnnouncement: number){   
        navigate(`/announcements/${idAnnouncement}`);
    }
    
    return(
        <div>
            <header>
            <label htmlFor="inpSearch">Pesquisar</label>
            <input id="inpSearch" type="search" placeholder={randomElement}/>

            <div className="links">
                <a href="#">My announcements</a>
                <a href="#">About</a>
                <a href="#">Terms of use</a>
                <a href="https://github.com/Davi-y08/MinecraftE-Commerce">Project</a>
            </div>

            <div className="menuUser">
                <p>{notLog}</p>
                <img onClick={loginPage} src={`https://localhost:7253/${pfp}`} className="pfpUser" width={50}/>
                <button onClick={logout}>Sair</button>
            </div>
            </header>

            <div className="contentSite">
                    {announcements?.map((announcement: Announcement) => (
                        <div onClick={() => redirect(announcement.id)} className="cardAnnouncement" key={announcement.id}>
                            <img className="imageadd" width={50} src={`https://localhost:7253/${announcement.imageAnnouncement}`}/>
                            <p className="username">{announcement.userName}</p>
                            <p className="title">{announcement.title}</p>
                            <img className="userpfp" width={20} src={`https://localhost:7253/${announcement.userPfp}`}/>
                            <p className="description">{announcement.descripton}</p>
                            <small className="price">{announcement.priceService}</small>
                            <br />
                            <small className="datetime">{announcement.createdAt}</small>
                        </div>
                    ))}
            </div>
        </div>  
    )
}

export default HomeMain;