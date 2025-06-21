import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";

export default function SectionAnnouncements(){

    const navigate = useNavigate();
    const token = localStorage.getItem('token');

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

    return <>
        <div className="contentSite">
                {sections.map(section => renderSection(section.title, section.data))}
        </div>
    </>
}