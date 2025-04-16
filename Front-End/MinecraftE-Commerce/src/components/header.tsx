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
    let arr = ['Quer deixar seu mundo mais bonito?', 'Plugins legais', 'Que tal uma pesquisa', 'Gosta de criar mods?'];
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
        userPfp: string,
        typeOfAnnouncement: number
    }

    const [announcements, setAnnouncements] = useState<Announcement[]>([]);
    const [plugins, setPlugins] = useState<Announcement[]>([]);
    const [mods, setMods] = useState<Announcement[]>([]);
    const [construcoes, setConstrucoes] = useState<Announcement[]>([]);
    const [servicos, setServicos] = useState<Announcement[]>([]);
    const [ajuda, setAjuda] = useState<Announcement[]>([]);
    const [seeds, setSeeds] = useState<Announcement[]>([]);
    const [skins, setSkins] = useState<Announcement[]>([]);

    async function display() {
        const response = await axios.get('https://localhost:7253/api/v1/GetInRandomOrder');
        const data: Announcement[] = await response.data;

        setPlugins(data.filter(a => a.typeOfAnnouncement === 0));
        setMods(data.filter(a => a.typeOfAnnouncement === 1));
        setConstrucoes(data.filter(a => a.typeOfAnnouncement === 2));
        setServicos(data.filter(a => a.typeOfAnnouncement === 3));
        setAjuda(data.filter(a => a.typeOfAnnouncement === 4));
        setSeeds(data.filter(a => a.typeOfAnnouncement === 5));
        setSkins(data.filter(a => a.typeOfAnnouncement === 6));
    }

    useEffect(() => {
        display(); 
    }, [])


    if (announcements != null) {
        useEffect(() => {
            
        }, [announcements])
    }       

    function redirect(idAnnouncement: number){   
        navigate(`/announcements/${idAnnouncement}`);
    }

    function navToCreateAd(){
        navigate('/createad');
    }

    const isLogged = token !== null;
    
    async function pesquisarAnuncios(strSearch: string) {
        try {
            const responseSearch = 
            axios.get
            (`https://localhost:7253/api/v1/SearchAn?strSearch=${strSearch}`);

            const data = await (await responseSearch).data;

            const list = document.getElementById('listTitlesSearch');

            if(list){
                list.innerHTML = '';

                data.forEach((announcement: Announcement) => {
                const listItem = document.createElement('li');
                listItem.textContent = announcement.title;
                list.appendChild(listItem);

                if(strSearch == ''){                    
                   list.innerHTML = '';
                }

            });
            }
        } 
        
        catch (error) {
            console.log(error);
        }
    }

   // const response = await fetch("https://localhost:7253/api/v1/Login", {
       // method: "POST",
      //  headers: {
        //    "Content-Type": "application/json",
        //    "Authorization": "Bearer " + token,
       // },
      //  body: JSON.stringify({
           // 'emailforlogin': email,
           // 'passwordforlogin': password
        //}),
   // });

   const sections = [
    {title : 'Plugins', data: plugins},
    {title : 'Mods', data: mods},
    {title : 'Construções', data: construcoes},
    {title : 'Serviços', data: servicos},
    {title : 'Ajuda', data: ajuda},
    {title : 'Seeds', data: seeds},
    {title : 'Skins', data: skins},
   ]

   function renderSection(title: string, data: Announcement[]){
    if(data.length === 0){
        return null;
    }

    return(
        <section className="sectionHome" key={title}>
            <h2>{title}</h2>
            <div className="gridAnuncios">
                {data.map((announcement) => (
                    <div
                        onClick={() => redirect(announcement.id)}
                        className="cardAnnouncement"
                        key={announcement.id}
                    >
                        <img
                            width={100}
                            className="imageadd"
                            src={`https://localhost:7253/${announcement.imageAnnouncement}`}
                        />
                        <div className="infoAnuncio">
                            <img
                                className="userPfpInInfo"
                                width={20}
                                src={`https://localhost:7253/${announcement.userPfp}`}
                            />
                            <p className="title">{announcement.title}</p>
                            <p className="description">{announcement.descripton}</p>
                            <small className="price">R$: {announcement.priceService}</small>
                            <br />
                            <small className="datetime">{announcement.createdAt}</small>
                            <small className="type">Tipo: {announcement.typeOfAnnouncement}</small>
                        </div>
                    </div>
                ))}
            </div>
        </section>
    );
   }
    
    return(
        <div className="appMain">

            <head>9
            <link href="https://fonts.googleapis.com/css2?family=Roboto+Mono:wght@300;400;500&display=swap" rel="stylesheet" />
            </head>

            <header className="headerHome">
            <input className="inptSearch" id="inpSearch" type="search" onChange={(e) => pesquisarAnuncios(e.target.value)} placeholder={randomElement}/>
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
            {sections.map(section => renderSection(section.title, section.data))}
            </div>
        </div>
    )
}

export default HomeMain;