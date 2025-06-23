//Imports
//****************//

import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import '../../styles/home.css';
import UserMenu from "./UserMenu";
import SectionAnnouncements from "./SectionAnnouncements";
import Search from "./search";

//****************//


function HomeMain(){

    //Interfaces
    //****************//

    //Declarations 
    //****************//


    const navigate = useNavigate();


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
    
    return(
        <div className="appMain">

            <head>
            <link href="https://fonts.googleapis.com/css2?family=Roboto+Mono:wght@300;400;500&display=swap" rel="stylesheet" />
            </head>

            <header className="headerHome">
            
            <Search/>

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