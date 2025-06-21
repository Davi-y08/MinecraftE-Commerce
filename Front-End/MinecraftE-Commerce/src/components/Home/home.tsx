//Imports
//****************//


import axios from "axios";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import '../../styles/home.css';
import lupa from '../images/lupa.png';
import UserMenu from "./UserMenu";
import SectionAnnouncements from "./SectionAnnouncements";

//****************//


function HomeMain(){

    //Interfaces
    //****************//

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

            <SectionAnnouncements/>
        </div>
    )
}

export default HomeMain;