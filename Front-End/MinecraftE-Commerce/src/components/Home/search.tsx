import axios from "axios"
import lupa from '../images/lupa.png';

export default function Search(){

let arr = ['Quer deixar seu mundo mais bonito?', 'Plugins?', 'Que tal uma pesquisa', 'Gosta de criar mods?'];
const randomIndex = Math.floor(Math.random() * arr.length);
const randomElement = arr[randomIndex];

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

    function auxiliar(strSearch: string){
        pesquisarAnuncios(strSearch);
    }

    async function pesquisarAnuncios(strSearch: string){
        try {
            const responseSearch = 
            axios.get
            (`https://localhost:7253/api/v1/SearchAn?strSearch=${strSearch}`);

            const data = await (await responseSearch).data;
            const list = document.getElementById('listTitlesSearch');

            if(list){
                list.innerHTML = '';

                if(strSearch === ""){
                    return;
                }

                data.forEach((announcement: Announcement) => {
                const listItem = document.createElement('li');
                listItem.textContent = announcement.title;
                list.appendChild(listItem);
            });
            }
        } 
        
        catch (error) {
            console.log(error);
        }
    }

    return <>
            <input className="inptSearch" id="inpSearch" type="search" onChange={(e) => auxiliar(e.target.value)} placeholder={randomElement}/>
            <label className="lblSearch" htmlFor="inpSearch"><img width={27} height={27} src={lupa}/></label>

            <ul id="listTitlesSearch"></ul>
    </>
}