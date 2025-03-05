import axios from "axios";
import { useEffect, useState } from "react";
import { useQueries, useQuery } from "react-query";
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
    
    return(
        <div>
            <header>
            <label htmlFor="inpSearch">Pesquisar</label>
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
            </header>

            <div className="contentSite">
                    {announcements?.map((announcement: Announcement) => (
                        <div className="cardAnnouncement" key={announcement.id}>
                            <img width={50} src={`https://localhost:7253/${announcement.imageAnnouncement}`}/>
                            <p>{announcement.title}</p>
                            <p>{announcement.descripton}</p>
                            <small>{announcement.priceService}</small>
                            <br />
                            <small>{announcement.createdAt}</small>
                        </div>
                    ))}
            </div>
        </div>  
    )
}

export default HomeMain;