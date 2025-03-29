import axios from "axios";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import '../styles/home.css'
import lupa from '../images/lupa.png'

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
    
    function verifyLog(){
        if(token == null){
            loginPage();
        }
        else{
            exibirUserMenu();
        }
    }

    function exibirUserMenu(){
        const displayMenuLateral = document.getElementById('menuLateral');
        if (displayMenuLateral) {
            displayMenuLateral.classList.add('show');
        }
    }

    useEffect(() => {

        const handleClick = (event: MouseEvent) => {
            const menulateral = document.getElementById('menuLateral');
            if (menulateral && !menulateral.contains(event.target as Node)) {
                hideMenuLateral();
            }
        };

        document.addEventListener('mousedown', handleClick);

        return () => {
            document.removeEventListener('mousedown', handleClick);
        }

    },[])
   

    function hideMenuLateral(){
        const displayMenuLateral = document.getElementById('menuLateral');
        if (displayMenuLateral) {
            displayMenuLateral.classList.remove('show');
        }
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

    function navToCreateAd(){
        navigate('/createad');
    }

    const isLogged = token !== null;

    
    return(
        <div className="appMain">

            <head>
            <link href="https://fonts.googleapis.com/css2?family=Roboto+Mono:wght@300;400;500&display=swap" rel="stylesheet" />
            </head>

            <header className="headerHome">
            <input className="inptSearch" id="inpSearch" type="search" placeholder={randomElement}/>
            <label className="lblSearch" htmlFor="inpSearch"><img width={27} height={27} src={lupa}/></label>

            <div className="links">
                <a href="#">My announcements</a>
                <a href="#">About</a>
                <a href="#">Terms of use</a>
                <a href="https://github.com/Davi-y08/MinecraftE-Commerce">Project</a>
            </div>

            <div className="menuUser">
                <p>{notLog}</p>
                <img id="pfpUserHead" onClick={verifyLog} src={`https://localhost:7253/${pfp}`} className="pfpUser" width={50}/>
            </div>
            
            <div id="menuLateral" className="MenuLateral">
                <img className="pfpInMenu" src={`https://localhost:7253/${pfp}`} width={45}/>
                <button className="myProfile">My Profile</button>
                <button className="myAds">My ads</button>
                <button className="darkTheme">Dark theme</button>
                <button className="configs">Configs</button>
                <button onClick={navToCreateAd} className="createAd">Create ad</button>
                {
                     isLogged && <button onClick={logout}>Sair</button>
                }
            </div>

            </header>
                
                

            <div className="contentSite">
                    {announcements?.map((announcement: Announcement) => (
                        <div onClick={() => redirect(announcement.id)} className="cardAnnouncement" key={announcement.id}>
                            <div className="divImage">
                                <img className="imageadd" src={`https://localhost:7253/${announcement.imageAnnouncement}`}/>
                            </div>
                        <div className="infoAnuncio">
                            <img className="userPfpInInfo" width={20} src={`https://localhost:7253/${announcement.userPfp}`}/>
                            <p className="title">{announcement.title}</p>
                            <p className="description">{announcement.descripton}</p>
                            <small className="price">{announcement.priceService}</small>
                            <br />
                            <small className="datetime">{announcement.createdAt}</small>]
                        </div>
                        </div>
                    ))}
            </div>
        </div>
    )
}

export default HomeMain;