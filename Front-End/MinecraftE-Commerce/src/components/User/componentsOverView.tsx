import axios from "axios";
import { useEffect, useState } from "react";

export function SobreMim(){
    const token = localStorage.getItem('token');
    const [clicksInMounth, setClicksInMounth] = useState('');

    

    async function returnClicks() {
            const response = await fetch('https://localhost:7253/api/v1/cliquesem30dias', {
                method: 'GET',
                headers: {
                    "Authorization": "Bearer " + token,
                }
            })
    
            const data = await response.json();
            console.log(data);
            setClicksInMounth(data);
            console.log(clicksInMounth);
        }

        useEffect(() => {
            returnClicks();
        },[])

    return(
        <div>
            <p>{clicksInMounth}</p>
        </div>
    )
}

export function MinhasCompras(){

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

    const [compras, setCompras] = useState<Announcement[]>([]);
    const token = localStorage.getItem('token');
    
    async function getCompras() {
        const reponse = await axios.get('https://localhost:7253/api/v1/getPurchasesByUser', {
            headers: {
                'Authorization': 'Bearer ' + token,
            }
        })
        .then(response => {
            setCompras(response.data);
        })
    }

    useEffect(() => {
        getCompras();
    },[])

    return(
        <div>
            <h2>Minhas Compras</h2>
        {compras.map(compra => (
            <div key={compra.id}>
                <h3>{compra.title}</h3>
                <p>{compra.descripton}</p>
                <p>Preço: R$ {compra.priceService}</p>
                <img src={compra.imageAnnouncement} alt={compra.title} width={200} />
            </div>
        ))}
        </div>
    )
}

export function MeusAnuncios(){
    return(
        <div>
            <p></p>
        </div>
    )
}