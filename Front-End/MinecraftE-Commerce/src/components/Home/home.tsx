//Imports
//****************//


import axios from "axios";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import '../../styles/home.css';
import lupa from '../images/lupa.png';
import UserMenu from "./UserMenu";


//****************//


function HomeMain(){
    //Declarations 
    //****************//


    const navigate = useNavigate();
    const pfp = localStorage.getItem("pfp");
    const token = localStorage.getItem("token");
    let arr = ['Quer deixar seu mundo mais bonito?', 'Plugins?', 'Que tal uma pesquisa', 'Gosta de criar mods?'];
    const randomIndex = Math.floor(Math.random() * arr.length);
    const randomElement = arr[randomIndex];


    //****************//

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
    

    interface Image{
        imagePath: string
    }

    
interface Announcement {
  id: number;
  title: string;
  description: string;
  priceService: number;
  createdAt: string;
  images: Image[];
  userName: string;
  userPfp: string;
  typeAnnouncement: number;
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
    const data: Announcement[] = response.data;

    setPlugins(data.filter(a => a.typeAnnouncement === 0));
    setMods(data.filter(a => a.typeAnnouncement === 1));
    setConstrucoes(data.filter(a => a.typeAnnouncement === 2));
    setServicos(data.filter(a => a.typeAnnouncement === 3));
    setAjuda(data.filter(a => a.typeAnnouncement === 4));
    setSeeds(data.filter(a => a.typeAnnouncement === 5));
    setSkins(data.filter(a => a.typeAnnouncement === 6));
}

    useEffect(() => {
        display(); 
    }, [])


    if (announcements != null) {
        useEffect(() => {
            
        }, [announcements])
    }       

    async function redirectAndAddMoreClick(idAnnouncement: number){   

        navigate(`/announcements/${idAnnouncement}`);

        if(token !== null){
            const addClicked = await fetch(`https://localhost:7253/api/v1/${idAnnouncement}`, {
                method: 'PUT',
                headers: {
                    "Authorization": "Bearer " + token,
                }
            })

            const data = await addClicked.json();
            console.log(data);
        }
        
        else{
            const addClicked = await fetch(`https://localhost:7253/api/v1/${idAnnouncement}`, {
                method: 'PUT',
            })

            const dataClicked = await addClicked.json();
            console.log(dataClicked);
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
                        onClick={() => redirectAndAddMoreClick(announcement.id)}
                        className="cardAnnouncement"
                        key={announcement.id}
                    >
                        <img
                            width={100}
                            className="imageadd"
                            src={`https://localhost:7253/${announcement.images[0].imagePath}`}
                        />
                        <div className="infoAnuncio">
                            <img
                                alt="Imagem do anuncio"
                                className="userPfpInInfo"
                                width={20}
                                src={`https://localhost:7253/${announcement.userPfp}`}
                            />
                            <p className="title">{announcement.title}</p>
                            <p className="description">{announcement.description}</p>
                            <small className="price">R$: {announcement.priceService}</small>
                            <br />
                            <small className="datetime">{announcement.createdAt}</small>
                            <small className="type">Tipo: {announcement.typeAnnouncement}</small>
                        </div>
                    </div>
                ))}
            </div>
        </section>
    );
   }
    
    return(
        <div className="appMain">

            <head>
            <link href="https://fonts.googleapis.com/css2?family=Roboto+Mono:wght@300;400;500&display=swap" rel="stylesheet" />
            </head>

            <header className="headerHome">
            <input className="inptSearch" id="inpSearch" type="search" onChange={(e) => pesquisarAnuncios(e.target.value)} placeholder={randomElement}/>
            <label className="lblSearch" htmlFor="inpSearch"><img width={27} height={27} src={lupa}/></label>

            <div className="links">
                <a onClick={overViewPages}>My announcements</a>
                <a href="#">About</a>
                <a href="#">Terms of use</a>
                <a href="https://github.com/Davi-y08/MinecraftE-Commerce">Project</a>
            </div>

            <UserMenu/>

            </header>
            <div className="contentSite">
                {sections.map(section => renderSection(section.title, section.data))}
            </div>
        </div>
    )
}

export default HomeMain;